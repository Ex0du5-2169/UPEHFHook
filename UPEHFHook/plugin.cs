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


        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;


        void Awake()
        {
            Log = this.Logger;
            Log.LogInfo("Mad Island Universal Pregnancy Enabler");
            /*string location = ((BaseUnityPlugin)this).Info.Location;
            location = location.Replace("\\", "/");
            string text = "UPEHFHook.dll";
            string text2 = location.TrimEnd(text.ToCharArray());
            string text3 = text2 + "miassets";
            AssetBundle val = AssetBundle.LoadFromFile(text3);
            if (val == null)
            {
                Log.LogError("Failed to load assets!");
                return;
            }*/

            harmony.PatchAll(typeof(UPEHFBase));
            harmony.PatchAll(typeof(GetPreg));
            harmony.PatchAll(typeof(Skeleton));


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
