using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Spine.Unity;
using YotanModCore;
using UnityEngine;

namespace UPEHFHook.Patches
{
    internal class NewSwap
    {
        [HarmonyPatch(typeof(CommonStates))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]

        public static void CSPostFix(CommonStates __instance)
        {
            SkeletonGraphic skelgraphic = new SkeletonGraphic();
            //GameObject newobj = __instance.gameObject;
            switch (__instance.npcID)
            {
                case 5:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Reika;
                    skelgraphic.initialSkinName = "default"; //nullref, needs name assigning within Spine?
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 6:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Nami;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 90:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.ESis;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 110:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Giant;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 113:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Cassie;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 114:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Shino;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 115:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Sally;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
                case 116:
                    skelgraphic = __instance.GetComponent<SkeletonGraphic>();
                    skelgraphic.skeletonDataAsset = UPEHFBase.Merry;
                    skelgraphic.initialSkinName = "default";
                    skelgraphic.Initialize(true);
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                    skelgraphic.SetMaterialDirty();
                    break;
            }
        }

    }
}
