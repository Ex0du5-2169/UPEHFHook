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
        [HarmonyPatch(typeof(CommonStates))]
        [HarmonyPatch("LoveChange")]
        [HarmonyPostfix]

        public static void CSPostFix(CommonStates partner)
        {
            CommonStates commonNew = new CommonStates();
            commonNew = partner;
            if (commonNew.anim.skeleton.FindSlot("body_preg") == null)
            {
                GameObject newobj = commonNew.gameObject;
                SkeletonGraphic skelgraphic = newobj.GetComponent<SkeletonGraphic>();
                switch (commonNew.npcID)
                {
                    case 5:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Reika;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        //newobj = UnityEngine.Object.Instantiate(newobj);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 6:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Nami;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 90:
                        skelgraphic.skeletonDataAsset = UPEHFBase.ESis;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 110:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Giant;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 113:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Cassie;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 114:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Shino;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 115:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Sally;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                    case 116:
                        skelgraphic.skeletonDataAsset = UPEHFBase.Merry;
                        skelgraphic.initialSkinName = "default";
                        UPEHFBase.Log.LogInfo("Swapping data asset for " + commonNew.npcID);
                        skelgraphic.SetMaterialDirty();
                        skelgraphic.Initialize(true);
                        skelgraphic.AnimationState.SetAnimation(0, "A_Idle", true);
                        break;
                }
            }
        }
    }
}
