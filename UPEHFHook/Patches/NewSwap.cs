using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Spine.Unity;
using YotanModCore;
using UnityEngine;
using Spine;

namespace UPEHFHook.Patches
{
    internal class NewSwap
    {
        [HarmonyPatch(typeof(CommonStates))]
        [HarmonyPatch("LoveChange")]
        [HarmonyPostfix]

        public static void CSPostFix(CommonStates partner)
        {

            if (partner.anim.skeleton.FindSlot("body_preg") == null)
            {
                switch (partner.npcID)
                {
                    case 5:
                    case 6:
                    case 90:
                    case 110:
                    case 113:
                    case 114:
                    case 115:
                    case 116:
                        SkeletonSwapper.CleanAndTrackSkeletons();
                        break;
                }
            }
        }
    }
}
