using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// overall manager
/// </summary>
public class GameManager : MonoBehaviour, IGameUI
{
    [Header("UI")]
    [SerializeField] private MainMenuScript mainMenu;
    [SerializeField] private ViewpointView viewpointView;
    [SerializeField] private QuestionView questionView;
    [SerializeField] private ExplanationView explanationView;
    [SerializeField] private EndView endView;
    [SerializeField] private GameObject[] popUpObjects;

    [Header("Data")]
    [SerializeField] private TimelineDefinition timeline;

    private GameSession session;
    private int currentStepIndex;

    public void Start()
    {
        ShowMenu(StartPreGame);//showmenu, callback to startpregame, when clicked start button
        foreach (var obj in popUpObjects) obj.SetActive(false);
    }

    public async void StartPreGame()
    {
       
        //menu stuff
        mainMenu.Hide();
        await mainMenu.MoveCameraToGame();
        foreach (var obj in popUpObjects) obj.SetActive(true);
        //viewpoint stuff
        Debug.Log("Starting pre-game setup...");
        viewpointView.gameObject.SetActive(true);
        UIApprovalRating.Instance.gameObject.SetActive(true);
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

    private void ResetGame()
    {
        session = null;
        currentStepIndex = 0;
        ShowMenu(StartPreGame);
    }

    #region steps
    private void RunCurrentStep()
    {
        if (currentStepIndex >= timeline.steps.Count)
        {
            Debug.Log("Game Finished! Approval rating: " + UIApprovalRating.Instance.GetApprovalRating() * 100f + "/100");

            ShowEndScreen(session.GetQuestionSelectionCounts(), UIApprovalRating.Instance.GetApprovalRating(), ResetGame);

            //endView.Show(session.GetQuestionSelectionCounts(), UIApprovalRating.Instance.GetApprovalRating());
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
    public void ShowMenu(Action onStartGame)
    {
        mainMenu.Show(onStartGame);
    }
    public void ShowEndScreen(Dictionary<ViewpointType, int> counts, float approvalRating, Action onEnd)
    {
        //its in explanation view gg
        explanationView.ShowEndScreen(counts, approvalRating, onEnd);
    }

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
