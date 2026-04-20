using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupBuilder : MonoBehaviour
{
    [Header("Popup References")]
    [SerializeField] private GameObject settingsPopup;

    [ContextMenu("Build Settings Popup")]
    public void BuildSettingsPopup()
    {
        if (settingsPopup == null)
        {
            Debug.LogError("Settings Popup reference is missing!");
            return;
        }

        // Clear existing children
        for (int i = settingsPopup.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(settingsPopup.transform.GetChild(i).gameObject);

        // Overlay
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(settingsPopup.transform, false);
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0, 0, 0, 0.6f);
        RectTransform overlayRT = overlay.GetComponent<RectTransform>();
        overlayRT.anchorMin = Vector2.zero;
        overlayRT.anchorMax = Vector2.one;
        overlayRT.sizeDelta = Vector2.zero;

        // Panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(settingsPopup.transform, false);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.2f, 1f);
        RectTransform panelRT = panel.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0.5f, 0.5f);
        panelRT.anchorMax = new Vector2(0.5f, 0.5f);
        panelRT.sizeDelta = new Vector2(900, 1100);
        panelRT.anchoredPosition = Vector2.zero;

        // Title
        GameObject title = new GameObject("Title");
        title.transform.SetParent(panel.transform, false);
        TextMeshProUGUI titleText = title.AddComponent<TextMeshProUGUI>();
        titleText.text = "Settings";
        titleText.fontSize = 80;
        titleText.alignment = TextAlignmentOptions.Center;
        RectTransform titleRT = title.GetComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0.5f, 0.5f);
        titleRT.anchorMax = new Vector2(0.5f, 0.5f);
        titleRT.sizeDelta = new Vector2(800, 120);
        titleRT.anchoredPosition = new Vector2(0, 400);

        // Settings Container
        GameObject container = new GameObject("SettingsContainer");
        container.transform.SetParent(panel.transform, false);
        VerticalLayoutGroup vlg = container.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 60;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        RectTransform containerRT = container.GetComponent<RectTransform>();
        containerRT.anchorMin = new Vector2(0.5f, 0.5f);
        containerRT.anchorMax = new Vector2(0.5f, 0.5f);
        containerRT.sizeDelta = new Vector2(800, 400);
        containerRT.anchoredPosition = Vector2.zero;

        // BGM Row
        CreateToggleRow(container.transform, "BGMRow", "Background Music", "BGMToggle");

        // SFX Row
        CreateToggleRow(container.transform, "SFXRow", "Sound Effects", "SFXToggle");

        // Close Button
        GameObject closeBtn = new GameObject("CloseButton");
        closeBtn.transform.SetParent(panel.transform, false);
        Image closeBtnImage = closeBtn.AddComponent<Image>();
        closeBtnImage.color = new Color(0.8f, 0.2f, 0.2f, 1f);
        Button closeBtnComp = closeBtn.AddComponent<Button>();
        RectTransform closeBtnRT = closeBtn.GetComponent<RectTransform>();
        closeBtnRT.anchorMin = new Vector2(0.5f, 0.5f);
        closeBtnRT.anchorMax = new Vector2(0.5f, 0.5f);
        closeBtnRT.sizeDelta = new Vector2(120, 120);
        closeBtnRT.anchoredPosition = new Vector2(380, 480);

        GameObject closeTxt = new GameObject("Text");
        closeTxt.transform.SetParent(closeBtn.transform, false);
        TextMeshProUGUI closeTxtComp = closeTxt.AddComponent<TextMeshProUGUI>();
        closeTxtComp.text = "X";
        closeTxtComp.fontSize = 60;
        closeTxtComp.alignment = TextAlignmentOptions.Center;
        RectTransform closeTxtRT = closeTxt.GetComponent<RectTransform>();
        closeTxtRT.anchorMin = Vector2.zero;
        closeTxtRT.anchorMax = Vector2.one;
        closeTxtRT.sizeDelta = Vector2.zero;

        settingsPopup.SetActive(false);
        Debug.Log("Settings Popup built successfully");
    }

    private void CreateToggleRow(Transform parent, string rowName, string labelText, string toggleName)
    {
        GameObject row = new GameObject(rowName);
        row.transform.SetParent(parent, false);
        HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 40;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        RectTransform rowRT = row.GetComponent<RectTransform>();
        rowRT.sizeDelta = new Vector2(800, 120);

        GameObject label = new GameObject("Label");
        label.transform.SetParent(row.transform, false);
        TextMeshProUGUI labelTMP = label.AddComponent<TextMeshProUGUI>();
        labelTMP.text = labelText;
        labelTMP.fontSize = 55;
        labelTMP.alignment = TextAlignmentOptions.MidlineLeft;
        RectTransform labelRT = label.GetComponent<RectTransform>();
        labelRT.sizeDelta = new Vector2(550, 100);

        GameObject toggleObj = new GameObject(toggleName);
        toggleObj.transform.SetParent(row.transform, false);
        Toggle toggle = toggleObj.AddComponent<Toggle>();
        toggle.isOn = true;
        RectTransform toggleRT = toggleObj.GetComponent<RectTransform>();
        toggleRT.sizeDelta = new Vector2(160, 100);
    }
}