using HarmonyLib;
using Spine.Unity;
using UnityEngine;

namespace UPEHFHook.Patches
{
    static class AssetsLoader
    {
        private static bool loadedAssets = false;

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadDLC))]
        [HarmonyPostfix]
        private static void LoadAssets()
        {
            NPCManager npcm = GameObject.Find("NPCManager").GetComponent<NPCManager>();
            if (UPEHFBase.skelDataAssets == null)
                UPEHFBase.skelDataAssets = new SkeletonDataAsset[npcm.npcPrefab.Length];
            // Load and/or replace Assets
            for (int i = 0; i < npcm.npcPrefab.Length; i++)
            {
                GameObject prefab = npcm.npcPrefab[i];

                if (prefab == null)
                    continue;

                if (!loadedAssets)
                {
                    string assetName = prefab.name.Replace("_prefab", "_SkeletonData.asset");
                    UPEHFBase.skelDataAssets[i] = UPEHFBase.Assets.LoadAsset<SkeletonDataAsset>(assetName);
                }
                SkeletonDataAsset newDataAsset = UPEHFBase.skelDataAssets[i];
                if (newDataAsset == null)
                    continue;

                SkeletonAnimation anim = prefab.GetComponentInChildren<SkeletonAnimation>();

                anim.skeletonDataAsset = newDataAsset;
                anim.initialSkinName = "default";
                anim.Initialize(true);
                if (anim.skeleton.FindSlot("Body_preg") != null)
                {
                    anim.skeleton.SetAttachment("Body_preg", null);
                }
                SkeletonGraphic graph = anim.GetComponent<SkeletonGraphic>();
                graph.SetMaterialDirty();
                UPEHFBase.Log.LogInfo("Patched " + prefab.name + "(npcID [" + i + "])");
            }
            loadedAssets = true;
        }
    }
}