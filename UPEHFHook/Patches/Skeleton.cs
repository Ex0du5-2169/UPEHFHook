using HarmonyLib;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



namespace UPEHFHook.Patches
{
    class skeleton
    {
        //[HarmonyPatch(typeof(CommonStates))]
        //[HarmonyPatch("Start")]
        //[HarmonyPrefix]
        public static void skelSwapper(CommonStates __instance)
        {
            if (__instance == null)
            {
                UPEHFBase.Log.LogError("SexManager Instance not found");
            }

            __instance = CommonStates.FindObjectOfType<CommonStates>();
            UPEHFBase.Log.LogInfo(__instance + ": Current instance");
            SkeletonDataAsset currentSkel;
                SkeletonDataAsset newSkel;
                switch (__instance.npcID)
                {
                    case 5:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.reikaSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.reikaSkel);
                        break;
                    case 6:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.namiSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.namiSkel);
                        break;
                    case 17:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.lfemSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.lfemSkel);
                        break;
                    case 90:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.genbba2Skel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.genbba2Skel);
                        break;
                    case 110:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.giantSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.giantSkel);
                        break;
                    case 113:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.cassieSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.cassieSkel);
                        break;
                    case 114:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.shinoSkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.shinoSkel);
                        break;
                    case 115:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.sallySkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.sallySkel);
                        break;
                    case 116:
                        currentSkel = __instance.GetComponentInChildren<SkeletonDataAsset>();
                        newSkel = UPEHFBase.merrySkel;
                        __instance.anim.skeletonDataAsset = newSkel;
                        __instance.anim.Initialize(true);
                        __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                        UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.merrySkel);
                        break;
                }
            
        }
    }
}