using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for explanations, also used for end screen since its tied to the tablet
/// </summary>
public class ExplanationView : MonoBehaviour
{
    [Header("Shared")]
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private Button continueButton;

    [Header("End Screen")]
    [Tooltip("Uses the index from viewpointtype")]
    [TextArea]public List<string> endScreenMessages;

    private Action onContinue;
    private Action onEnd;

    public async void Show(string explanation, bool wasCorrect, Action onContinue)
    {
        this.onContinue = onContinue;

        explanationText.GetComponent<TMPTextFormatter>().SetText(explanation);
        resultLabel.text = wasCorrect ? "Godt arbejde!" : "Hvad tænkte du på?";

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);

        await TabletController.Instance.MoveOnScreen();
    }

    public async void ShowEndScreen(Dictionary<ViewpointType, int> counts, float approvalRating, Action onEnd)
    {
        this.onEnd = onEnd;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnEndClicked);

        //the talk message
        var topKvp = counts.OrderByDescending(kvp => kvp.Value).First();
        int messageIndex = (int)topKvp.Key;
        string message = endScreenMessages[messageIndex];
        explanationText.GetComponent<TMPTextFormatter>().SetText(message);

        //the number message
        var sb = new StringBuilder();
        sb.AppendLine("Spørgsmåls valg:");

        foreach (var kvp in counts)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }

        var result = sb.ToString();
        resultLabel.text = result;


        await TabletController.Instance.MoveOnScreen();
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
