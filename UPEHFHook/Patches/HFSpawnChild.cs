using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HFramework.Handlers;
using HarmonyLib;
using YotanModCore;
using YotanModCore.Consts;


namespace UPEHFHook.Patches
{
    public class HFSpawnChild
    {
        public static CommonStates Girl;
        public static int getGender {  get; set; }
        public void newChild(CommonStates girl)
        {
            Girl = girl;
        }

        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildGennder")]
        [HarmonyPrefix]

        private static void GrabGender(ref int __gender)
        {
            getGender = __gender;
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
                //case NpcID.Kana:
                //case NpcID.Lulu:
                    childNpcId = getGender == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl;
                    break;
                case NpcID.Giant:
                    childNpcId = getGender == Gender.Male ? NpcID.YoungMan : NpcID.Giant2;
                    break;
                case NpcID.Nami:
                case NpcID.Sally: //Maybe add chance for normal native births too?
                case NpcID.Shino:
                //case NpcID.Mira:
                    childNpcId = getGender == Gender.Male ? NpcID.UnderGroundBoy : NpcID.UnderGroundGirl;
                    break;
                case NpcID.Merry: //December-only present birth chance?
                    if ((System.DateTime.Today.Month == 12) && (randomBirth >= 10))
                    {
                        childNpcId = getGender == Gender.Male ? NpcID.PresentBlue : NpcID.PresentRed;
                        UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a pressent!");
                        randomBirth = -1;
                        break;
                    }
                    else
                    {
                        childNpcId = getGender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                        break;
                    }


                case NpcID.ElderSisterNative:
                    childNpcId = getGender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
                /*
                case NpcID.Yona:
                    switch (CommonUtils.GetPregnantFatherId(this.Girl))
                    {
                        case NpcID.Spike:
                            childNpcId = __gender == Gender.Male ? NpcID.Spider : NpcID.Spider2;
                            break;
                        case NpcID.Planton:
                            childNpcId = __gender == Gender.Male ? NpcID.Mandrake : NpcID.Nepenthes;
                            break;
                        default:
                            UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + this.Girl.npcID);
                            childNpcId = __gender == Gender.Male ? NpcID.YoungMan : NpcID.YoungLady;
                            break;
                    }
                    break;
                */
                default:
                    UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + Girl.npcID);
                    childNpcId = getGender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
            }
        }

    }
}
