using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for explanations
/// </summary>
public class ExplanationView : MonoBehaviour
{
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private Button continueButton;

    private Action onContinue;

    public async void Show(string explanation, bool wasCorrect, Action onContinue)
    {
        this.onContinue = onContinue;

        explanationText.GetComponent<TMPTextFormatter>().SetText(explanation);
        resultLabel.text = wasCorrect ? "Godt arbejde!" : "Hvad tænkte du på?";

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);

        await TabletController.Instance.MoveOnScreen();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        TutorialView.Instance.EndTutorial();
        TabletController.Instance.MoveOffScreen();
        gameObject.SetActive(false);
    }

    private void OnContinueClicked()
    {
        Hide();
        onContinue?.Invoke();
    }
}
