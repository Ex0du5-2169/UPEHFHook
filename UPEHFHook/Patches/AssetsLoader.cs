using BepInEx;
using HarmonyLib;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UPEHFHook;

namespace UPEHFHook.Patches
{
    static class AssetsLoader
    {
        private static readonly Dictionary<string, SkeletonDataAsset> cache = new Dictionary<string, SkeletonDataAsset>();

        // Pads the texture to a multiple of 4 by bleeding the edge pixels into the padded area (same result as the game's WorkshopNPCManager.PadToMultipleOf4_CPU for readable textures).
        private static Texture2D PadToMultipleOf4(Texture2D src)
        {
            int width = src.width;
            int height = src.height;
            int w = (width + 3) & -4;
            int h = (height + 3) & -4;
            if (w == width && h == height)
                return src;

            Color[] srcPixels = src.GetPixels(0, 0, width, height);
            Texture2D padded = new Texture2D(w, h, TextureFormat.RGBA32, mipChain: false, linear: false);
            padded.SetPixels(0, 0, width, height, srcPixels);

            if (w > width)
            {
                Color[] rightColumn = src.GetPixels(width - 1, 0, 1, height);
                for (int x = width; x < w; x++)
                    padded.SetPixels(x, 0, 1, height, rightColumn);
            }
            if (h > height)
            {
                Color[] bottomRow = src.GetPixels(0, height - 1, width, 1);
                for (int y = height; y < h; y++)
                    padded.SetPixels(0, y, width, 1, bottomRow);
            }
            if (w > width && h > height)
            {
                Color corner = src.GetPixel(width - 1, height - 1);
                Color[] cornerBlock = new Color[(w - width) * (h - height)];
                for (int i = 0; i < cornerBlock.Length; i++)
                    cornerBlock[i] = corner;
                padded.SetPixels(width, height, w - width, h - height, cornerBlock);
            }

            padded.Apply(updateMipmaps: false, makeNoLongerReadable: false);
            UnityEngine.Object.Destroy(src);
            return padded;
        }

        // Clears opaque pure magenta pixels (they show as purple outlines at sprite borders) and premultiplies alpha in a single readback/upload pass.
        private static void ProcessPixels(Texture2D tex, bool clearMagentaKey)
        {
            Color32[] pixels = tex.GetPixels32();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 c = pixels[i];
                if (clearMagentaKey && c.r == 255 && c.g == 0 && c.b == 255 && c.a == 255)
                {
                    pixels[i] = new Color32(0, 0, 0, 0);
                    continue;
                }
                pixels[i] = new Color32((byte)(c.r * c.a / 255), (byte)(c.g * c.a / 255), (byte)(c.b * c.a / 255), c.a);
            }
            tex.SetPixels32(pixels);
            tex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
        }

        // Copies bones that the original rig has but the replacement doesn't (e.g. spine_root, rod_point), keeping their parent and setup pose, so the game's bone lookups resolve to real bones.
        private static void AddMissingBones(SkeletonDataAsset originalAsset, SkeletonDataAsset replacementAsset, string name)
        {
            if (originalAsset == null)
            {
                UPEHFBase.Log.LogWarning("Original skeleton for NPC '" + name + "' is missing; bones not copied.");
                return;
            }

            SkeletonData original = originalAsset.GetSkeletonData(true);
            SkeletonData replacement = replacementAsset.GetSkeletonData(true);
            List<string> added = new List<string>();
            foreach (BoneData source in original.Bones)
            {
                if (replacement.FindBone(source.Name) != null)
                    continue;

                BoneData parent = source.Parent == null ? null : replacement.FindBone(source.Parent.Name);
                if (source.Parent != null && parent == null)
                {
                    UPEHFBase.Log.LogWarning("Bone '" + source.Name + "' for NPC '" + name + "' has no matching parent '" + source.Parent.Name + "'; skipped.");
                    continue;
                }

                BoneData bone = new BoneData(replacement.Bones.Count, source.Name, parent)
                {
                    Length = source.Length,
                    X = source.X,
                    Y = source.Y,
                    Rotation = source.Rotation,
                    ScaleX = source.ScaleX,
                    ScaleY = source.ScaleY,
                    ShearX = source.ShearX,
                    ShearY = source.ShearY,
                    TransformMode = source.TransformMode
                };
                replacement.Bones.Add(bone);
                added.Add(source.Name);
            }

            if (added.Count > 0)
                UPEHFBase.Log.LogInfo("Copied " + added.Count + " missing bone(s) for NPC '" + name + "': " + string.Join(", ", added));
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadDLC)), HarmonyPostfix]
        private static void ReplaceSkeletons()
        {
            GameObject npcManagerObject = GameObject.Find("NPCManager");
            if (npcManagerObject == null)
            {
                UPEHFBase.Log.LogError("NPCManager not found; skeleton replacement skipped.");
                return;
            }
            GameObject[] npcPrefabs = npcManagerObject.GetComponent<NPCManager>().npcPrefab;
            NpcIdInfo.Verify(npcPrefabs);
            for (int i = 0; i < npcPrefabs.Length; i++)
            {
                GameObject npcPrefab = npcPrefabs[i];
                if (npcPrefab == null) continue;
                string name = npcPrefab.name.Replace("_prefab", "");
                string npcPrefabPath = Paths.ConfigPath + "/skeletonReplacers/" + name;
                if (!Directory.Exists(npcPrefabPath))
                    continue;

                try
                {
                    SkeletonAnimation prefabAnimation = npcPrefab.GetComponentInChildren<SkeletonAnimation>();

                    // Reuse the skeleton built on an earlier LoadDLC call instead of rebuilding it. Keyed by name, since the prefab array index can change between calls.
                    if (cache.TryGetValue(name, out SkeletonDataAsset cached))
                    {
                        prefabAnimation.skeletonDataAsset = cached;
                        prefabAnimation.Initialize(overwrite: true);
                        UPEHFBase.Log.LogWarning("Replaced Skeleton for NPC '" + name + "' with npcID [" + i + "] from cache.");
                        continue;
                    }

                    // Load files
                    byte[] textureData = File.ReadAllBytes(npcPrefabPath + "/" + name + ".png");
                    TextAsset skeletonDataFile = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".json"));
                    TextAsset atlasText = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".atlas"));
                    Material mat = UnityEngine.Object.Instantiate<Material>(prefabAnimation.GetComponent<MeshRenderer>().materials[0]);

                    // Load and Setup Atlas Page
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false, linear: false);
                    texture.LoadImage(textureData, markNonReadable: false);
                    texture = PadToMultipleOf4(texture);
                    ProcessPixels(texture, UPEHFHook.Config.Instance.SkeletonMagentaKeyEnabled.Value);
                    texture.wrapMode = TextureWrapMode.Clamp;
                    texture.filterMode = FilterMode.Bilinear;
                    texture.anisoLevel = 1;
                    texture.Apply(updateMipmaps: true, makeNoLongerReadable: false);
                    texture.Compress(highQuality: true);
                    texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
                    texture.name = name;

                    // Setup Atlas
                    SpineAtlasAsset atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, new Texture2D[] { texture }, mat, initialize: true);

                    // Load and setup skeletonData
                    SkeletonDataAsset skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonDataFile, atlasAsset, initialize: true, scale: 0.01f);
                    AddMissingBones(prefabAnimation.skeletonDataAsset, skeletonDataAsset, name);
                    prefabAnimation.skeletonDataAsset = skeletonDataAsset;
                    prefabAnimation.Initialize(overwrite: true);
                    cache[name] = skeletonDataAsset;

                    // Finished!
                    UPEHFBase.Log.LogWarning("Replaced Skeleton for NPC '" + name + "' with npcID [" + i + "]");
                }
                catch (Exception ex)
                {
                    UPEHFBase.Log.LogError("Failed to replace skeleton for NPC '" + name + "' with npcID [" + i + "]!\r\n" + ex);
                }
            }
        }
    }
}