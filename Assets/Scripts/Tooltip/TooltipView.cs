using TMPro;
using UnityEngine;

public class TooltipView : MonoBehaviour
{
    public static TooltipView Instance;

    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_Text definitionText;
    [SerializeField] private Vector2 offset = new(15f,15f);

    private RectTransform canvasRect;
    private Canvas parentCanvas;

    private void Awake()
    {
        Instance = this;
        parentCanvas = GetComponentInParent<Canvas>();
        canvasRect = parentCanvas.GetComponent<RectTransform>();
        panel.gameObject.SetActive(false);
    }

    public void Show(string definition, Vector2 screenPosition)
    {
        definitionText.text = definition;
        panel.gameObject.SetActive(true);
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
        panel.anchoredPosition = localPoint + offset;
    }
}
