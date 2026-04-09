using UnityEngine;
using UnityEngine.UI;

public class PartyTooltip : MonoBehaviour
{

    [SerializeField] private GameObject[] tooltipObjects;
    [SerializeField] private Sprite[] partyLogoSprites;
    private Image backgroundImage;
    [SerializeField] private GameObject tooltipButton;
    [SerializeField] private GameSession session;

    void Start()
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.enabled = false;
        tooltipButton.SetActive(false);
    }

    public void EnablePartyTooltip(int viewPointID)
    {
        backgroundImage.enabled = true;
        tooltipButton.SetActive(true);
        switch (viewPointID)
        {
            case 0:
                tooltipButton.GetComponent<Image>().sprite = partyLogoSprites[0];
                break;
            case 1:
                tooltipButton.GetComponent<Image>().sprite = partyLogoSprites[1];
                break;
        }

    }

}
