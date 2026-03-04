using System;
using UnityEngine;
/// <summary>
/// runtime class for a run, advances the questions
/// </summary>
public class GameSession
{
    public ViewpointType viewpoint { get; private set; }
    public int approvalRating { get; private set; }

    //callbacks for ui and stuff
    public Action<QuestionStep, Action<int>> ShowQuestion;
    public Action<string, bool, Action> ShowExplanation;

    public GameSession(ViewpointType viewpoint)
    {
        this.viewpoint = viewpoint;
        approvalRating = 0;
    }

    public void AddApproval(int amount)
    {
        approvalRating += amount;
    }

    //public void AnswerQuestion(int answerIndex)
    //{
    //    var answers = timeline.questions[currentQuestion].answers;
    //    var answer = answers[answerIndex];

    //    if(answer.viewpoint == sessionViewpoint)
    //    {
    //        approvalRating += 10;
    //    }
    //    else
    //    {
    //        approvalRating -= 10;
    //    }

    //    if (timeline.questions[currentQuestion++] == null)
    //    {
    //        isFinished = true;
    //        return;
    //    }
    //}
}
