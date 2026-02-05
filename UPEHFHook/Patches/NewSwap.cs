using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Spine.Unity;
using YotanModCore;
using UnityEngine;
using Spine;

namespace UPEHFHook.Patches
{
    internal class NewSwap
    {
        [HarmonyPatch(typeof(NPCManager))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]

        public static void CSPostFix(CommonStates partner)
        {

            if (partner.anim.skeleton.FindSlot("body_preg") == null)
            {
                SkeletonAnimation skelAnim = partner.anim;
                CommonStates skelCom = skelAnim.GetComponent<CommonStates>();
                skelCom.parameters = partner.parameters;
                SkeletonGraphic skelGraph = skelAnim.GetComponent<SkeletonGraphic>();
                switch (partner.npcID)
                {
                    case 5:
                        skelAnim.skeletonDataAsset = UPEHFBase.Reika;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    case 6:
                        skelAnim.skeletonDataAsset = UPEHFBase.Nami;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    //case 90:
                    case 110:
                        skelAnim.skeletonDataAsset = UPEHFBase.Giant;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    case 113:
                        skelAnim.skeletonDataAsset = UPEHFBase.Cassie;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    case 114:
                        skelAnim.skeletonDataAsset = UPEHFBase.Shino;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    case 115:
                        skelAnim.skeletonDataAsset = UPEHFBase.Sally;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                    case 116:
                        skelAnim.skeletonDataAsset = UPEHFBase.Merry;
                        skelAnim.initialSkinName = "default";
                        skelAnim.Initialize(true);
                        skelGraph.SetMaterialDirty();
                        if (partner.pregnant[0] == -1)
                        {
                            skelAnim.skeleton.SetAttachment("Body_preg", null);
                        }
                        break;
                }
            }
        }
    }
}
