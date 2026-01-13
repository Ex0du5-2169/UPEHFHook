using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Spine.Unity;
using YotanModCore;

namespace UPEHFHook.Patches
{
    internal class NewSwap
    {
        [HarmonyPatch(typeof(CommonStates))]
        [HarmonyPatch("LoveChange")]
        [HarmonyPostfix]

        public static void CSPostFix(CommonStates __instance)
        {
            SkeletonAnimation newskel = new SkeletonAnimation();
            switch (__instance.npcID)
            {
                case 5:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Reika;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 6:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Nami;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 90:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.ESis;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 110:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Giant;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 113:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Cassie;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 114:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Shino;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 115:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Sally;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
                case 116:
                    newskel = __instance.anim;
                    newskel.skeletonDataAsset = UPEHFBase.Merry;
                    __instance.anim = newskel;
                    UPEHFBase.Log.LogInfo("Swapping data asset for " + __instance.npcID);
                    __instance.anim.Initialize(true);
                    break;
            }
        }

    }
}
