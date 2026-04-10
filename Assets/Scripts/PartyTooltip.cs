using UnityEngine;
using UnityEngine.UI;

public class PartyTooltip : MonoBehaviour
{

    [SerializeField] private GameObject[] tooltipObjects;
    [SerializeField] private Sprite[] partyLogoSprites;
    private Image backgroundImage;
    private Sprite sessionPartySprite;
    [SerializeField] private Sprite exitSprite;
    [SerializeField] private GameObject tooltipButton;
    [SerializeField] private QuestionView questionView;
    private GameObject sessionTooltip;
    private bool isTooltipShowing = false;

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
        sessionTooltip = tooltipObjects[viewPointID];
        sessionPartySprite = partyLogoSprites[viewPointID];
        tooltipButton.GetComponent<Image>().sprite = sessionPartySprite;
    }

    public void ChangeTooltipVisibility()
    {
        if (isTooltipShowing == false)
        {
            ShowPartyTooltip();
        } else
        {
            HidePartyTooltip();
        }
    }

    public void ShowPartyTooltip()
    {
        sessionTooltip.SetActive(true);
        tooltipButton.GetComponent<Image>().sprite = exitSprite;
        isTooltipShowing = true;
        var questionTexts = questionView.GetComponentsInChildren<TMPTextFormatter>();
        foreach (var questionText in questionTexts)
        {
            questionText.tooltipEnabled = false;
        }
    }

    public void HidePartyTooltip()
    {
        sessionTooltip.SetActive(false);
        tooltipButton.GetComponent<Image>().sprite = sessionPartySprite;
        isTooltipShowing = false;
        var questionTexts = questionView.GetComponentsInChildren<TMPTextFormatter>();
        foreach (var questionText in questionTexts)
        {
            questionText.tooltipEnabled = true;
        }
    }

}
