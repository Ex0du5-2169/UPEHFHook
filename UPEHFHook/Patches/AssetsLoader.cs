using BepInEx;
using HarmonyLib;
using HFramework;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UPEHFHook.Patches
{
    static class AssetsLoader
    {
        static Dictionary<int, SkeletonDataAsset> cache = new Dictionary<int, SkeletonDataAsset>();

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadDLC)), HarmonyPostfix]
        private static void ReplaceSkeletons()
        {
            NPCManager npcManager = GameObject.Find("NPCManager").GetComponent<NPCManager>();
            GameObject[] npcPrefabs = npcManager.npcPrefab;
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
                    if (cache.ContainsKey(i))
                    {
                        prefabAnimation.skeletonDataAsset = cache[i];
                        UPEHFBase.Log.LogWarning("Replaced Skeleton for NPC '" + name + "' with npcID [" + i + "] from cache.");
                        continue;
                    }
                    // Load files
                    byte[] textureData = File.ReadAllBytes(npcPrefabPath + "/" + name + ".png");
                    TextAsset skeletonDataFile = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".json"));
                    TextAsset atlasText = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".atlas"));
                    Material mat = UnityEngine.Object.Instantiate<Material>(prefabAnimation.GetComponent<MeshRenderer>().materials[0]);

                    // Load and Setup Atlas Page
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
                    texture.LoadImage(textureData, false);
                    texture = WorkshopNPCManager.PadToMultipleOf4_CPU(texture, true);
                    texture.wrapMode = TextureWrapMode.Clamp;
                    texture.filterMode = FilterMode.Bilinear;
                    texture.anisoLevel = 1;
                    texture.Apply(true, false);
                    texture.Compress(true);
                    texture.Apply(false, true);
                    texture.name = name;

                    // Setup Atlas
                    SpineAtlasAsset atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, new Texture2D[] { texture }, mat, true);

                    // Load and setup skeletonData
                    SkeletonDataAsset skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonDataFile, atlasAsset, true, 0.01f);
                    cache[i] = skeletonDataAsset;
                    prefabAnimation.skeletonDataAsset = skeletonDataAsset;
                    prefabAnimation.Initialize(true);

                    // Finished!
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