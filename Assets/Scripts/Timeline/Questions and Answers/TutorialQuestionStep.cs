using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TutorialQuestionStep", menuName = "Scriptable Objects/Timeline/Tutorial Question Step")]
public class TutorialQuestionStep : QuestionStep
{
    [SerializeField] private List<TutorialData> tutorialSteps = new();
    public override void Begin(GameSession session, Action onComplete)
    {
        this.session = session;
        this.onComplete = onComplete;
        //ui show question, and callback to this
        session.ui.ShowQuestion(this, OnAnswerSelected);
        TutorialView.Instance.StartTutorial(tutorialSteps, () =>
        {
            //didnt even need this shi
        });
    }

    protected override void ApprovalRecordAndChange(AnswerOption answer)
    {
        //empty so it dont do anything
    }
}
