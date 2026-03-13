using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipView : MonoBehaviour
{
    public static TooltipView Instance;

    public RectTransform panel {  get; private set; }
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text definitionText;
    [SerializeField] private Image imageImage;
    [SerializeField] private Vector2 offset = new(15f,15f);

    private RectTransform canvasRect;
    private Canvas parentCanvas;
    private LayoutElement layoutElement;
    public int characterWrapLimit = 80;

    private void Awake()
    {
        Instance = this;
        parentCanvas = GetComponentInParent<Canvas>();
        canvasRect = parentCanvas.GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        panel = gameObject.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void Show(string header, string definition, Sprite image, Vector2 screenPosition)
    {
        headerText.text = header;
        definitionText.text = definition;
        imageImage.sprite = image;

        int headerLength = header.Length;
        int definitionLength = definition.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || definitionLength > characterWrapLimit;

        panel.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel);

        PositionAt(screenPosition);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    private void PositionAt(Vector2 screenPosition)
    {
        Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, cam, out Vector2 localPoint);

        //place at left bottom corner
        Vector2 tooltipSize = panel.rect.size;
        Vector2 pivotOffset = new Vector2(panel.pivot.x * tooltipSize.x, panel.pivot.y * tooltipSize.y);

        panel.anchoredPosition = localPoint + pivotOffset + offset;
    }
}
