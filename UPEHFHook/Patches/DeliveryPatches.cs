using HarmonyLib;
using HFramework.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YotanModCore;
using YotanModCore.Consts;
using System.Linq;



namespace UPEHFHook.Patches
{
    public class DeliveryPatches
    {
        public static int[] month = { 1, 11, 12 };

        public static void DoPatch(Harmony harmony)
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("HFramework"))
            {
                harmony.Patch(
                  typeof(SpawnChild).GetMethod("GetChildNpcId"),
                  postfix: new HarmonyMethod(typeof(DeliveryPatches).GetMethod("RecalculateChild"))
                );
            }
            else
            {
                harmony.Patch(
                  typeof(SexManager).GetMethod("Delivery"),
                  transpiler: new HarmonyMethod(typeof(DeliveryPatches).GetMethod("TranspileDelivery"))
                );
            }
        }
        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildNpcId")]
        [HarmonyPostfix]

        private static void RecalculateChild(ref int __result, int gender, SpawnChild __instance)
        {
            int childNpcId;
            int randomBirth;
            randomBirth = UnityEngine.Random.Range(0, 20);

            //Add a valentines birth chance too?
            if (SaveManager.dlc00 != null)
            {
                switch (__instance.Girl.npcID)
                {
                    case NpcID.Reika:
                    case NpcID.Cassie:
                    case NpcID.Giant: //Remember to switch her to birth young giant when she's actually implemented in the game/has conntent
                    case NpcID.Lulu:
                        if (gender == 0)
                        {
                            childNpcId = 140;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 141;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Nami:
                    case NpcID.Sally: //Maybe add chance for normal native births too?
                    case NpcID.Shino:
                    case NpcID.Kana: //Maybe normal natives for her too? Considering the boss R that can happen.
                    case NpcID.Mira:
                        if (gender == 0)
                        {
                            childNpcId = 142;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 143;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Merry: //December-only present birth chance?
                        if ((month.Contains(System.DateTime.Now.Month)) && (randomBirth >= 10))
                        {
                            if (gender == 0)
                            {
                                childNpcId = 172;
                                UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                                randomBirth = -1;
                                __result = childNpcId;
                            }
                            else
                            {
                                randomBirth = -1;
                                childNpcId = 170;
                                __result = childNpcId;
                            }
                            break;
                        }
                        else if (gender == 0)
                        {
                            childNpcId = 16;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 14;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Yona:    //Add a valentines birth chance too? Wait until Ton adds Valentines content.
                        switch (__instance.Girl.pregnant[0])
                        {
                            case NpcID.Spike:
                                if (gender == 0)
                                {
                                    childNpcId = 21;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 30;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Planton:
                                if (gender == 0)
                                {
                                    childNpcId = 26;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 27;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Bigfoot:
                                if (gender == 0)
                                {
                                    childNpcId = 140;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 141;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Werewolf:
                                if (gender == 0)
                                {
                                    childNpcId = 24;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 35;
                                    __result = childNpcId;
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (__instance.Girl.npcID)
                {
                    case NpcID.Reika:
                    case NpcID.Giant:
                    case NpcID.Cassie:
                        //case NpcID.Kana:
                        //case NpcID.Lulu:
                        if (gender == 0)
                        {
                            childNpcId = 90;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 91;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Nami:
                        if (gender == 0)
                        {
                            childNpcId = 73;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 91;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Shino:
                    case NpcID.Sally:
                        if (gender == 0)
                        {
                            childNpcId = 15;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 181;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Merry:
                        if ((month.Contains(System.DateTime.Now.Month)) && (randomBirth >= 10))
                        {
                            if (gender == 0)
                            {
                                childNpcId = 172;
                                UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                                randomBirth = -1;
                                __result = childNpcId;
                            }
                            else
                            {
                                childNpcId = 170;
                                __result = childNpcId;
                            }
                            break;
                        }
                        else if (gender == 0)
                        {
                            childNpcId = 15;
                            __result = childNpcId;
                        }
                        else
                        {
                            childNpcId = 10;
                            __result = childNpcId;
                        }
                        break;
                    case NpcID.Yona:
                        switch (__instance.Girl.pregnant[0])
                        {
                            case NpcID.Spike:
                                if (gender == 0)
                                {
                                    childNpcId = 21;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 30;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Planton:
                                if (gender == 0)
                                {
                                    childNpcId = 26;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 27;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Bigfoot:
                                if (gender == 0)
                                {
                                    childNpcId = 17;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 11;
                                    __result = childNpcId;
                                }
                                break;
                            case NpcID.Werewolf:
                                if (gender == 0)
                                {
                                    childNpcId = 24;
                                    __result = childNpcId;
                                }
                                else
                                {
                                    childNpcId = 35;
                                    __result = childNpcId;
                                }
                                break;
                        }
                        break;
                }
            }
        }
        public static int BirthChart(int initialNpcId, int gender, CommonStates mother)
        {
            int childNpcId = initialNpcId;
            int randomBirth = UnityEngine.Random.Range(0, 20);

            // your checks
            if (SaveManager.dlc00 != null)
            {
                switch (mother.npcID)
                {
                    case 5:
                    case 113:
                    case 110: //Remember to switch her to birth young giant when she's actually implemented in the game/has conntent
                    case 163:
                        if (gender == 0)
                        {
                            childNpcId = 140;
                        }
                        else
                        {
                            childNpcId = 141;
                        }
                        break;
                    case 6:
                    case 115: //Maybe add chance for normal native births too?
                    case 114:
                    case 162: //Maybe normal natives for her too? Considering the boss R that can happen.
                    case 118:
                        if (gender == 0)
                        {
                            childNpcId = 142;
                        }
                        else
                        {
                            childNpcId = 143;
                        }
                        break;
                    case 116: //December-only present birth chance?
                        if ((month.Contains(System.DateTime.Now.Month)) && (randomBirth >= 10))
                        {
                            if (gender == 0)
                            {
                                childNpcId = 172;
                                UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                                randomBirth = -1;
                            }
                            else
                            {
                                randomBirth = -1;
                                childNpcId = 170;
                            }
                            break;
                        }
                        else if (gender == 0)
                        {
                            childNpcId = 16;
                        }
                        else
                        {
                            childNpcId = 14;
                        }
                        break;
                    case 0:    //Add a valentines birth chance too? Wait until Ton adds Valentines content.
                        switch (mother.pregnant[0])
                        {
                            case 101:
                                if (gender == 0)
                                {
                                    childNpcId = 21;
                                }
                                else
                                {
                                    childNpcId = 30;
                                }
                                break;
                            case 103:
                                if (gender == 0)
                                {
                                    childNpcId = 26;
                                }
                                else
                                {
                                    childNpcId = 27;
                                }
                                break;
                            case 25:
                                if (gender == 0)
                                {
                                    childNpcId = 140;
                                }
                                else
                                {
                                    childNpcId = 141;
                                }
                                break;
                            case 35:
                                if (gender == 0)
                                {
                                    childNpcId = 24;
                                }
                                else
                                {
                                    childNpcId = 35;
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (mother.npcID)
                {
                    case 5:
                    case 110:
                    case 113:
                        //case NpcID.Kana:
                        //case NpcID.Lulu:
                        if (gender == 0)
                        {
                            childNpcId = 90;
                        }
                        else
                        {
                            childNpcId = 91;
                        }
                        break;
                    case 6:
                        if (gender == 0)
                        {
                            childNpcId = 73;
                        }
                        else
                        {
                            childNpcId = 91;
                        }
                        break;
                    case 114:
                    case 115:
                        if (gender == 0)
                        {
                            childNpcId = 15;
                        }
                        else
                        {
                            childNpcId = 181;
                        }
                        break;
                    case 116:
                        if ((month.Contains(System.DateTime.Now.Month)) && (randomBirth >= 10))
                        {
                            if (gender == 0)
                            {
                                childNpcId = 172;
                                UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                                randomBirth = -1;
                            }
                            else
                            {
                                childNpcId = 170;
                            }
                            break;
                        }
                        else if (gender == 0)
                        {
                            childNpcId = 15;
                        }
                        else
                        {
                            childNpcId = 10;
                        }
                        break;
                    case 0:
                        switch (mother.pregnant[0])
                        {
                            case 101:
                                if (gender == 0)
                                {
                                    childNpcId = 21;
                                }
                                else
                                {
                                    childNpcId = 30;
                                }
                                break;
                            case 103:
                                if (gender == 0)
                                {
                                    childNpcId = 26;
                                }
                                else
                                {
                                    childNpcId = 27;
                                }
                                break;
                            case 25:
                                if (gender == 0)
                                {
                                    childNpcId = 17;
                                }
                                else
                                {
                                    childNpcId = 11;
                                }
                                break;
                            case 35:
                                if (gender == 0)
                                {
                                    childNpcId = 24;
                                }
                                else
                                {
                                    childNpcId = 35;
                                }
                                break;
                        }
                        break;
                }
            }


            return childNpcId;
        }
        public static IEnumerable<CodeInstruction> TranspileDelivery(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo rng = AccessTools.Method(typeof(Random), "Range", new System.Type[] { typeof(int), typeof(int) });
            CodeMatcher cm = new CodeMatcher(instructions);
            object common = cm.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "common")
            ).Advance(1).Operand; // Get the common!
            cm.MatchForward(true,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Ldc_I4_2),
                new CodeMatch(OpCodes.Call, rng),
                new CodeMatch(OpCodes.Stfld)
            );
            object childGender = cm.Operand;
            cm.Start();
            cm.MatchForward(false,
                new CodeMatch(OpCodes.Stloc_S),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldc_I4_M1),
                new CodeMatch(OpCodes.Beq)
            );
            cm.Advance(1);
            object currentChild = cm.Operand;
            cm.Advance(1);
            cm.InsertAndAdvance(new CodeInstruction[] {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, childGender),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, common),
                CodeInstruction.Call((int a, int gender, CommonStates npc) => BirthChart(a, gender, npc)),
                new CodeInstruction(OpCodes.Stloc_S, currentChild),
                new CodeInstruction(OpCodes.Ldloc_S, currentChild),
            });
            return cm.Instructions();
        }
    }
}

