using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct EndScreenMessage
{
    public ViewpointType viewpoint;
    [TextArea] public string badMessage;
    [TextArea] public string midMessage;
    [TextArea] public string goodMessage;
}

/// <summary>
/// UI for explanations, also used for end screen since its tied to the tablet
/// </summary>
public class ExplanationView : MonoBehaviour
{
    [Header("Shared")]
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text boss;
    [SerializeField] private TMP_Text mail;
    [SerializeField] private Image ideoImg;
    [SerializeField] private Sprite realSprite;
    [SerializeField] private Sprite libSprite;

    [Header("End Screen")]
    [Tooltip("Uses the index from viewpointtype")]
    public List<EndScreenMessage> endScreenMessages;

    private Action onContinue;
    private Action onEnd;

    public async void Show(string explanation, bool wasCorrect, Action onContinue, ViewpointType viewpoint)
    {
        this.onContinue = onContinue;

        explanationText.GetComponent<TMPTextFormatter>().SetText(explanation);

        SetPartyFlair(viewpoint);

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);

        await TabletController.Instance.MoveOnScreen();
    }

    public async void ShowEndScreen(Dictionary<ViewpointType, int> counts, ViewpointType viewpoint, Action onEnd)
    {
        this.onEnd = onEnd;

        continueButton.onClick.RemoveAllListeners();

        await TabletController.Instance.MoveOffScreen(true);

        continueButton.onClick.AddListener(OnEndClicked);



        //get the EndMessage for viewpoint and approval rating
        EndScreenMessage viewpointMessage = default;
        foreach (var message in endScreenMessages)
        {
            if (message.viewpoint == viewpoint)
            {
                viewpointMessage = message;
                break;
            }
        }

        var approval = UIApprovalRating.Instance.GetApprovalRating();
        if(approval < 0.33f)
        {
            explanationText.GetComponent<TMPTextFormatter>().SetText(viewpointMessage.badMessage);
        }
        else if(approval < 0.66f)
        {
            explanationText.GetComponent<TMPTextFormatter>().SetText(viewpointMessage.midMessage);
        }
        else
        {
            explanationText.GetComponent<TMPTextFormatter>().SetText(viewpointMessage.goodMessage);
        }

        //the number message
        var sb = new StringBuilder();
        sb.AppendLine("Spørgsmåls valg:");

        foreach (var kvp in counts)
        {
            if (kvp.Key == ViewpointType.MiddleRoad) 
            {
                sb.AppendLine($"Neutral: {kvp.Value}");
                continue;
            }  //skip middle road since it doesn't have a party

            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }

        var result = sb.ToString();
        resultLabel.text = result;
        resultLabel.gameObject.SetActive(true);


        continueButton.transform.GetComponentInChildren<TMP_Text>().text = "Afslut spil";


        await TabletController.Instance.MoveOnScreen();
    }

    private void SetPartyFlair(ViewpointType viewpoint)
    {
        boss.text = viewpoint == ViewpointType.Realist ? "Statsminister Jensen, Realisternes Parti" : "Statminister Jørgensen, Liberalisternes Parti";
        mail.text = viewpoint == ViewpointType.Realist ? "Realisternes Parti" : "Liberalisternes Parti";
        ideoImg.sprite = viewpoint == ViewpointType.Realist ? realSprite : libSprite;
    }

    public void Hide()
    {
        TabletController.Instance.MoveOffScreen();
    }

    private void OnContinueClicked()
    {
        Hide();
        TutorialView.Instance.EndTutorial();
        onContinue?.Invoke();
    }

    private void OnEndClicked()
    {
        Hide();
        onEnd?.Invoke();
    }
}
