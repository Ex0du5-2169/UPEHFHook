﻿using System;
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
        private CommonStates Girl;
        public void newChild(CommonStates girl)
        {
            this.Girl = girl;
        }

        [HarmonyPatch(typeof(SpawnChild))]
        [HarmonyPatch("GetChildNpcId")]
        [HarmonyPrefix]

        private void RecalculateChild(ref int __gender)
        {
            int childNpcId;
            int randomBirth;
            randomBirth = UnityEngine.Random.Range(0, 20);

            switch (this.Girl.npcID)
            {
                case NpcID.Reika:
                case NpcID.Cassie:
                    childNpcId = __gender == Gender.Male ? NpcID.LargeNativeBoy : NpcID.LargeNativeGirl;
                    break;
                case NpcID.Giant:
                    childNpcId = __gender == Gender.Male ? NpcID.YoungMan : NpcID.Giant2;
                    break;
                case NpcID.Nami:
                case NpcID.Sally:
                case NpcID.Shino:
                    childNpcId = __gender == Gender.Male ? NpcID.UnderGroundBoy : NpcID.UnderGroundGirl;
                    break;
                case NpcID.Merry: //maybe add a December-only present birth chance?
                    if ((System.DateTime.Today.Month == 12) && (randomBirth >= 10))
                    {
                        childNpcId = __gender == Gender.Male ? NpcID.PresentBlue : NpcID.PresentRed;
                        break;
                    }
                    else
                    {
                        childNpcId = __gender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                        break;
                    }


                case NpcID.ElderSisterNative:
                    childNpcId = __gender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;

                case NpcID.Yona:
                    //childNpcId = __gender == Gender.Male ? NpcID.YoungMan : NpcID.YoungLady;
                    switch (CommonUtils.GetPregnantFatherId(this.Girl))
                        {
                            case NpcID.Spike:
                                childNpcId = __gender == Gender.Male ? NpcID.Spider : NpcID.Spider2;
                                break;
                            case NpcID.Planton:
                                childNpcId = __gender == Gender.Male ? NpcID.Mandrake : NpcID.Nepenthes;
                                break;
                            }
                            break;

                default:
                    UPEHFBase.Log.LogWarning("GetChildNpcId: Unexpected npcID: " + this.Girl.npcID);
                    childNpcId = __gender == Gender.Male ? NpcID.NativeBoy : NpcID.NativeGirl;
                    break;
            }
        }

    }
}
