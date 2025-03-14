using HarmonyLib;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UPEHFHook;


public static class Skeleton
{
        [HarmonyPatch(typeof(SexManager))]
        [HarmonyPatch("SexCountChange")]
        [HarmonyPostfix]

        static void Postfix(CommonStates to, CommonStates from, SexManager.SexCountState sexState)
        {
            LogSkeletonInfo("to", to);
            LogSkeletonInfo("from", from);
        }
    static void LogSkeletonInfo(string label, CommonStates commonState)
        {
            if (commonState == null)
            {
                UPEHFBase.Log.LogWarning($"[{label}] CommonStates is null.");
                return;
            }

            GameObject go = commonState.gameObject;
            if (go == null)
            {
                UPEHFBase.Log.LogWarning($"[{label}] GameObject is null.");
                return;
            }

            UPEHFBase.Log.LogInfo($"[{label}] GameObject: {go.name}");
            // debug on need
            // LogTransformHierarchy(go.transform, "  ");

            SkeletonAnimation skeletonAnim = go.GetComponentInChildren<SkeletonAnimation>(true);
            if (skeletonAnim != null)
            {
                UPEHFBase.Log.LogInfo($"[{label}] Found SkeletonAnimation: {skeletonAnim.name}");
                string id = ExtractIdFromName(skeletonAnim.name);
                if (!string.IsNullOrEmpty(id))
                {
                    if (!SkeletonSwapper.skeletonDictionary.ContainsKey(id))
                    {
                        SkeletonSwapper.skeletonDictionary.Add(id, skeletonAnim);
                        UPEHFBase.Log.LogInfo($"[{label}] Ajout de SkeletonAnimation avec key: {id}");
                    }
                    else
                    {
                        SkeletonSwapper.skeletonDictionary[id] = skeletonAnim;
                        UPEHFBase.Log.LogInfo($"[{label}] Mise à jour de SkeletonAnimation avec key: {id}");
                    }
                }
                else
                {
                    UPEHFBase.Log.LogWarning($"[{label}] Impossible d'extraire l'ID depuis le nom: {skeletonAnim.name}");
                }

                if (skeletonAnim.Skeleton != null && skeletonAnim.Skeleton.Data != null)
                {
                    UPEHFBase.Log.LogInfo($"[{label}] Skeleton Data: {skeletonAnim.Skeleton.Data.Name}");
                }
                else
                {
                    UPEHFBase.Log.LogWarning($"[{label}] Skeleton or Skeleton Data not available{skeletonAnim.name}");
                }
            }
            else
            {
                UPEHFBase.Log.LogWarning($"[{label}] No component SkeletonAnimation found in the children {go.name}");
            }
        }
        static string ExtractIdFromName(string name)
        {
            int startIndex = name.IndexOf('(');
            int endIndex = name.IndexOf(')');
            if (startIndex >= 0 && endIndex > startIndex)
            {
                return name.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
            }
            return "";
        }

        static void LogTransformHierarchy(Transform t, string indent)
        {
            UPEHFBase.Log.LogInfo(indent + t.name);
            foreach (Transform child in t)
            {
                LogTransformHierarchy(child, indent + "  ");
            }
        }
}