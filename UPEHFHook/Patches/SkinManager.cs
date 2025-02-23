using BepInEx.Logging;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UPEHFHook;

[Serializable]
public class ModConfig
{
    public bool resetSlotColors = true;
    public bool disableOriginalAttachments = false;
}

public class SkinManager : MonoBehaviour
{
    private Dictionary<Skeleton, Skin> customSkins = new Dictionary<Skeleton, Skin>();
    private Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
    private Dictionary<string, Material> materialCache = new Dictionary<string, Material>();
    private Dictionary<string, Attachment> customAttachments = new Dictionary<string, Attachment>(StringComparer.OrdinalIgnoreCase);

    public string modsFolder = "image/Skins";
    public SkeletonAnimation skeletonAnim;
    private static SkinManager _instance;

    public static SkinManager Instance

    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SkinManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SkinManager");
                    _instance = obj.AddComponent<SkinManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMods();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeMods()
    {
        UPEHFBase.Log.LogInfo("InitializeMods called.");
        string modsPath = Path.Combine(Application.streamingAssetsPath, modsFolder).Replace("\\", "/");
        if (!Directory.Exists(modsPath))
        {
            UPEHFBase.Log.LogWarning($"Mods folder not found: {modsPath}");
            return;
        }

        string[] modFolders = Directory.GetDirectories(modsPath);
        foreach (string modFolder in modFolders)
        {
            string modName = Path.GetFileName(modFolder);
            string[] characterFolders = Directory.GetDirectories(modFolder);
            foreach (string characterFolder in characterFolders)
            {
                string characterName = Path.GetFileName(characterFolder);

                if (characterName.Equals("bench", StringComparison.OrdinalIgnoreCase))
                {
                    UPEHFBase.Log.LogInfo($"Skipping 'bench' folder at: {characterFolder}");
                    continue;
                }
            }
        }
    }

    public void ApplyModSkin(string modName, string characterName, SkeletonAnimation skeletonAnim, List<string> partsToApply = null, bool forceReload = false)
    {
        if (skeletonAnim == null)
        {
            UPEHFBase.Log.LogError("SkeletonAnimation is null.");
            return;
        }

        string modsPath = Path.Combine(Application.streamingAssetsPath, modsFolder).Replace("\\", "/");
        string modPath = Path.Combine(modsPath, modName, characterName).Replace("\\", "/");

        if (!Directory.Exists(modPath))
        {
            UPEHFBase.Log.LogWarning($"Mod path not found: {modPath}");
            return;
        }

        Dictionary<string, string> partToIcon = GetPartsAndIcons(modPath);

        if (partsToApply != null && partsToApply.Count > 0)
        {
            partToIcon = partToIcon
                .Where(kvp => partsToApply.Contains(kvp.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

        ApplyCustomSkins(partToIcon, skeletonAnim, forceReload);
        skeletonAnim.UpdateComplete -= HandleAnimationUpdate;
        skeletonAnim.UpdateComplete += HandleAnimationUpdate;
    }

    private Dictionary<string, string> GetPartsAndIcons(string modPath)
    {
        Dictionary<string, string> partToIcon = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string partsPath = modPath;
        string iconsPath = Path.Combine(modPath, "icons").Replace("\\", "/");

        if (!Directory.Exists(partsPath))
        {
            UPEHFBase.Log.LogWarning($"Parts folder not found at: {partsPath}");
            return partToIcon;
        }

        if (!Directory.Exists(iconsPath))
        {
            UPEHFBase.Log.LogWarning($"Icons folder not found at: {iconsPath}");
            return partToIcon;
        }

        string[] partFiles = Directory.GetFiles(partsPath, "*.png", SearchOption.TopDirectoryOnly);
        foreach (string partFile in partFiles)
        {
            string partName = Path.GetFileNameWithoutExtension(partFile);
            string expectedIconName = partName + "_icon.png";
            string iconPath = Path.Combine(iconsPath, expectedIconName).Replace("\\", "/");

            if (File.Exists(iconPath))
            {
                partToIcon[partName] = iconPath;
            }
            else
            {
                UPEHFBase.Log.LogWarning($"Icon not found for part '{partName}' at: {iconPath}");
            }
        }

        return partToIcon;
    }

    private void ApplyCustomSkins(
        Dictionary<string, string> partToIcon,
        SkeletonAnimation skeletonAnim,
        bool forceReload)
    {
        Skeleton skeleton = skeletonAnim.Skeleton;
        Skin customSkin;
        if (customSkins.ContainsKey(skeleton))
        {
            customSkin = customSkins[skeleton];
        }
        else
        {
            customSkin = new Skin("customSkin");
            foreach (Slot s in skeleton.Slots)
            {
                Attachment attachment = s.Attachment;
                if (attachment != null)
                {
                    customSkin.SetAttachment(s.Data.Index, s.Data.Name, attachment);
                }
            }
            customSkins[skeleton] = customSkin;
        }

        foreach (var kvp in partToIcon)
        {
            string slotName = kvp.Key;
            string iconPath = kvp.Value;

            Texture2D customTexture = LoadTexture(iconPath, forceReload);
            if (customTexture == null)
            {
                UPEHFBase.Log.LogWarning($"Failed to load texture from: {iconPath}");
                continue;
            }

            Material material = CreateMaterialForTexture(customTexture, skeletonAnim, forceReload);
            AtlasRegion atlasRegion = CreateAtlasRegion(customTexture, material, Path.GetFileName(iconPath));
            if (atlasRegion == null)
            {
                UPEHFBase.Log.LogError("Failed to create AtlasRegion from texture.");
                continue;
            }

            Slot slot = skeleton.FindSlot(slotName);
            if (slot == null)
            {
                UPEHFBase.Log.LogError($"Slot '{slotName}' not found.");
                continue;
            }

            int slotIndex = slot.Data.Index;
            string attachmentName = slot.Data.AttachmentName;
            if (string.IsNullOrEmpty(attachmentName))
            {
                UPEHFBase.Log.LogError($"Attachment name for slot '{slotName}' not found.");
                continue;
            }
            Attachment originalAttachment = skeleton.GetAttachment(slotIndex, attachmentName);
            if (originalAttachment == null)
            {
                UPEHFBase.Log.LogError($"Original attachment '{attachmentName}' not found in slot '{slotName}'.");
                continue;
            }

            Attachment newAttachment = null;
            if (originalAttachment is RegionAttachment regionAttachment)
            {
                newAttachment = regionAttachment.Copy();
                RegionAttachment newRegionAttachment = (RegionAttachment)newAttachment;

                newRegionAttachment.SetRegion(atlasRegion);
                newRegionAttachment.RendererObject = atlasRegion;
                newRegionAttachment.UpdateOffset();
            }
            else if (originalAttachment is MeshAttachment meshAttachment)
            {
                newAttachment = meshAttachment.Copy();
                MeshAttachment newMeshAttachment = (MeshAttachment)newAttachment;
                newMeshAttachment.SetRegion(atlasRegion);
                newMeshAttachment.UpdateUVs();
            }
            else
            {
                UPEHFBase.Log.LogWarning($"Attachment type '{originalAttachment.GetType()}' not supported.");
                continue;
            }

            customSkin.SetAttachment(slotIndex, attachmentName, newAttachment);
            slot.Attachment = newAttachment;

            bool resetSlotColors = true;
            bool disableOriginalAttachments = false;

            //To do later 
            // resetSlotColors = globalModConfig.resetSlotColors;
            // disableOriginalAttachments = globalModConfig.disableOriginalAttachments;

            if (resetSlotColors)
            {
                slot.SetColor(Color.white);
            }
            if (disableOriginalAttachments)
            {
                skeleton.SetAttachment(slotName, null);
            }
            customAttachments[slotName] = newAttachment;
            UPEHFBase.Log.LogInfo($"Custom attachment '{Path.GetFileName(iconPath)}' applied to slot '{slotName}'.");
        }
        skeleton.SetSkin(customSkin);
        skeletonAnim.AnimationState.Apply(skeleton);
    }

    private void HandleAnimationUpdate(ISkeletonAnimation animated)
    {
        Skeleton skeleton = animated.Skeleton;
        foreach (var kvp in customAttachments)
        {
            string slotName = kvp.Key;
            Attachment customAttachment = kvp.Value;

            Slot slot = skeleton.FindSlot(slotName);
            if (slot != null)
            {
                slot.Attachment = customAttachment;
            }
        }
    }

    private AtlasRegion CreateAtlasRegion(Texture2D texture, Material material, string regionName)
    {
        AtlasPage page = new AtlasPage
        {
            name = regionName,
            width = texture.width,
            height = texture.height,
            rendererObject = material
        };
        AtlasRegion region = new AtlasRegion
        {
            page = page,
            name = regionName,
            index = -1,
            rotate = false,
            u = 0f,
            v = 1f,
            u2 = 1f,
            v2 = 0f,
            width = texture.width,
            height = texture.height,
            originalWidth = texture.width,
            originalHeight = texture.height,
            offsetX = 0f,
            offsetY = 0f
        };
        return region;
    }

    public Texture2D LoadTexture(string path, bool forceReload = false)
    {
        string fileName = Path.GetFileName(path);

        if (!forceReload && textureCache.ContainsKey(fileName))
        {
            return textureCache[fileName];
        }

        try
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            if (tex.LoadImage(fileData))
            {
                tex.Apply();
                tex.name = Path.GetFileNameWithoutExtension(path);

                if (textureCache.ContainsKey(fileName))
                {
                    Texture2D oldTexture = textureCache[fileName];
                    Destroy(oldTexture);
                    textureCache[fileName] = tex;
                    UPEHFBase.Log.LogInfo($"[SkinManager] Texture loaded successfully from: {path}");
                }
                else
                {
                    textureCache.Add(fileName, tex);
                    UPEHFBase.Log.LogError($"[SkinManager] Failed to load texture from: {path}");
                }

                return tex;
            }
            else
            {
                UPEHFBase.Log.LogError($"Failed to load texture from path: {path}");
            }
        }
        catch (IOException ioEx)
        {
            UPEHFBase.Log.LogError($"IO Error loading texture: {ioEx.Message}. The file may be locked by another application.");
        }
        catch (Exception ex)
        {
            UPEHFBase.Log.LogError($"Error loading texture: {ex.Message}");
        }
        return null;
    }

    private Material CreateMaterialForTexture(Texture2D texture, SkeletonAnimation skeletonAnim, bool forceReload = false)
    {
        string textureName = texture.name;

        if (!forceReload && materialCache.ContainsKey(textureName))
        {
            return materialCache[textureName];
        }

        Material baseMaterial = skeletonAnim.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial;
        Material material = new Material(baseMaterial);
        material.mainTexture = texture;
        if (materialCache.ContainsKey(textureName))
        {
            Material oldMaterial = materialCache[textureName];
            Destroy(oldMaterial);
            materialCache[textureName] = material;
        }
        else
        {
            materialCache.Add(textureName, material);
        }

        return material;
    }
}
