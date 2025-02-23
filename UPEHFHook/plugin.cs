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
        internal static Texture2D cassieTex;
        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;
        private readonly HashSet<string> IncludeScenes = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
        {
            "scene_01"
        };

        void Awake()
        {
            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler");
            AssetBundle val = AssetBundle.LoadFromFile("miassets");
            if (val == null)
            {
                Log.LogError("Failed to load assets!");
                return;
            }

            cassieSkel = val.LoadAsset<SkeletonDataAsset>("Assets/modassets/cassie_01.json");
            cassieTex = val.LoadAsset<Texture2D>("Assets/modassets/cassie_01.png");

            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(GetPreg));
            harmony.PatchAll(typeof(PBAttach));

            Log.LogInfo("Fill them up.");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (IncludeScenes.Contains(scene.name))
            {
                init_scene01();
            }
        }

        private void init_scene01()
        {
            var componentsToInitialize = new List<Type>
            {

                typeof(SkinManager),
                typeof(PBAttach.SendSkeletonAndPart)
            };

            foreach (var componentType in componentsToInitialize)
            {
                if (FindObjectOfType(componentType) == null)
                {
                    InstantiateSingleton(componentType);
                }
            }
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
