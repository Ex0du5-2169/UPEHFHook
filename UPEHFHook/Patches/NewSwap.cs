using BepInEx;
using HarmonyLib;
using HFramework;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YotanModCore;

namespace UPEHFHook.Patches
{
    internal class NewSwap
    {

        [HarmonyPatch(typeof(NPCManager))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]

        public static void CSPostFix(NPCManager __instance)
        {

            for (int i = 0; i < __instance.npcPrefab.Length; i++)
            {
                GameObject npcPrefab = __instance.npcPrefab[i];
                if (npcPrefab == null) continue;
                string name = npcPrefab.name.Replace("_prefab", "");
                SkeletonAnimation skelAnim = npcPrefab.GetComponentInChildren<SkeletonAnimation>();
                CommonStates partner = skelAnim.GetComponent<CommonStates>();

                if (partner.anim.skeleton.FindSlot("body_preg") == null)
                {
                    CommonStates skelCom = skelAnim.GetComponent<CommonStates>();
                    skelCom.parameters = partner.parameters;
                    SkeletonGraphic skelGraph = skelAnim.GetComponent<SkeletonGraphic>();
                    SkeletonDataAsset AssetToUse = null;
                    switch (partner.npcID)
                    {
                        case 5:
                            AssetToUse = UPEHFBase.Reika;
                            break;
                        case 6:
                            AssetToUse = UPEHFBase.Nami;
                            break;
                        case 110:
                            AssetToUse = UPEHFBase.Giant;
                            break;
                        case 113:
                            AssetToUse = UPEHFBase.Cassie;
                            break;
                        case 114:
                            AssetToUse = UPEHFBase.Shino;
                            break;
                        case 115:
                            AssetToUse = UPEHFBase.Sally;
                            break;
                        case 116:
                            AssetToUse = UPEHFBase.Merry;
                            break;
                    }
                    if (AssetToUse == null)
                        return;
                    skelAnim.skeletonDataAsset = AssetToUse;
                    skelAnim.initialSkinName = "default";
                    skelAnim.Initialize(true);
                    skelGraph.SetMaterialDirty();
                    if (partner.pregnant[0] == -1 && skelAnim.skeleton.FindSlot("Body_preg") != null)
                    {
                        skelAnim.skeleton.SetAttachment("Body_preg", null);
                    }
                    else
                    {
                        skelAnim.skeleton.FindSlot("Body_Cloth").SetColor(new Color(1f, 1f, 1f, 0f));
                    }
                }
            }
        }
    }
}
