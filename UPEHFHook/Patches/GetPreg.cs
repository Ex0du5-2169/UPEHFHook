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
        public static readonly int[] AllowedIDs = { 5, 6, 73, 110, 113, 114, 115, 116, 118, 162, 163 };

        [HarmonyPatch(typeof(SexManager), nameof(SexManager.IsPregable)), HarmonyPostfix]
        public static void IsPregablePatch(ref bool __result, int npcID)
        {
            if (Array.IndexOf(AllowedIDs, npcID) >= 0)
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(RandomCharacter), nameof(RandomCharacter.EquipableNPCCache)), HarmonyPostfix]
        public static void EquipableNPCCache(ref List<int> __result, string itemKey, ManagersScript ___mn)
        {
            if (itemKey != "acce_s_00" || __result == null)
            {
                return;
            }
            int[] PregableIDs = AllowedIDs;
            foreach (int npcID in PregableIDs)
                if (npcID < ___mn.npcMN.npcPrefab.Length && ___mn.npcMN.npcPrefab[npcID] != null && !__result.Contains(npcID))
                {
                    __result.Add(npcID);
                }
        }

    }
}
