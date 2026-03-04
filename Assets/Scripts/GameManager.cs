using UnityEngine;
using System;
/// <summary>
/// overall manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private QuestionView questionView;
    [SerializeField] private ExplanationView explanationView;

    [Header("Data")]
    [SerializeField] private TimelineDefinition timeline;

    private GameSession session;
    private int currentStepIndex;

    public void Start()
    {
        //replace with selection screen later
        session = new GameSession(ViewpointType.Realist);

        //ui callbacks
        session.ShowQuestion = OnShowQuestion;
        session.ShowExplanation = OnShowExplanation;

        currentStepIndex = 0;
        explanationView.Hide();
        RunCurrentStep();
    }

    private void RunCurrentStep()
    {
        if (currentStepIndex >= timeline.steps.Count)
        {
            Debug.Log("Game Finished! Approval rating: " + session.approvalRating);

            //end screen show here

            return;
        }

        var step = timeline.steps[currentStepIndex];
        step.Begin(session, OnStepComplete);
    }

    private void OnStepComplete()
    {
        currentStepIndex++;
        RunCurrentStep();
    }

    #region UI methods

    private void OnShowQuestion(QuestionStep question, Action<int> onAnswerSelected)
    {
        explanationView.Hide();
        questionView.gameObject.SetActive(true);

        //unsubscribe previous listener, then subscribe new
        questionView.answerSelected -= HandleAnswer;
        currentAnswerCallback = onAnswerSelected;
        questionView.answerSelected += HandleAnswer;
        questionView.Show(question);
    }

    private Action<int> currentAnswerCallback;

    private void HandleAnswer(int index)
    {
        questionView.answerSelected -= HandleAnswer;
        questionView.gameObject.SetActive(false);
        currentAnswerCallback?.Invoke(index);
    }

    private void OnShowExplanation(string text, bool wasCorrect, Action onContinue)
    {
        explanationView.Show(text, wasCorrect, onContinue);
    }
    #endregion
}
