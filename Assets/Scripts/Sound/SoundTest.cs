using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SoundTest : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public SoundType clickSound;
    public SoundType hoverSound;

    private void Start()
    {
        // Find text component in children and set it
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = gameObject.name;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != SoundType.None) // or use a "None" enum value ideally
            AudioManager.Instance.Play(clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != 0)
            AudioManager.Instance.Play(hoverSound);
    }
}
