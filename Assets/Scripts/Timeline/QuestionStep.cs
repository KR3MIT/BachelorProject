using System;
using UnityEngine;

/// <summary>
///timeline step question, wait for answer, then shows explanation
/// </summary>
[CreateAssetMenu(fileName = "QuestionStep", menuName = "Scriptable Objects/Timeline/Question Step")]
public class QuestionStep : TimelineStep
{
    public QuestionDefinition question;

    private GameSession session;
    private Action onComplete;

    public override void Begin(GameSession session, Action onComplete)
    {
        this.session = session;
        this.onComplete = onComplete;

        session.ShowQuestion(question, OnAnswerSelected);
    }

    private void OnAnswerSelected(int answerIndex)
    {
        var answer = question.answers[answerIndex];

        if (answer.viewpoint == session.sessionViewpoint)
        {
            session.AddApproval(10);
        }
        else
        {
            session.AddApproval(-10);
        }

        //get explanation for answer
        Explanation explanation = null;
        foreach (var exp in answer.explanations)
        {
            if (exp.viewpoint == session.sessionViewpoint)
            {
                explanation = exp;
                break;
            }
        }

        string explanationText = explanation != null ? explanation.explanationText : "No explanation available.";

        bool wasCorrect = answer.viewpoint == session.sessionViewpoint;

        session.ShowExplanation(explanationText, wasCorrect, () =>
        {
            onComplete?.Invoke();
        });
    }
}