using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using Spine;
using UPEHFHook.Patches;
using UnityEngine.SceneManagement;

namespace UPEHFHook
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class UPEHFBase : BaseUnityPlugin
    {
        private const string modGUID = "Ex.MadIslandUPE";
        private const string modName = "Ex Universal Pregnancy Enabler";
        private const string modVersion = "1.0.0";

        internal static SkeletonDataAsset cassieSkel;
        internal static SkeletonDataAsset genbba2Skel;
        internal static SkeletonDataAsset reikaSkel;
        internal static SkeletonDataAsset namiSkel;
        internal static SkeletonDataAsset shinoSkel;
        internal static SkeletonDataAsset sallySkel;
        internal static SkeletonDataAsset giantSkel;
        internal static SkeletonDataAsset lfemSkel;
        internal static SkeletonDataAsset merrySkel;

        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;


        void Awake()
        {
            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler");
            string location = ((BaseUnityPlugin)this).Info.Location;
            location = location.Replace("\\", "/");
            string text = "UPEHFHook.dll";
            string text2 = location.TrimEnd(text.ToCharArray());
            string text3 = text2 + "miassets";
            AssetBundle val = AssetBundle.LoadFromFile(text3);
            if (val == null)
            {
                Log.LogError("Failed to load assets!");
                return;
            }

            cassieSkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/cassie_01.json");
            genbba2Skel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/genbba_02.json");
            reikaSkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/girlfriend_01.json");
            namiSkel = val.LoadAsset<SkeletonDataAsset>("Assets/ModAssets/girlfriend_02.json");
            shinoSkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/bakunyu_01.json");
            sallySkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/boss_prison_01.json");
            giantSkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/gengiant_01.json");
            lfemSkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/gengirl_03.json");
            merrySkel = val.LoadAsset<SkeletonDataAsset>("Assets/Modassets/santa_01.json");

            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(GetPreg));
            harmony.PatchAll(typeof(Skeleton));

            Log.LogInfo("Fill them up.");

        }

    }
}
