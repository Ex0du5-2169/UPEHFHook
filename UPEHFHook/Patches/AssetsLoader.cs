using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx.Logging;
using UnityEngine;
using System.Reflection;
using System.Resources;
using Spine.Unity;
using System.IO;
using System;

namespace UPEHFHook.Patches
{
    static class AssetsLoader
    {
        [HarmonyPatch(typeof(NPCManager), "Start"), HarmonyPostfix]
        private static void PatchSkeletons(NPCManager __instance)
        {

            for (int i = 0; i < __instance.npcPrefab.Length; i++)
            {
                GameObject npcPrefab = __instance.npcPrefab[i];
                if (npcPrefab == null) continue;
                string name = npcPrefab.name.Replace("_prefab", "");
                string npcPrefabPath = Paths.ConfigPath + "/skeletonReplacers/" + name;
                if (!Directory.Exists(npcPrefabPath))
                {
                    UPEHFBase.Log.LogInfo(npcPrefabPath + " skipped");
                    continue;
                }

                try
                {
                    SkeletonAnimation prefabAnimation = npcPrefab.GetComponentInChildren<SkeletonAnimation>();

                    byte[] textureData = File.ReadAllBytes(npcPrefabPath + "/" + name + ".png");
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
                    texture.LoadImage(textureData, false);
                    texture.LoadImage(textureData, false);
                    texture = WorkshopNPCManager.PadToMultipleOf4_CPU(texture, true);
                    texture.wrapMode = TextureWrapMode.Clamp;
                    texture.filterMode = FilterMode.Bilinear;
                    texture.anisoLevel = 1;
                    texture.Apply(true, false);
                    texture.Compress(true);
                    texture.Apply(false, true);
                    texture.name = name;

                    TextAsset skeletonDataFile = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".json"));
                    TextAsset atlasText = new TextAsset(File.ReadAllText(npcPrefabPath + "/" + name + ".atlas"));
                    Material mat = UnityEngine.Object.Instantiate<Material>(prefabAnimation.GetComponent<MeshRenderer>().materials[0]);
                    SpineAtlasAsset atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, new Texture2D[] { texture }, mat, true);
                    SkeletonDataAsset skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonDataFile, atlasAsset, true, 0.01f);
                    prefabAnimation.skeletonDataAsset = skeletonDataAsset;
                    prefabAnimation.Initialize(true);
                    prefabAnimation.skeleton.SetAttachment("Body_preg", null);
                    if (prefabAnimation.skeleton.FindSlot("Body_Cloth") != null)
                        prefabAnimation.skeleton.FindSlot("Body_Cloth").SetColor(new Color(1f, 1f, 1f, 1f));
                    prefabAnimation.state.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogWarning("Replaced " + i + " skeleton.");
                }
                catch (Exception ex)
                {
                    UPEHFBase.Log.LogError("Failed to replace npc with id " + i);
                    UPEHFBase.Log.LogWarning(ex.Message);
                }

            }
        }
    }
}