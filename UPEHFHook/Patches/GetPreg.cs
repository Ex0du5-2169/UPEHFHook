using HarmonyLib;
using HFramework.Scenes.Conditionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UPEHFHook.Patches
{
    class GetPreg
    {
        private static bool pregresult = new bool();

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
            //Should be fairly obvious, allows use of the perfume item on any NPC added to this list.
            if (!__result)
            {
               __result = true;
                      
            }
        }

        public static bool PregCheckCall(CommonStates girl, CommonStates man)
        {

            bool canGet = false;
            bool canGet2 = false;
            CommonStates getsIt = girl;
            CommonStates givesIt = man;

            switch (getsIt.npcID)
            {
                case 0:
                case 5:
                case 6:
                case 17:
                case 19:
                case 44:
                case 90:
                case 110:
                case 113:
                case 114:
                case 115:
                case 116:
                    canGet = true;
                    break;
                case 1:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 18:
                case 25:
                case 35:
                case 89:
                case 91:
                    canGet2 = false;
                    return false;

            }
            switch (givesIt.npcID)
            {
                case 0:
                case 5:
                case 6:
                case 17:
                case 19:
                case 44:
                case 90:
                case 110:
                case 113:
                case 114:
                case 115:
                case 116:
                    canGet = true;
                    break;
                case 1:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 18:
                case 25:
                case 35:
                case 89:
                case 91:
                    canGet2 = false;
                    return false;

            }
            bool creamed = false;
            creamed = true; //Must have taken an action that gives the creampie state, for now we have given it that state through other means.
            Debug.Log(creamed + ": Creampied");

            int isPreg = UnityEngine.Random.Range(0, 15); //Set a random range for preg chance
            Debug.Log(isPreg + ": Random int, must be > 11 for pregnancy");
            int pregStage = new int(); //eventually will become part of a mentstrual system, for now it's only used to hold an int for the game's preg system to receive.
            pregStage = 0;

            if ((creamed == true) && (isPreg >= 11) && (canGet == true) && (canGet2 == false)) //Tests whether creampied and if the RNG allows it, for now. Later it will test creampied vs the mentstrual stage plus some RNG.
            {
                pregStage = 12;
                Debug.Log(pregStage + ": Staging, ignore, not needed yet");
                girl.pregnant[1] = pregStage; //Trigger the game's pregnancy system.
                girl.pregnant[0] = man.friendID; //Pregnancy system requires the father be set.


                if (girl.pregnant[1] == 12)
                {

                    Debug.Log(girl.pregnant[1] + ": Default pregnancy state");
                    Debug.Log(girl.pregnant[0] + ": Return ID of sperm donor");
                    pregStage = 0; //Reset used variables, just in case.
                    creamed = false;
                    getsIt = null;
                    givesIt = null;
                    canGet = false;
                    canGet2 = false;
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
                pregresult = PregCheckCall(girl, man);

                __result = pregresult;

                if (__result)
                {
                    ___mn.uiMN.FriendHealthCheck(girl);
                    ___mn.sound.GoSound(108, girl.transform.position, randomPitch: true);
                }
            }

        }
    }
}
