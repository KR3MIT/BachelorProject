using UnityEngine;
using System;
/// <summary>
/// overall manager
/// </summary>
public class GameManager : MonoBehaviour, IGameUI
{
    [Header("UI")]
    [SerializeField] private ViewpointView viewpointView;
    [SerializeField] private QuestionView questionView;
    [SerializeField] private ExplanationView explanationView;
    [SerializeField] private EndView endView;
    [SerializeField] private UIApprovalRating approvalRating;

    [Header("Data")]
    [SerializeField] private TimelineDefinition timeline;

    private GameSession session;
    private int currentStepIndex;

    public void StartPreGame()
    {
        viewpointView.gameObject.SetActive(true);
        approvalRating.gameObject.SetActive(true);
    }

    public void StartGame(int viewpointID)
    {
        ViewpointType viewpoint = (ViewpointType)viewpointID;

        //replace with selection screen later
        session = new GameSession(viewpoint, this, timeline);

        currentStepIndex = 0;
        explanationView.Hide();
        RunCurrentStep();
    }


    #region steps
    private void RunCurrentStep()
    {
        if (currentStepIndex >= timeline.steps.Count)
        {
            Debug.Log("Game Finished! Approval rating: " + UIApprovalRating.Instance.GetApprovalRating() * 100f + "/100");

            endView.Show(session.GetQuestionSelectionCounts(), UIApprovalRating.Instance.GetApprovalRating());
            UIApprovalRating.Instance.gameObject.SetActive(false);

            return;
        }

        timeline.steps[currentStepIndex].Begin(session, OnStepComplete);
    }

    private void OnStepComplete()
    {
        currentStepIndex++;
        RunCurrentStep();
    }
    #endregion

    #region IGameUI

    void IGameUI.ShowQuestion(QuestionStep question, Action<int> onAnswerSelected)
    {
        explanationView.Hide();
        questionView.Show(question, onAnswerSelected);
    }

    void IGameUI.ShowExplanation(string text, bool wasCorrect, Action onContinue)
    {
        explanationView.Show(text, wasCorrect, onContinue);
    }
    #endregion
}
