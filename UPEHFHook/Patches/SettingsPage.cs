using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UPEHFHook;

namespace UPE.Patches
{
    internal static class SettingsPage
    {
        private const string ButtonName = "UPE_SettingsButton";
        private const string WindowName = "UPE_Settings";
        private const float RowHeight = 70f;

        private static readonly Color Parchment = new Color(0.93f, 0.85f, 0.70f, 1f);
        private static readonly Color Ink = new Color(0.27f, 0.18f, 0.10f, 1f);
        private static readonly Color TabIdle = new Color(0.80f, 0.68f, 0.50f, 1f);
        private static readonly Color TabActive = new Color(0.60f, 0.45f, 0.28f, 1f);
        private static readonly Color FieldBg = new Color(1f, 0.97f, 0.88f, 1f);
        private static readonly Color RowBg = new Color(0.27f, 0.18f, 0.10f, 0.08f);

        private sealed class Tab
        {
            public GameObject Scroll;
            public Image Image;
            public RectTransform Content;
        }

        private static ManagersScript _mn;
        private static GameObject _menuButton;
        private static bool _readyLogged;
        private static bool _menuErrorLogged;
        private static GameObject _root;
        private static TMP_FontAsset _font;
        private static readonly List<Tab> Tabs = new List<Tab>();
        private static readonly List<Action> Refreshers = new List<Action>();

        internal static void Tick(ManagersScript mn)
        {
            _mn = mn;
            if (mn == null || !mn.allSet || mn.uiMN == null)
            {
                return;
            }
            if (!_readyLogged)
            {
                _readyLogged = true;
                UPEHFBase.Log.LogInfo("Settings: managers ready, adding UPE menu button.");
            }
            try
            {
                EnsureMenuButton(mn);
            }
            catch (Exception ex)
            {
                if (!_menuErrorLogged)
                {
                    _menuErrorLogged = true;
                    UPEHFBase.Log.LogError("Settings: failed to add UPE menu button: " + ex);
                }
            }
            if (_root != null && _root.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        private static void EnsureMenuButton(ManagersScript mn)
        {
            if (_menuButton != null)
            {
                return;
            }
            GameObject menu = mn.uiMN.settingMenu;
            if (menu == null)
            {
                UPEHFBase.Log.LogWarning("Settings: uiMN.settingMenu is null, UPE menu button not added.");
                return;
            }
            Transform template = menu.transform.Find("SettingButton");
            if (template == null)
            {
                foreach (Transform child in menu.transform)
                {
                    if (child.GetComponent<Button>() != null)
                    {
                        template = child;
                        break;
                    }
                }
            }
            if (template == null)
            {
                UPEHFBase.Log.LogWarning("Settings: no SettingButton template under settingMenu, UPE menu button not added.");
                return;
            }

            GameObject button = UnityEngine.Object.Instantiate(template.gameObject, menu.transform, false);
            button.name = ButtonName;
            int index = template.GetSiblingIndex() + 1;
            while (index < menu.transform.childCount && menu.transform.GetChild(index).name.StartsWith("IslandUI_", StringComparison.Ordinal))
            {
                index++;
            }
            button.transform.SetSiblingIndex(index);

            foreach (Component component in button.GetComponentsInChildren<Component>(true))
            {
                if (component != null && component.GetType().Name == "LocalizeStringEvent")
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }

            TMP_Text label = button.GetComponentInChildren<TMP_Text>(true);
            if (label != null)
            {
                label.text = "UPE";
                _font = label.font;
            }

            Button clickable = button.GetComponent<Button>();
            if (clickable != null)
            {
                clickable.onClick = new Button.ButtonClickedEvent();
                clickable.onClick.AddListener(Open);
            }

            CanvasGroup group = button.GetComponent<CanvasGroup>();
            if (group != null)
            {
                group.alpha = 1f;
                group.interactable = true;
                group.blocksRaycasts = true;
            }

            button.SetActive(true);
            _menuButton = button;
            UPEHFBase.Log.LogInfo("Settings: menu button 'UPE' cloned from '" + template.name + "' at sibling index " + button.transform.GetSiblingIndex() + ".");
        }

        private static void Open()
        {
            if (_mn == null || _mn.uiMN == null)
            {
                return;
            }
            GameObject menu = _mn.uiMN.settingMenu;
            if (menu != null && menu.activeSelf)
            {
                _mn.uiMN.SettingPanelOpen();
            }
            if (!EnsureWindow())
            {
                return;
            }
            foreach (Action refresh in Refreshers)
            {
                refresh();
            }
            _root.SetActive(true);
            _root.transform.SetAsLastSibling();
        }

        private static void Close()
        {
            if (_root != null)
            {
                _root.SetActive(false);
            }
        }

        private static bool EnsureWindow()
        {
            if (_root != null)
            {
                return true;
            }
            if (_mn == null || _mn.uiMN == null || _mn.uiMN.settingMenu == null)
            {
                return false;
            }
            Canvas canvas = _mn.uiMN.settingMenu.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                return false;
            }

            Tabs.Clear();
            Refreshers.Clear();

            _root = NewObject(WindowName, canvas.rootCanvas.transform);
            Stretch(_root.GetComponent<RectTransform>(), 0f, 0f, 0f, 0f);
            AddImage(_root, new Color(0f, 0f, 0f, 0.5f));

            GameObject windowGo = NewObject("Window", _root.transform);
            SetRect(windowGo.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(640f, 420f));
            AddImage(windowGo, Parchment);
            RectTransform frame = windowGo.GetComponent<RectTransform>();

            TextMeshProUGUI title = AddText(frame, "UPE Settings", 20f, true, TextAlignmentOptions.Center);
            SetRect(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -14f), new Vector2(500f, 34f));

            GameObject closeButton = MakeButton(frame, "X", 14f, TabIdle);
            closeButton.GetComponent<Button>().onClick.AddListener(Close);
            SetRect(closeButton.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-12f, -12f), new Vector2(32f, 32f));

            Tab general = NewTab(frame, "General", 0);
            Tab childStats = NewTab(frame, "Child stats", 1);
            Tab mutations = NewTab(frame, "Mutations", 2);
            Tab debug = NewTab(frame, "Debug", 3);
            BuildRows(general, childStats, mutations, debug);

            GameObject done = MakeButton(frame, "Done", 15f, TabActive);
            done.GetComponent<Button>().onClick.AddListener(Close);
            SetRect(done.GetComponent<RectTransform>(), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-30f, 26f), new Vector2(140f, 36f));

            TextMeshProUGUI note = AddText(frame, "Changes apply immediately and are saved to the config file.", 11f);
            SetRect(note.rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(20f, 16f), new Vector2(380f, 36f));

            GameObject grip = NewObject("ResizeGrip", frame);
            AddImage(grip, TabActive);
            grip.AddComponent<ResizeGrip>().Target = frame;
            SetRect(grip.GetComponent<RectTransform>(), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-3f, 3f), new Vector2(16f, 16f));

            ShowTab(0);
            _root.SetActive(false);
            return true;
        }

        private static void ShowTab(int index)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Scroll.SetActive(i == index);
                Tabs[i].Image.color = i == index ? TabActive : TabIdle;
            }
        }

        private static Tab NewTab(RectTransform frame, string name, int index)
        {
            Tab tab = new Tab();

            GameObject button = MakeButton(frame, name, 15f, TabIdle);
            tab.Image = button.GetComponent<Image>();
            int tabIndex = index;
            button.GetComponent<Button>().onClick.AddListener(() => ShowTab(tabIndex));
            SetRect(button.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -(60f + index * 52f)), new Vector2(160f, 44f));

            GameObject scroll = NewObject("Scroll", frame);
            AddImage(scroll, Color.clear);
            Stretch(scroll.GetComponent<RectTransform>(), 196f, 60f, 20f, 60f);
            ScrollRect scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            GameObject viewport = NewObject("Viewport", scroll.transform);
            Stretch(viewport.GetComponent<RectTransform>(), 0f, 0f, 16f, 0f);
            viewport.AddComponent<RectMask2D>();
            AddImage(viewport, Color.clear);

            GameObject bar = NewObject("Scrollbar", scroll.transform);
            AddImage(bar, new Color(0.27f, 0.18f, 0.10f, 0.2f));
            RectTransform barRect = bar.GetComponent<RectTransform>();
            barRect.anchorMin = new Vector2(1f, 0f);
            barRect.anchorMax = new Vector2(1f, 1f);
            barRect.pivot = new Vector2(1f, 0.5f);
            barRect.anchoredPosition = Vector2.zero;
            barRect.sizeDelta = new Vector2(12f, 0f);
            GameObject handle = NewObject("Handle", bar.transform);
            Image handleImage = AddImage(handle, TabActive);
            RectTransform handleRect = handle.GetComponent<RectTransform>();
            Stretch(handleRect, 2f, 2f, 2f, 2f);
            Scrollbar scrollbar = bar.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollbar.targetGraphic = handleImage;
            scrollbar.handleRect = handleRect;
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

            GameObject content = NewObject("Content", viewport.transform);
            RectTransform contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;
            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.spacing = 6f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRect.viewport = viewport.GetComponent<RectTransform>();
            scrollRect.content = contentRect;

            tab.Scroll = scroll;
            tab.Content = contentRect;
            Tabs.Add(tab);
            return tab;
        }

        private static void BuildRows(Tab general, Tab childStats, Tab mutations, Tab debug)
        {
            Config c = Config.Instance;

            AddBool(general, c.AllowAllPerfume);
            AddBool(general, c.PregnancyMedicineEnabled);
            /*AddString(general, c.PregnancyMedicineItemKey);
            AddString(general, c.PregnancyMedicineNpcIds);
            AddBool(general, c.BirthChartEnabled);
            AddBool(general, c.OrderSexEnabled);
            AddString(general, c.OrderSexHotkey);
            AddBool(general, c.OrderSexAllowDaySleep);
            AddBool(general, c.OrderSexBypassAllPerformerChecks);

            AddBool(childStats, c.ChildStatsEnabled);
            AddBool(childStats, c.RecalculateChildOnly);
            AddFloat(childStats, c.ChildMalePercent, 0f, 100f);
            AddFloat(childStats, c.ChildGenderVariancePercent, 0f, 100f);
            AddFloat(childStats, c.AppearanceMotherLeanPercent, 0f, 100f);
            AddFloat(childStats, c.AppearanceLeanVariancePercent, 0f, 100f);
            AddBool(childStats, c.InheritAppearance);
            AddBool(childStats, c.InheritVitals);
            AddBool(childStats, c.InheritAttackSpeed);
            AddBool(childStats, c.InheritClassLevel);
            AddBool(childStats, c.InheritDerivedLevel);
            AddBool(childStats, c.InheritTraits);
            AddFloat(childStats, c.TraitMotherChancePercent, 0f, 100f);
            AddFloat(childStats, c.TraitOffsetPercent, -100f, 100f);
            AddFloat(childStats, c.TraitVariancePercent, 0f, 100f);
            AddFloat(childStats, c.VariancePercent, 0f, 100f);

            AddBool(mutations, c.MutationsEnabled);
            AddBool(mutations, c.MutationStacking);
            AddFloat(mutations, c.RandomMutationChancePercent, 0f, 100f);
            AddFloat(mutations, c.RandomMutationBuffNerfPercent, 0f, 100f);
            AddFloat(mutations, c.Mutation86FullSiblingPercent, 0f, 100f);
            AddFloat(mutations, c.Mutation86HalfSiblingPercent, 0f, 100f);
            AddFloat(mutations, c.Mutation29ParentChildPercent, 0f, 100f);
            AddFloat(mutations, c.Mutation29FullSiblingPercent, 0f, 100f);
            AddFloat(mutations, c.Mutation29HalfSiblingPercent, 0f, 100f);
            AddFloat(mutations, c.MissingPartParentChildPercent, 0f, 100f);
            AddFloat(mutations, c.MissingPartFullSiblingPercent, 0f, 100f);
            AddFloat(mutations, c.MissingPartHalfSiblingPercent, 0f, 100f);

            AddBool(debug, c.DebugChildLogTable);*/
        }



        private static void AddBool(Tab tab, ConfigEntry<bool> entry)
        {
            RectTransform row = NewRow(tab, entry.Definition.Key, entry.Description.Description);
            GameObject button = MakeButton(row, "", 13f, FieldBg);
            SetRect(button.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-10f, -6f), new Vector2(90f, 24f));
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            Action refresh = () => text.text = entry.Value ? "On" : "Off";
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                entry.Value = !entry.Value;
                refresh();
            });
            Refreshers.Add(refresh);
        }

        private static void AddFloat(Tab tab, ConfigEntry<float> entry, float min, float max)
        {
            RectTransform row = NewRow(tab, entry.Definition.Key, entry.Description.Description);
            TMP_InputField field = MakeField(row, TMP_InputField.ContentType.DecimalNumber);
            SetRect(field.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-10f, -6f), new Vector2(120f, 24f));
            Action refresh = () => field.text = entry.Value.ToString("0.###", CultureInfo.InvariantCulture);
            field.onEndEdit.AddListener(s =>
            {
                if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                {
                    entry.Value = Mathf.Clamp(value, min, max);
                }
                refresh();
            });
            Refreshers.Add(refresh);
        }

        private static void AddString(Tab tab, ConfigEntry<string> entry)
        {
            RectTransform row = NewRow(tab, entry.Definition.Key, entry.Description.Description);
            TMP_InputField field = MakeField(row, TMP_InputField.ContentType.Standard);
            SetRect(field.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-10f, -6f), new Vector2(260f, 24f));
            Action refresh = () => field.text = entry.Value;
            field.onEndEdit.AddListener(s =>
            {
                entry.Value = s.Trim();
                refresh();
            });
            Refreshers.Add(refresh);
        }

        private static RectTransform NewRow(Tab tab, string key, string description)
        {
            GameObject row = NewObject("Row", tab.Content);
            LayoutElement layout = row.AddComponent<LayoutElement>();
            layout.preferredHeight = RowHeight;
            layout.minHeight = RowHeight;
            AddImage(row, RowBg).raycastTarget = false;

            TextMeshProUGUI keyText = AddText(row.transform, key, 14f, true);
            SetRect(keyText.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(10f, -6f), new Vector2(280f, 22f));

            TextMeshProUGUI descText = AddText(row.transform, description, 10.5f);
            RectTransform descRect = descText.rectTransform;
            descRect.anchorMin = Vector2.zero;
            descRect.anchorMax = new Vector2(1f, 0f);
            descRect.offsetMin = new Vector2(10f, 4f);
            descRect.offsetMax = new Vector2(-10f, 42f);

            return row.GetComponent<RectTransform>();
        }

        private static TMP_InputField MakeField(Transform parent, TMP_InputField.ContentType type)
        {
            GameObject go = NewObject("Field", parent);
            Image background = AddImage(go, FieldBg);
            TMP_InputField field = go.AddComponent<TMP_InputField>();
            field.targetGraphic = background;

            GameObject area = NewObject("TextArea", go.transform);
            Stretch(area.GetComponent<RectTransform>(), 6f, 2f, 6f, 2f);
            area.AddComponent<RectMask2D>();
            TextMeshProUGUI text = AddText(area.transform, "", 13f);
            Stretch(text.rectTransform, 0f, 0f, 0f, 0f);

            field.textViewport = area.GetComponent<RectTransform>();
            field.textComponent = text;
            field.contentType = type;
            return field;
        }

        private static GameObject MakeButton(Transform parent, string label, float size, Color color)
        {
            GameObject go = NewObject("Button", parent);
            Image image = AddImage(go, color);
            Button button = go.AddComponent<Button>();
            button.targetGraphic = image;
            TextMeshProUGUI text = AddText(go.transform, label, size, true, TextAlignmentOptions.Center);
            Stretch(text.rectTransform, 0f, 0f, 0f, 0f);
            return go;
        }

        private static TextMeshProUGUI AddText(Transform parent, string value, float size, bool bold = false, TextAlignmentOptions align = TextAlignmentOptions.MidlineLeft)
        {
            GameObject go = NewObject("Text", parent);
            TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
            if (_font != null)
            {
                text.font = _font;
            }
            text.text = value;
            text.fontSize = size;
            text.color = Ink;
            text.alignment = align;
            text.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
            text.enableWordWrapping = true;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.raycastTarget = false;
            return text;
        }

        private static Image AddImage(GameObject go, Color color)
        {
            Image image = go.AddComponent<Image>();
            image.color = color;
            return image;
        }

        private static GameObject NewObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect, float left, float top, float right, float bottom)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(-right, -top);
        }
    }

    internal class ResizeGrip : MonoBehaviour, IDragHandler
    {
        private static readonly Vector2 MinSize = new Vector2(600f, 360f);
        public RectTransform Target;

        public void OnDrag(PointerEventData eventData)
        {
            Canvas canvas = Target.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                return;
            }
            Canvas root = canvas.rootCanvas;
            float scale = root.scaleFactor > 0f ? root.scaleFactor : 1f;
            Vector2 limit = (root.transform as RectTransform).rect.size - new Vector2(20f, 20f);

            Vector2 current = Target.sizeDelta;
            Vector2 wanted = current + new Vector2(eventData.delta.x, -eventData.delta.y) / scale;
            wanted.x = Mathf.Clamp(wanted.x, MinSize.x, Mathf.Max(MinSize.x, limit.x));
            wanted.y = Mathf.Clamp(wanted.y, MinSize.y, Mathf.Max(MinSize.y, limit.y));

            Vector2 change = wanted - current;
            Target.sizeDelta = wanted;
            Target.anchoredPosition += new Vector2(change.x, -change.y) * 0.5f;
        }
    }
}
