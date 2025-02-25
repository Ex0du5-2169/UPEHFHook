using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Spine.Unity;
using UPEHFHook;
using System.Linq;

public class SkeletonBundleLoader : MonoBehaviour
{
    public string bundlesDirectory = "AssetBundles";

    private List<string> animationNames = new List<string>();
    private List<SkeletonDataAsset> skeletonDataAssets = new List<SkeletonDataAsset>();
    private Dictionary<string, SkeletonDataAsset> skeletonDataDict =
        new Dictionary<string, SkeletonDataAsset>(System.StringComparer.OrdinalIgnoreCase);

    public static Dictionary<string, SkeletonDataAsset> allowedSkeletonAssets = new Dictionary<string, SkeletonDataAsset>(System.StringComparer.OrdinalIgnoreCase);

    public static readonly List<string> allowedIDs = new List<string>
    {
        "bakunyu_01",
        "boss_prison_01",
        "cassie_01",
        "genbba_02",
        "gengiant_01",
        "gengirl_03",
        "girlfriend_01",
        "girlfriend_02",
        "santa_01"
    };

    private Camera mainCamera;
    private static SkeletonBundleLoader _instance;
    public static SkeletonBundleLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SkeletonBundleLoader>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("Skeleton_Loader");
                    _instance = obj.AddComponent<SkeletonBundleLoader>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (Camera.main != null)
            mainCamera = Camera.main;
        else
        {
            mainCamera = FindObjectOfType<Camera>();
            if (mainCamera == null)
                UPEHFBase.Log.LogError("[Skeleton_Loader] No camera found in the scene.");
        }

        StartCoroutine(LoadAllSkeletons());
    }

    private IEnumerator LoadAllSkeletons()
    {
        string fullDirectoryPath = Path.Combine(Application.streamingAssetsPath, bundlesDirectory);
        fullDirectoryPath = fullDirectoryPath.Replace("\\", "/");

        if (!Directory.Exists(fullDirectoryPath))
        {
            UPEHFBase.Log.LogError($"[Skeleton_Loader] AssetBundles directory not found at path: {fullDirectoryPath}");
            yield break;
        }
        string[] bundleFiles = Directory.GetFiles(fullDirectoryPath, "*", SearchOption.AllDirectories);

        foreach (string bundlePath in bundleFiles)
        {
            string extension = Path.GetExtension(bundlePath).ToLower();
            if (extension != ".bundle" && extension != ".assetbundle" && !string.IsNullOrEmpty(extension))
                continue;

            UPEHFBase.Log.LogInfo($"[Skeleton_Loader] Attempting to load AssetBundle: {bundlePath}");
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return bundleLoadRequest;

            AssetBundle bundle = bundleLoadRequest.assetBundle;
            if (bundle == null)
            {
                UPEHFBase.Log.LogError($"[Skeleton_Loader] Failed to load AssetBundle from {bundlePath}");
                continue;
            }
            string[] assetNames = bundle.GetAllAssetNames();
            foreach (string assetName in assetNames)
            {
                if (assetName.EndsWith(".asset", System.StringComparison.OrdinalIgnoreCase))
                {
                    AssetBundleRequest assetLoadRequest = bundle.LoadAssetAsync<SkeletonDataAsset>(assetName);
                    yield return assetLoadRequest;

                    SkeletonDataAsset skeletonDataAsset = assetLoadRequest.asset as SkeletonDataAsset;
                    if (skeletonDataAsset != null)
                    {
                        skeletonDataAssets.Add(skeletonDataAsset);
                        skeletonDataDict[skeletonDataAsset.name] = skeletonDataAsset;

                        UPEHFBase.Log.LogInfo($"[Skeleton_Loader] Loaded SkeletonDataAsset: {assetName} from bundle: {bundlePath}");
                        LogAvailableAnimations(skeletonDataAsset);
                        LogAvailableSkins(skeletonDataAsset);

                        string id = ExtractIdFromSkeletonDataAssetName(skeletonDataAsset.name);
                        if (!string.IsNullOrEmpty(id) && allowedIDs.Contains(id, System.StringComparer.OrdinalIgnoreCase))
                        {
                            if (!allowedSkeletonAssets.ContainsKey(id))
                            {
                                allowedSkeletonAssets.Add(id, skeletonDataAsset);
                                UPEHFBase.Log.LogInfo($"[Skeleton_Loader] Added allowed SkeletonDataAsset with id: {id}");
                            }
                        }
                    }
                    else
                    {
                        UPEHFBase.Log.LogWarning($"[Skeleton_Loader] File '{assetName}' in '{bundlePath}' is not a valid SkeletonDataAsset.");
                    }
                }
            }
            bundle.Unload(false);
        }

        UPEHFBase.Log.LogInfo("[Skeleton_Loader] All asset bundles got checked.");
    }

    private string ExtractIdFromSkeletonDataAssetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "";
        int index = name.IndexOf("_SkeletonData", System.StringComparison.OrdinalIgnoreCase);
        if (index > 0)
            return name.Substring(0, index);
        return "";
    }

    public SkeletonDataAsset GetSkeletonDataByName(string skeletonName)
    {
        if (skeletonDataDict.TryGetValue(skeletonName, out var asset))
            return asset;
        else
        {
            UPEHFBase.Log.LogWarning($"[Skeleton_Loader] SkeletonData '{skeletonName}' not found in dictionary!");
            return null;
        }
    }

    public void LogAvailableAnimations(SkeletonDataAsset skeletonDataAsset)
    {
        if (skeletonDataAsset != null && skeletonDataAsset.GetSkeletonData(true) != null)
        {
            var animations = skeletonDataAsset.GetSkeletonData(true).Animations;
            UPEHFBase.Log.LogInfo($"[Skeleton_Loader] Available Animations for '{skeletonDataAsset.name}':");
            foreach (var anim in animations)
            {
                UPEHFBase.Log.LogInfo($"    - {anim.Name}");
                animationNames.Add(anim.Name);
            }
        }
        else
        {
            UPEHFBase.Log.LogWarning($"[Skeleton_Loader] Cannot log animations. SkeletonDataAsset '{skeletonDataAsset?.name}' or its SkeletonData is null.");
        }
    }
    public void LogAvailableSkins(SkeletonDataAsset skeletonDataAsset)
    {
        if (skeletonDataAsset != null && skeletonDataAsset.GetSkeletonData(true) != null)
        {
            var skins = skeletonDataAsset.GetSkeletonData(true).Skins;
            UPEHFBase.Log.LogInfo($"[Skeleton_Loader] Available Skins for '{skeletonDataAsset.name}':");
            if (skins.Count == 0)
            {
                UPEHFBase.Log.LogInfo("    - No skin available.");
                return;
            }
            foreach (var skin in skins)
            {
                UPEHFBase.Log.LogInfo($"    - {skin.Name}");
            }
        }
        else
        {
            UPEHFBase.Log.LogWarning($"[Skeleton_Loader] Cannot log skins. SkeletonDataAsset '{skeletonDataAsset?.name}' or its SkeletonData is null.");
        }
    }

    public void LogAllAvailableSkins()
    {
        if (skeletonDataAssets.Count == 0)
        {
            UPEHFBase.Log.LogInfo("[Skeleton_Loader] No SkeletonDataAsset loaded to list skins.");
            return;
        }
        UPEHFBase.Log.LogInfo("[Skeleton_Loader] Listing all available skins for loaded SkeletonDataAssets:");
        foreach (var skeletonDataAsset in skeletonDataAssets)
            LogAvailableSkins(skeletonDataAsset);
    }
}
