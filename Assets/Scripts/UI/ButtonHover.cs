using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialPosition;
    private RectTransform rectTransform;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOMoveY(rectTransform.position.y + 10f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOMoveY(initialPosition.y, 0.2f).SetEase(Ease.OutQuad);
    }
}
