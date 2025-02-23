using BepInEx.Logging;
using JetBrains.Annotations;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using UPEHFHook;
using UnityEngine.UI;


namespace UPEHFHook.Patches
{
    class PBAttach
    {
        public class SendSkeletonAndPart : MonoBehaviour
        {
            public static SendSkeletonAndPart Instance { get; private set; }
            private bool reverse = false;
            public CommonStates activeCommon;
            private SexManager sexM;

            // get more skeleton convert it to a dictionnary string + skeletonanimation : send the name of the skeletonanimation that you want to apply your body 
            SkeletonAnimation skeletonNative;
            SkeletonAnimation skeletongirlplayer;


            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
                Instance = this;
                DontDestroyOnLoad(gameObject);
                UPEHFBase.Log.LogInfo("SendSkeletonAndPart singleton instance created.");
            }

            public SkeletonAnimation GetPlayerGirlSkel() // get a skeleton dynamically (you might want to use that method depend the goal)
            {
                GameObject playerGirl = GameObject.Find("PlayerGirl");
                if (playerGirl == null)
                {
                    UPEHFBase.Log.LogError("PlayerGirl not found.");
                    return null;
                }
                SkeletonAnimation skeletonAnim = playerGirl.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnim == null)
                {
                    UPEHFBase.Log.LogError("SkeletonAnimation not found in PlayerGirl's children.");
                    return null;
                }

                UPEHFBase.Log.LogInfo("PlayerGirl SkeletonAnimation init.");
                return skeletonAnim;
            }
            private void DebugListChildComponents(GameObject parent)
            {
                if (parent == null)
                {
                    UPEHFBase.Log.LogError("Parent GameObject is null.");
                    return;
                }

                // Get Components names

                UPEHFBase.Log.LogInfo($"GameObject: {parent.name}");
                Component[] components = parent.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    UPEHFBase.Log.LogInfo($"  Component: {comp.GetType().Name}");
                }

                // Get the childs recursively 
                foreach (Transform child in parent.transform)
                {
                    DebugListChildComponents(child.gameObject);
                }
            }
            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.F4))
                {

                    reverse = !reverse;


                    CoroutineHelper.Instance.StartCoroutine(Looping_Update(reverse));
                }
            }

            private IEnumerator Looping_Update(bool stop)
            {
                UPEHFBase.Log.LogInfo($"stop : {stop}");
                while (!stop)
                {
                    ApplyCustomClothToPlayer(true);
                    yield return new WaitForSeconds(3f);
                }
            }
            public void SkelGetter()
            {
                if (sexM == null)
                {
                    sexM = FindObjectOfType<SexManager>();
                    //for example the variable of the skeletonanimation is named  "anim" like in commonstate
                    //optimally you want a dictionnary of skeletonanimations but we will keep it simple 
                    skeletonNative = sexM.GetComponent<SkeletonAnimation>(); // we get the skeletonanimation of the native in the sexmanager (hypothetical)
                    if (sexM == null)
                        UPEHFBase.Log.LogInfo("SkeletonNative is null.");
                }
            }

            void ApplyCustomClothToPlayer(bool looping)
            {
                if (looping)
                    UPEHFBase.Log.LogInfo("Part updated");

                if (skeletongirlplayer == null)
                {
                    UPEHFBase.Log.LogWarning("Player's skeleton is null attempt to get player object...");
                    skeletongirlplayer = GetPlayerGirlSkel();
                    if (skeletongirlplayer == null)
                    {
                        UPEHFBase.Log.LogError("Player's skeleton still null probably the title scene...");
                        return;
                    }
                }

                List<string> partsToApply = new List<string> { "Body_cloth" };
                //SkinManager.Instance.ApplyModSkin("body_preg_small", "yona", skeletongirlplayer, partsToApply, true);
                SkinManager.Instance.ApplyModSkin("body_preg_big", "yona", skeletongirlplayer, partsToApply, true);
                UPEHFBase.Log.LogInfo(string.Join(", ", partsToApply));
            }
        }
    }
}

