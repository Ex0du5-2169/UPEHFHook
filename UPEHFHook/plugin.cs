using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HFramework;
using HFramework.Hook;
using HFramework.Performer;
using HFramework.Scenes;
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
    [BepInDependency("HFramework", "1.1.2")]
    public class UPEHFBase : BaseUnityPlugin
    {
        private const string modGUID = "Ex.MadIslandUPE";
        private const string modName = "Ex Universal Pregnancy Enabler";
        private const string modVersion = "1.0.0";


        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;
        public static SkeletonDataAsset[] skelDataAssets;
        public static AssetBundle Assets;

        

        void Awake()
        {
            UPEHFHook.Config.Instance.Init(((BaseUnityPlugin)this).Config);
            
            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler 0.9.9-test-01");
            string assetPath = Path.Combine(Path.GetDirectoryName(Info.Location), "Assets/miassets");
            Assets = AssetBundle.LoadFromFile(assetPath);
            if (Assets == null)
            {
                Log.LogError("Failed to load assets!");
                return;
            }
            harmony.PatchAll(typeof(AssetsLoader));
            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(GetPreg));
            //harmony.PatchAll(typeof(HFSpawnChild));


            Log.LogInfo("Fill them up.");

            PerformerLoader.OnLoadPeformers += () =>
            {
                string[] array = new string[1]
                {
                "ExDelivery_Performers.xml"
                };
                string[] array2 = array;
                foreach (string text in array2)
                {
                    PerformerLoader.AddPerformersFromFile("BepInEx/plugins/UPEdefinitions/" + text);
                    Log.LogInfo("Loading custom performers:" + text);
                }
            };
            ScenesManager.OnRegisterScenes += () =>
            {
                string[] array3 = new string[1]
{
                "ExDelivery_Scenes.xml"
};
                string[] array4 = array3;
                foreach (string text2 in array4)
                {
                    ScenesLoader.LoadScenesFromFile("BepInEx/plugins/UPEdefinitions/" + text2);
                    Log.LogInfo("Loading custom scenes:" + text2);
                }
            };
            
        }

    }
}
