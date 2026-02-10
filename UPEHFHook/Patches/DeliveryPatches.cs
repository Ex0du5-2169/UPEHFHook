using HarmonyLib;
using HFramework.Handlers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using YotanModCore.Consts;
using System.Linq;



namespace UPEHFHook.Patches
{
    // Seperate class, so we don't reference the missing dll. unless necessary?
    public static class YotanDeliveryPatch
    {
        public static void DoPatch(Harmony harmony)
        {
            harmony.Patch(
                typeof(SpawnChild).GetMethod("GetChildNpcId"),
                postfix: new HarmonyMethod(typeof(YotanDeliveryPatch).GetMethod("RecalculateChild"))
            );
        }

        private static void RecalculateChild(ref int __result, int gender, SpawnChild __instance)
        {
            __result = DeliveryPatches.BirthChart(__result, gender, __instance.Girl);
            return;
        }
    }

    public static class DeliveryPatches
    {
        public static int[] month = { 1, 11, 12 };

        public static void DoPatch(Harmony harmony)
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("HFramework"))
                YotanDeliveryPatch.DoPatch(harmony);
            else
                harmony.PatchAll(typeof(DeliveryPatches));
        }

        public static int BirthChart(int initialNpcId, int gender, CommonStates mother)
        {
            int childNpcId = initialNpcId;
            int randomBirth = UnityEngine.Random.Range(0, 20);
            bool hasDLC = SaveManager.dlc00 != null;
            // your checks
            if (hasDLC)
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
            if (childNpcId != -1) return childNpcId;
            if (gender == 0) return hasDLC ? 16 : 15;
            return hasDLC ? 14 : 10;
        }

        [HarmonyPatch(typeof(SexManager), nameof(SexManager.Delivery), MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TranspileDelivery(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo rng = AccessTools.Method(typeof(UnityEngine.Random), "Range", new System.Type[] { typeof(int), typeof(int) });
            CodeMatcher cm = new CodeMatcher(instructions);
            object common = cm.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "common")
            ).Advance(1).Operand; // Get the common!
            //UPEHFBase.Log.LogInfo("Found common");
            cm.MatchForward(true,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Ldc_I4_2),
                new CodeMatch(OpCodes.Call, rng),
                new CodeMatch(OpCodes.Stfld)
            );
            object childGender = cm.Operand;
            //UPEHFBase.Log.LogInfo("Found childGender");
            cm.Start();
            cm.MatchForward(false,
                new CodeMatch(OpCodes.Stloc_S),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldc_I4_M1),
                new CodeMatch(OpCodes.Beq)
            );
            cm.Advance(1);
            object currentChild = cm.Operand;
            //UPEHFBase.Log.LogInfo("Found currentChild");
            cm.Advance(1);
            cm.InsertAndAdvance(new CodeInstruction[] {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, childGender),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, common),

                CodeInstruction.Call(typeof(DeliveryPatches), nameof(DeliveryPatches.BirthChart)),
                new CodeInstruction(OpCodes.Stloc_S, currentChild),
                new CodeInstruction(OpCodes.Ldloc_S, currentChild),
            });
            return cm.Instructions();
        }
    }
}

