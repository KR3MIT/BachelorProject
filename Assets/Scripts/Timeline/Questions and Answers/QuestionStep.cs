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
}

/// <summary>
/// A timeline step that contains question data and handles the question flow.
/// </summary>
[CreateAssetMenu(fileName = "QuestionStep", menuName = "Scriptable Objects/Timeline/Question Step")]
public class QuestionStep : TimelineStep
{
    [TextArea] public string question;
    public AnswerOption[] answers;

    private GameSession session;
    private Action onComplete;

    public override void Begin(GameSession session, Action onComplete)
    {
        this.session = session;
        this.onComplete = onComplete;

        session.ShowQuestion(this, OnAnswerSelected);
    }

    private void OnAnswerSelected(int answerIndex)
    {
        var answer = answers[answerIndex];

        if (answer.viewpoint == session.viewpoint)
        {
            session.AddApproval(10);
        }
        else
        {
            session.AddApproval(-10);
        }

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

        session.ShowExplanation(explanationText, wasCorrect, () =>
        {
            onComplete?.Invoke();
        });
    }
}