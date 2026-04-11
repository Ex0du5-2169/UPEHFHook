using HarmonyLib;
using HFramework;
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

        [HarmonyPatch(typeof(SexManager), nameof(SexManager.IsPregable)), HarmonyPostfix]
        public static void IsPregablePatch(ref bool __result, int npcID)
        {
            if (__result == false && npcID != 0 && UPEHFBase.PregableID.Contains(npcID))
                __result = true;
        }

        [HarmonyPatch(typeof(RandomCharacter), nameof(RandomCharacter.EquipableNPCCache)), HarmonyPostfix]
        public static void EquipableNPCCache(List<int> __result, string itemKey, int startID = -1)
        {
            if (itemKey == "acce_s_00")
                foreach (int npcID in UPEHFBase.PregableID)
                    if (!__result.Contains(npcID))
                        __result.Add(npcID);
        }

    }
}
