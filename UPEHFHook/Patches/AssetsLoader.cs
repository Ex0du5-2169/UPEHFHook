using BepInEx;
using HarmonyLib;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UPEHFHook.Patches
{
    internal static class AssetsLoader
    {
        private static Texture2D PadToMultipleOf4(Texture2D src)
        {
            int w = (src.width + 3) & -4;
            int h = (src.height + 3) & -4;
            if (w == src.width && h == src.height)
            {
                return src;
            }
            RenderTexture rt = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(src, rt);
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D readable = new Texture2D(src.width, src.height, TextureFormat.RGBA32, mipChain: false);
            readable.ReadPixels(new Rect(0f, 0f, src.width, src.height), 0, 0);
            readable.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            Color32[] srcPixels = readable.GetPixels32();
            Color32[] dstPixels = new Color32[w * h];
            for (int y = 0; y < h; y++)
            {
                int sy = Math.Min(y, src.height - 1);
                for (int x = 0; x < w; x++)
                {
                    int sx = Math.Min(x, src.width - 1);
                    dstPixels[y * w + x] = srcPixels[sy * src.width + sx];
                }
            }
            Texture2D texture2D = new Texture2D(w, h, TextureFormat.RGBA32, mipChain: false);
            texture2D.SetPixels32(dstPixels);
            texture2D.Apply();
            UnityEngine.Object.Destroy(readable);
            UnityEngine.Object.Destroy(src);
            return texture2D;
        }

        [HarmonyPatch(typeof(SaveManager), "LoadDLC")]
        [HarmonyPostfix]
        private static void ReplaceSkeletons()
        {
            GameObject[] npcPrefabs = GameObject.Find("NPCManager").GetComponent<NPCManager>().npcPrefab;
            for (int i = 0; i < npcPrefabs.Length; i++)
            {
                GameObject npcPrefab = npcPrefabs[i];
                if (npcPrefab == null)
                {
                    continue;
                }
                string name = npcPrefab.name.Replace("_prefab", "");
                string npcPrefabPath = Paths.ConfigPath + "/skeletonReplacers/" + name;
                if (Directory.Exists(npcPrefabPath))
                {
                    try
                    {
                        SkeletonAnimation prefabAnimation = npcPrefab.GetComponentInChildren<SkeletonAnimation>();
                        byte[] textureData = File.ReadAllBytes(npcPrefabPath + "/" + name + ".png");
                        TextAsset skeletonDataFile = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".json"));
                        TextAsset atlasText = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".atlas"));
                        Material mat = UnityEngine.Object.Instantiate(prefabAnimation.GetComponent<MeshRenderer>().materials[0]);
                        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false, linear: false);
                        texture.LoadImage(textureData, markNonReadable: false);
                        texture = PadToMultipleOf4(texture);
                        texture.wrapMode = TextureWrapMode.Clamp;
                        texture.filterMode = FilterMode.Bilinear;
                        texture.anisoLevel = 1;
                        texture.Apply(updateMipmaps: true, makeNoLongerReadable: false);
                        texture.Compress(highQuality: true);
                        texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
                        texture.name = name;
                        SpineAtlasAsset atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, new Texture2D[1] { texture }, mat, initialize: true);
                        SkeletonDataAsset skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonDataFile, atlasAsset, initialize: true);
                        prefabAnimation.skeletonDataAsset = skeletonDataAsset;
                        prefabAnimation.Initialize(overwrite: true);
                        UPEHFBase.Log.LogWarning("Replaced Skeleton for NPC '" + name + "' with npcID [" + i + "]");
                    }
                    catch (Exception ex)
                    {
                        UPEHFBase.Log.LogError("Failed to replace skeleton for NPC '" + name + "' with npcID [" + i + "]!\r\n" + ex.Message);
                    }
                }
            }
        }
    }
}