using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UPEHFHook.Patches;


namespace UPEHFHook
{
  
    [BepInPlugin(modGUID, modName, modVersion)]
    public class UPEHFBase : BaseUnityPlugin
    {
        private const string modGUID = "Ex.MadIslandUPE";
        private const string modName = "Ex Universal Pregnancy Enabler";
        private const string modVersion = "1.1.2";


        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;


        void Awake()
        {

            UPEHFHook.Config.Instance.Init(((BaseUnityPlugin)this).Config);

            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler " + modVersion + " Forked");

            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(AssetsLoader));
            harmony.PatchAll(typeof(GetPreg));

            Log.LogInfo("Get pregnant, get pregnant, get pregnant!");


        }

    }
}
