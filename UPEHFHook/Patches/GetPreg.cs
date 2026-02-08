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
        [HarmonyPostfix]

        public static void IsPregablePatch(ref bool __result, int npcID)
        {
            if (UPEHFBase.PregableID.Contains(npcID))
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(RandomCharacter), nameof(RandomCharacter.EquipableNPCCache)), HarmonyPostfix]
        public static void EquipableNPCCache(List<int> __result, string itemKey, int startID = -1)
        {
            if (itemKey == "acce_s_00")
            {
                foreach (int npcID in UPEHFBase.PregableID)
                {
                    if (!__result.Contains(npcID))
                        __result.Add(npcID);
                }
            }
        }

    }
}
