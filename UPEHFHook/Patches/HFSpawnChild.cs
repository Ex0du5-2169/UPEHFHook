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
        public void newChild(CommonStates girl)
        {
            Girl = girl;
        }

        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildNpcId")]
        [HarmonyPrefix]

        private static void RecalculateChild(int __0)
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
                    childNpcId = __0 == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl;
                    break;
                case NpcID.Giant:
                    childNpcId = __0 == Gender.Male ? NpcID.YoungMan : NpcID.Giant2;
                    break;
                case NpcID.Nami:
                case NpcID.Sally: //Maybe add chance for normal native births too?
                case NpcID.Shino:
                //case NpcID.Mira:
                    childNpcId = __0 == Gender.Male ? NpcID.UnderGroundBoy : NpcID.UnderGroundGirl;
                    break;
                case NpcID.Merry: //December-only present birth chance?
                    if ((System.DateTime.Today.Month == 12) && (randomBirth >= 10))
                    {
                        childNpcId = __0 == Gender.Male ? NpcID.PresentBlue : NpcID.PresentRed;
                        UPEHFBase.Log.LogInfo("Congratulations, Merry birthed a present!");
                        randomBirth = -1;
                        break;
                    }
                    else
                    {
                        childNpcId = __0 == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                        break;
                    }


                case NpcID.ElderSisterNative:
                    childNpcId = __0 == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
                /*
                case NpcID.Yona:
                    switch (CommonUtils.GetPregnantFatherId(this.Girl))
                    {
                        case NpcID.Spike:
                            childNpcId = __0 == Gender.Male ? NpcID.Spider : NpcID.Spider2;
                            break;
                        case NpcID.Planton:
                            childNpcId = __0 == Gender.Male ? NpcID.Mandrake : NpcID.Nepenthes;
                            break;
                        default:
                            UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + this.Girl.npcID);
                            childNpcId = __0 == Gender.Male ? NpcID.YoungMan : NpcID.YoungLady;
                            break;
                    }
                    break;
                */
                default:
                    UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + Girl.npcID);
                    childNpcId = __0 == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
            }
        }

    }
}
