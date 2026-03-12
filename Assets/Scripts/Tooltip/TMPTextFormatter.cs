using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// Put on tmptext that should be tooltipable
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TMPTextFormatter : MonoBehaviour
{
    [SerializeField] private TooltipDefinition tooltipDefinition;

    private TMP_Text tmpText;
    private Camera uiCamera;
    private int hoveredLinkIndex = -1;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();

        if (tmpText is TextMeshProUGUI)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            uiCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? canvas.worldCamera
                : null;
        }
        else
        {
            uiCamera = Camera.main;
        }
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(tmpText.text))
            tmpText.text = FormatWithTooltipLinks(tmpText.text, tooltipDefinition);
    }

    private void LateUpdate()
    {
        if (tooltipDefinition == null || TooltipView.Instance == null) return;

        bool isOverText = TMP_TextUtilities.IsIntersectingRectTransform(
            tmpText.rectTransform, Input.mousePosition, uiCamera);

        bool isOverTooltipPanel = TMP_TextUtilities.IsIntersectingRectTransform(
            TooltipView.Instance.panel, Input.mousePosition, uiCamera) &&
            TooltipView.Instance.gameObject.activeSelf;

        Debug.Log($"Mouse over text: {isOverText}, Mouse over tooltip: {isOverTooltipPanel}");

        if (isOverTooltipPanel) { return; }

        if (!isOverText)
        {
            HideIfNeeded();
            return;
        }

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            tmpText, Input.mousePosition, uiCamera);

        if (linkIndex == hoveredLinkIndex) return;

        hoveredLinkIndex = linkIndex;

        if (linkIndex == -1)
        {
            TooltipView.Instance.Hide();
            return;
        }

        TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
        string keyword = linkInfo.GetLinkID();

        if (tooltipDefinition.TryGetDefinition(keyword, out string definition, out Sprite image))
        {
            var capitalizedKeyword = char.ToUpper(keyword[0]) + keyword.Substring(1);
            TooltipView.Instance.Show(capitalizedKeyword, definition, image, Input.mousePosition);
        }
        else
            TooltipView.Instance.Hide();
    }

    public void SetText(string rawText)
    {
        tmpText.text = FormatWithTooltipLinks(rawText, tooltipDefinition);
    }


    public static string FormatWithTooltipLinks(string text, TooltipDefinition definition)
    {
        if (definition == null) return text;

        foreach (var entry in definition.entries)
        {
            if (string.IsNullOrEmpty(entry.keyword)) continue;

            string escaped = Regex.Escape(entry.keyword);

            text = Regex.Replace(
                text,
                $@"\b{escaped}\b",
                match => $"<link=\"{entry.keyword}\"><u>{match.Value}</u></link>",
                RegexOptions.IgnoreCase
            );
        }

        return text;
    }

    private void HideIfNeeded()
    {
        if (hoveredLinkIndex == -1) return;
        hoveredLinkIndex = -1;
        TooltipView.Instance.Hide();
    }
}
