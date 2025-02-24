using HarmonyLib;
using HFramework.Scenes.Conditionals;
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
        [HarmonyPatch(typeof(CommonStates))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]

        public static void skelSwapper(CommonStates __instance)
        {
            switch (__instance.npcID)
            {
                case 5:
                    __instance.anim.skeletonDataAsset = UPEHFBase.reikaSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.reikaSkel);
                    break;
                case 6:
                    __instance.anim.skeletonDataAsset = UPEHFBase.namiSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.namiSkel);
                    break;
                case 17:
                    __instance.anim.skeletonDataAsset = UPEHFBase.lfemSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.lfemSkel);
                    break;
                case 90:
                    __instance.anim.skeletonDataAsset = UPEHFBase.genbba2Skel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.genbba2Skel);
                    break;
                case 110:
                    __instance.anim.skeletonDataAsset = UPEHFBase.giantSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.giantSkel);
                    break;
                case 113:
                    __instance.anim.skeletonDataAsset = UPEHFBase.cassieSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.cassieSkel);
                    break;
                case 114:
                    __instance.anim.skeletonDataAsset = UPEHFBase.shinoSkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.shinoSkel);
                    break;
                case 115:
                    __instance.anim.skeletonDataAsset = UPEHFBase.sallySkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.sallySkel);
                    break;
                case 116:
                    __instance.anim.skeletonDataAsset = UPEHFBase.merrySkel;
                    __instance.anim.Initialize(true);
                    __instance.anim.AnimationState.SetAnimation(0, "A_idle", true);
                    UPEHFBase.Log.LogInfo(__instance.npcID + " skeleton swapped" + UPEHFBase.merrySkel);
                    break;
            }
        }
    }
}