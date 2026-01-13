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


namespace UPEHFHook.Patches
{
    public class HFSpawnChild
    {
        public static CommonStates Girl;
        public static int getgender;
        public void newChild(CommonStates girl)
        {
            Girl = girl;
        }

        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildGender")]
        [HarmonyPostfix]

        public static void GetGender(int gender)
        {
            getgender = gender;
        }

        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildNpcId")]
        [HarmonyPrefix]

        private static void RecalculateChild()
        {
            int childNpcId;
            int randomBirth;
            randomBirth = UnityEngine.Random.Range(0, 20);

            //Add a valentines birth chance too? Which NPC?

            switch (Girl.npcID)
            {
                case NpcID.Reika:
                case NpcID.Cassie:
                    //case NpcID.Lulu:
                    childNpcId = getgender == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl;
                    break;
                case NpcID.Giant:
                    childNpcId = getgender == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl; //change to Giant2 when she's implemented
                    break;
                case NpcID.Nami:
                case NpcID.Sally: //Maybe add chance for normal native births too?
                case NpcID.Shino:
                    //case NpcID.Mira:
                    childNpcId = getgender == Gender.Male ? NpcID.UnderGroundBoy : NpcID.UnderGroundGirl;
                    break;
                case NpcID.Merry: //December-only present birth chance?
                    if ((System.DateTime.Today.Month == 12) && (randomBirth >= 10))
                    {
                        childNpcId = getgender == Gender.Male ? NpcID.PresentBlue : NpcID.PresentRed;
                        UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                        randomBirth = -1;
                        break;
                    }
                    else
                    {
                        childNpcId = getgender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                        break;
                    }

                //case NpcID.Kana:
                case NpcID.ElderSisterNative:
                    childNpcId = getgender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
                
                case NpcID.Yona:
                    switch (CommonUtils.GetPregnantFatherId(Girl))
                    {
                        case NpcID.Spike:
                            childNpcId = getgender == Gender.Male ? NpcID.Spider : NpcID.Spider2;
                            break;
                        case NpcID.Planton:
                            childNpcId = getgender == Gender.Male ? NpcID.Mandrake : NpcID.Nepenthes;
                            break;
                        case NpcID.Bigfoot:
                            childNpcId = getgender == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl;
                            break;
                        default:
                            UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + Girl.npcID);
                            childNpcId = getgender == Gender.Male ? NpcID.Son : NpcID.YoungLady;
                            break;
                    }
                    break;
                
                default:
                    UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + Girl.npcID);
                    childNpcId = getgender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
            }
            
            return;
        }

    }
}
