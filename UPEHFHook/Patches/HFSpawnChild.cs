using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HFramework.Handlers;
using HarmonyLib;
using YotanModCore;
using YotanModCore.Consts;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;


namespace UPEHFHook.Patches
{
    public class HFSpawnChild
    {
        public static int[] month = { 1, 11, 12 };


        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildNpcId")]
        [HarmonyPrefix]

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
    }
}

