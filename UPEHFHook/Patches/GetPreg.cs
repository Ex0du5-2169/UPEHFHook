using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UPEHFHook.Patches
{
    public class GetPreg
    {

        [HarmonyPatch(typeof(NPCManager))]
        [HarmonyPatch("IsPerfumeNPC")]
        [HarmonyPostfix]

        public static void PerfumeFix(NPCManager __instance, ref bool __result, CommonStates common)
        {
            if (UPEHFHook.Config.Instance.AllowAllPerfume.Value)
            {
                //Should be fairly obvious, allows use of the perfume item on any NPC added to this list. As it stands, I've set it to everyone (default off to prevent SSNPC conflicts).
                if (!__result)
                {
                    __result = true;

                }
            }
        }

        [HarmonyPatch(typeof(SexManager))]
        [HarmonyPatch("IsPregable")]
        [HarmonyPrefix]

        public static void IsPregablePatch(CommonStates __instance, ref bool __result)
        {
            switch (__instance.npcID)
            {
                case 5:
                case 6:
                case 110:
                case 113:
                case 114:
                case 115:
                case 116:
                    //case 118:
                    //case 162:
                    //case 163:
                    __result = true;
                    break;
            }
        }

    }
}
