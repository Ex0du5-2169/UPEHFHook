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
        private static bool pregresult = new bool();
        public static int currentStage = new int();

        [HarmonyPatch(typeof(SexManager))]
        [HarmonyPatch("PlayerRaped")]
        [HarmonyPostfix]

        public static void PRaped(CommonStates to, CommonStates from, SexManager __instance, ref ManagersScript ___mn)
        {
            switch (to.npcID)
            {
                case 0:
                    __instance.PregnancyCheck(to, from);
                    ___mn.uiMN.FriendHealthCheck(to);
                    break;
                case 1:
                    break;
            }
            //This section attaempts to trigger a pregnancy check upon the player being raped.
        }
        [HarmonyPatch(typeof(NPCManager))]
        [HarmonyPatch("IsPerfumeNPC")]
        [HarmonyPostfix]

        public static void PerfumeFix(NPCManager __instance, ref bool __result, CommonStates common)
        {
            if (UPEHFHook.Config.Instance.AllowAllPerfume.Value)
            {
                //Should be fairly obvious, allows use of the perfume item on any NPC added to this list. As it stands, I've set it to everyone.
                if (!__result)
                {
                    __result = true;

                }
            }
        }/*
        [HarmonyPatch(typeof(NPCMove))]
        [HarmonyPatch("Live")]
        [HarmonyPostfix]
        
        public static void RapeRateIncrease(CommonStates common, CommonStates partner, ref ManagersScript ___mn)
        {
            if (UPEHFHook.Config.Instance.ChangeRapeRate.Value)
            {
                if ((common.debuff.discontent <= 2) && (common.libido >= 50))
                {
                    if (___mn.sexMN.RapesCheck(common, partner))
                    {
                        ___mn.sexMN.StartCoroutine(___mn.sexMN.CommonRapesNPC(common, partner, 0)); //Part of a potential R rate increase test I was doing.

                    }
                }
            }
        }
        public static Cumflate(CommonStates girl)
        {
            switch (girl.npcID)
            {
                case 0:
                case 15:
                case 16:
                case 17:
                case 44:
                case 90:
                case 110:
                case 113:
                case 114:
                case 115:
                case 116:
                    return;
            }
        }*/

        public static bool PregCheckCall(CommonStates girl, CommonStates man)
        {


            /*bool creamed = false;
            creamed = true; //Must have taken an action that gives the creampie state, for now we have given it that state through other means.
            UPEHFBase.Log.LogInfo(creamed + ": Creampied");*/

            int isPreg = UnityEngine.Random.Range(0, 15); //Set a random range for preg chance
            UPEHFBase.Log.LogInfo(isPreg + ": Random int, must be > 11 for pregnancy");
            int pregStage = new int(); //eventually will become part of a mentstrual system, for now it's only used to hold an int for the game's preg system to receive.
            pregStage = 0;


            if (/*(creamed == true) && */(isPreg >= 12) && (girl.npcID != 0) && (girl.npcID != 44) && (girl.npcID != 17)) //Tests whether creampied(placeholder, does nothing yet) and if the RNG allows it, for now..
            {
                pregStage = 12;
                UPEHFBase.Log.LogInfo(pregStage + ": Staging, ignore, not needed yet");
                girl.pregnant[1] = pregStage; //Trigger the game's pregnancy system. Yes, the result is based on approx 3/15 or 1/5 RNG, I will eventually make this more complex.
                girl.pregnant[0] = man.friendID; //Pregnancy system requires the father be set.


                if (girl.pregnant[1] == 12)
                {

                    UPEHFBase.Log.LogInfo(girl.pregnant[1] + ": Default pregnancy state");
                    UPEHFBase.Log.LogInfo(girl.pregnant[0] + ": Return ID of sperm donor");
                    pregStage = 0; //Reset used variables, just in case.
                    //creamed = false;
                    isPreg = 0;
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        [HarmonyPatch(typeof(SexManager))]
        [HarmonyPatch("PregnancyCheck")]
        [HarmonyPostfix]
        public static void PcheckCallback(ref bool __result, ManagersScript ___mn, CommonStates girl, CommonStates man)
        {
            if ((!__result) && (girl.pregnant[1] == 0))
            {
                UPEHFBase.Log.LogInfo(girl.pregnant[1] + ": Not pregnant, passing to pregnancy checker");
                pregresult = PregCheckCall(girl, man); //Calls my pregnancy check if the game declares the result false and the girl is not currently pregnant. Essentially I say to the code "nuh uh, run this mess instead".

                __result = pregresult;
                ___mn.uiMN.FriendHealthCheck(girl);
                if (pregresult)
                {
                    UPEHFBase.Log.LogInfo(pregresult + ": Pregnancy check result");
                    ___mn.sound.GoSound(108, girl.transform.position, randomPitch: true);
                    SkeletonSwapper.CleanAndTrackSkeletons();
                    UPEHFBase.Log.LogInfo("Swapper Go!");
                }
                else
                {
                    UPEHFBase.Log.LogInfo(pregresult + ": Pregnancy check result");
                    return;
                }
            }
            else if (!__result)
            {
                UPEHFBase.Log.LogInfo(__result + ": Pregnancy check result");
                SkeletonSwapper.RevertSkeleton();
                UPEHFBase.Log.LogInfo("Swapping back!");
                return;
            }
            if (girl.pregnant[1] == 3)
            {
                SkeletonSwapper.CleanAndTrackSkeletons();
                UPEHFBase.Log.LogInfo("Swapper Go!");
                return;
            }
        }

    }
}
