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
    [HarmonyPatch(typeof(NPCManager), "Start")]
    public class RandomCharacterPatch
    {
        static void Postfix(GameObject[] npcPrefab, object girl, object man)
        {
            Transform animTransform = npcPrefab[90].transform.Find("Scale/Anim");
            if (animTransform != null)
            {
                
                SkeletonAnimation skeletonAnim = animTransform.GetComponent<SkeletonAnimation>();
                if (skeletonAnim != null)
                {
                    UPEHFBase.Log.LogInfo("SkeletonAnimation name: " + skeletonAnim.name);

                    if (skeletonAnim.Skeleton != null && skeletonAnim.Skeleton.Data != null)
                    {
                        UPEHFBase.Log.LogInfo("Skeleton Data Name: " + skeletonAnim.Skeleton.Data.Name);
                    }
                    else
                    {
                        UPEHFBase.Log.LogInfo("Skeleton or Skeleton Data not available.");
                    }
                }
                else
                {
                    UPEHFBase.Log.LogInfo("Composant SkeletonAnimation cannot be found " + animTransform.name);
                }
            }
            else
            {
                UPEHFBase.Log.LogInfo("Transform 'Scale/Anim' cannot be found in hChara");
            }
        }
    }
}