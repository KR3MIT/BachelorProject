using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialPosition;
    private RectTransform rectTransform;
    public bool isActive = true;
    public Button button { get; private set; }

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        initialPosition = rectTransform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive) return;
        rectTransform.DOMoveY(rectTransform.position.y + 10f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive) return;
        rectTransform.DOMoveY(initialPosition.y, 0.2f).SetEase(Ease.OutQuad);
    }
}
