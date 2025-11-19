using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UPEHFHook;

public class SkeletonSwapper : MonoBehaviour
{
    public KeyCode triggerKey2 = KeyCode.F4;
    public KeyCode triggerKey3 = KeyCode.F5;

    private static SkeletonAnimation playerSkeletonAnimation;
    private static SkeletonDataAsset originalSkeletonDataAsset;
    private static string originalSkinName;

    private static SkeletonDataAsset swappedSkeletonDataAsset;
    private static bool isSwapped = false;

    public bool IsDoneLoading { get; private set; } = false;
    public static Dictionary<string, SkeletonAnimation> skeletonDictionary = new Dictionary<string, SkeletonAnimation>();
    public static readonly List<string> allowedIDs = new List<string>
    {
        "bakunyu_01",
        "boss_prison_01",
        "cassie_01",
        "genbba_02",
        "gengiant_01",
        "girlfriend_01",
        "girlfriend_02",
        "santa_01"
    };
    public static int[] params1 = new int[10];

    public static SkeletonSwapper Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UPEHFBase.Log.LogInfo("SkeletonSwapper singleton instance created.");
        }
        else
        {
            UPEHFBase.Log.LogWarning("SkeletonSwapper singleton exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        // playerSkeletonAnimation = ...;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey2))
        {
            UPEHFBase.Log.LogWarning("trigger F4...");
            CleanAndTrackSkeletons();
            // SwapSkeleton("genbba_02"); // fail !
        }
        if (Input.GetKeyDown(triggerKey3))
        {
            UPEHFBase.Log.LogWarning("trigger F5...");
            RevertSkeleton();
        }
    }

    public static void CleanAndTrackSkeletons()
    {
        var keysToRemove = skeletonDictionary.Keys
            .Where(id => !allowedIDs.Any(allowed => string.Equals(allowed, id, System.StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var key in keysToRemove)
        {
            UPEHFBase.Log.LogInfo($"Removing entry '{key}' as it is not allowed.");
            skeletonDictionary.Remove(key);
        }
        foreach (var kvp in skeletonDictionary)
        {
            string id = kvp.Key;
            SkeletonAnimation skelAnim = kvp.Value;
            string currentAnim = "none";

            if (skelAnim != null && skelAnim.AnimationState != null)
            {
                var currentTrack = skelAnim.AnimationState.GetCurrent(0);
                if (currentTrack != null && currentTrack.Animation != null)
                    currentAnim = currentTrack.Animation.Name;
            }
            UPEHFBase.Log.LogInfo($"ID: {id} | SkeletonAnimation: {skelAnim.name} | Current Animation: {currentAnim}");
        }
        SwapMatchingSkeletons();
    }


    public static void SwapSkeleton(string id)
    {
        if (playerSkeletonAnimation == null)
        {
            UPEHFBase.Log.LogError("[SkeletonSwapper] No playerSkeletonAnimation assigned!");
            return;
        }

        SkeletonDataAsset newSkeletonAsset = null;
        if (SkeletonBundleLoader.allowedSkeletonAssets.TryGetValue(id, out newSkeletonAsset))
        {
            swappedSkeletonDataAsset = newSkeletonAsset;
        }
        else
        {
            UPEHFBase.Log.LogError($"[SkeletonSwapper] No SkeletonDataAsset found for id '{id}'.");
            return;
        }

        if (originalSkeletonDataAsset == null)
        {
            originalSkeletonDataAsset = playerSkeletonAnimation.skeletonDataAsset;
            originalSkinName = playerSkeletonAnimation.Skeleton?.Data?.DefaultSkin?.Name;
        }

        playerSkeletonAnimation.skeletonDataAsset = swappedSkeletonDataAsset;
        playerSkeletonAnimation.Initialize(true);

        var defaultSkin = swappedSkeletonDataAsset.GetSkeletonData(true)?.DefaultSkin?.Name;
        if (!string.IsNullOrEmpty(defaultSkin))
        {
            playerSkeletonAnimation.Skeleton.SetSkin(defaultSkin);
            playerSkeletonAnimation.Skeleton.SetToSetupPose();
        }
        //playerSkeletonAnimation.AnimationState.SetAnimation(0, "A_idle", true);

        isSwapped = true;
        UPEHFBase.Log.LogInfo($"[SkeletonSwapper] Swapped to skeleton '{swappedSkeletonDataAsset.name}'.");
    }
    public static void SwapMatchingSkeletons()
    {
        foreach (var kvp in skeletonDictionary)
        {
            string id = kvp.Key;
            SkeletonAnimation skelAnim = kvp.Value;
            params1 = skelAnim.GetComponent<CommonStates>().parameters;

            if (SkeletonBundleLoader.allowedSkeletonAssets.TryGetValue(id, out SkeletonDataAsset newAsset))
            {
                UPEHFBase.Log.LogInfo($"Swapping skeleton for key: {id} with asset: {newAsset.name}");


                skelAnim.skeletonDataAsset = newAsset;
                skelAnim.GetComponent<CommonStates>().parameters = params1;
                Slot slot1 = skelAnim.skeleton.FindSlot("Body_preg");
                slot1.Attachment = null;
                skelAnim.Initialize(true);

                string defaultSkin = newAsset.GetSkeletonData(true)?.DefaultSkin?.Name;
                if (!string.IsNullOrEmpty(defaultSkin))
                {
                    skelAnim.Skeleton.SetSkin(defaultSkin);
                    slot1.Attachment = null;
                    skelAnim.Skeleton.SetToSetupPose();

                }

                // Optionnel : lancer l'animation "IDLE"
                //skelAnim.AnimationState.SetAnimation(0, "A_idle", true);
            }
            else
            {
                UPEHFBase.Log.LogWarning($"No allowed SkeletonDataAsset found for key: {id}");
            }
        }
    }


    public static void RevertSkeleton()
    {
        if (playerSkeletonAnimation == null)
        {
            UPEHFBase.Log.LogError("[SkeletonSwapper] No playerSkeletonAnimation assigned!");
            return;
        }
        if (!isSwapped)
        {
            UPEHFBase.Log.LogWarning("[SkeletonSwapper] No swap has been performed.");
            return;
        }
        playerSkeletonAnimation.skeletonDataAsset = originalSkeletonDataAsset;
        playerSkeletonAnimation.Initialize(true);

        if (!string.IsNullOrEmpty(originalSkinName))
        {
            playerSkeletonAnimation.Skeleton.SetSkin(originalSkinName);
            playerSkeletonAnimation.Skeleton.SetToSetupPose();
        }

        isSwapped = false;
        UPEHFBase.Log.LogInfo("[SkeletonSwapper] Revert performed, original skeleton restored.");
    }
}
