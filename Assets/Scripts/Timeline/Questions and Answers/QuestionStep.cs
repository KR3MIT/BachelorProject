using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///shared enum for all viewpoint types
/// </summary>
public enum ViewpointType
{
    Realist,
    Liberalist,
    MiddleRoad,
}

[Serializable]
public class AnswerOption
{
    [TextArea] public string answerText;
    public ViewpointType viewpoint;

    public List<Explanation> explanations;
}

[Serializable]
public class Explanation
{
    [TextArea] public string explanationText;
    public ViewpointType viewpoint;
    public Sprite image;
}

/// <summary>
/// A timeline step that contains question data and handles the question flow.
/// </summary>
[CreateAssetMenu(fileName = "QuestionStep", menuName = "Scriptable Objects/Timeline/Question Step")]
public class QuestionStep : TimelineStep
{
    [TextArea] public string question;
    public AnswerOption[] answers;

    protected GameSession session;
    protected Action onComplete;

    public override void Begin(GameSession session, Action onComplete)
    {
        this.session = session;
        this.onComplete = onComplete;

        //ui show question, and callback to this
        session.ui.ShowQuestion(this, OnAnswerSelected);
    }

    protected virtual void OnAnswerSelected(int answerIndex)
    {
        var answer = answers[answerIndex];

        ApprovalRecordAndChange(answer);//a method so it can be overriden

        Explanation explanation = null;
        foreach (var exp in answer.explanations)
        {
            if (exp.viewpoint == session.viewpoint)
            {
                explanation = exp;
                break;
            }
        }

        string explanationText = explanation != null
            ? explanation.explanationText
            : "No explanation available.";

        bool wasCorrect = answer.viewpoint == session.viewpoint;

        session.ui.ShowExplanation(explanationText, explanation.image, wasCorrect, () => onComplete?.Invoke());
    }

    protected virtual void ApprovalRecordAndChange(AnswerOption answer)
    {
        session.RecordQuestionSelection(answer.viewpoint);
        session.ChangeApproval(answer.viewpoint == session.viewpoint ? ApprovalChangeType.Add : answer.viewpoint == ViewpointType.MiddleRoad ? ApprovalChangeType.SmallRemove : ApprovalChangeType.Remove);

    }
}