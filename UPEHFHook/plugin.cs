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
using HFramework;
using HFramework.Performer;
using HFramework.Scenes;
using HFramework.Hook;


namespace UPEHFHook
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("HFramework", "1.1.0")]
    public class UPEHFBase : BaseUnityPlugin
    {
        private const string modGUID = "Ex.MadIslandUPE";
        private const string modName = "Ex Universal Pregnancy Enabler";
        private const string modVersion = "1.0.0";


        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;

        public static SkeletonDataAsset Reika;
        public static SkeletonDataAsset Nami;
        public static SkeletonDataAsset Cassie;
        public static SkeletonDataAsset Shino;
        public static SkeletonDataAsset Sally;
        public static SkeletonDataAsset Giant;
        public static SkeletonDataAsset ESis;
        public static SkeletonDataAsset Merry;


        void Awake()
        {
            UPEHFHook.Config.Instance.Init(((BaseUnityPlugin)this).Config);
            
            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler 0.9.9-beta-8");
            string location = ((BaseUnityPlugin)this).Info.Location;
            string newtext = "UPEHFHook.dll";
            string text1 = location.TrimEnd(newtext.ToCharArray());
            string text3 = text1 + "miassets";
            AssetBundle val = AssetBundle.LoadFromFile(text3);
            if (val == null)
            {
                UPEHFBase.Log.LogError("Failed to load assets!");
                return;
            }
            Reika = val.LoadAsset<SkeletonDataAsset>("modassets/girlfriend_01/girlfriend_01_SkeletonData.asset");
            Nami = val.LoadAsset<SkeletonDataAsset>("modassets/girlfriend_02/girlfriend_02_SkeletonData.asset");
            Merry = val.LoadAsset<SkeletonDataAsset>("modassets/santa_01/santa_01_SkeletonData.asset");
            ESis = val.LoadAsset<SkeletonDataAsset>("modassets/genbba_02/genbba_02_SkeletonData.asset");
            Giant = val.LoadAsset<SkeletonDataAsset>("modassets/gengiant_01/gengiant_01_SkeletonData.asset");
            Shino = val.LoadAsset<SkeletonDataAsset>("modassets/bakunyu_01/bakunyu_01_SkeletonData.asset");
            Cassie = val.LoadAsset<SkeletonDataAsset>("modassets/cassie_01/cassie_01_SkeletonData.asset");
            Sally = val.LoadAsset<SkeletonDataAsset>("modassets/boss_prison_01/boss_prison_01_SkeletonData.asset");

            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(GetPreg));
            harmony.PatchAll(typeof(Skeleton));
            harmony.PatchAll(typeof(HFSpawnChild));
            harmony.PatchAll(typeof(NewSwap));


            Log.LogInfo("Fill them up.");

            var componentsToInitialize = new List<Type>
        {
            typeof(SkeletonSwapper),
            typeof(SkeletonBundleLoader),

        };

            foreach (var componentType in componentsToInitialize)
            {
                if (FindObjectOfType(componentType) == null)
                {
                    InstantiateSingleton(componentType);
                }
            }
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
        private void InstantiateSingleton(Type type)
        {
            GameObject obj = new GameObject(type.Name);
            obj.AddComponent(type);
            DontDestroyOnLoad(obj);
            Log.LogInfo($"{type.Name} instantiated.");
        }

    }
}
