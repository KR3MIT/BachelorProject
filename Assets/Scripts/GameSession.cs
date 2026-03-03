using UnityEngine;
/// <summary>
/// runtime class for a run, advances the questions
/// </summary>
public class GameSession
{
    public QuestionDefinition.ViewpointType sessionViewpoint { get; private set; }
    public int approvalRating { get; private set; }

    public TimelineDefinition timeline { get; private set; }
    public int currentQuestion { get; private set; }
    public bool isFinished;

    public GameSession(TimelineDefinition timeline)
    {
        this.timeline = timeline;
        currentQuestion = 0;
    }

    public void AnswerQuestion(int answerIndex)
    {
        var answers = timeline.questions[currentQuestion].answers;
        var answer = answers[answerIndex];

        if(answer.viewpoint == sessionViewpoint)
        {
            approvalRating += 10;
        }
        else
        {
            approvalRating -= 10;
        }

        if (timeline.questions[currentQuestion++] == null)
        {
            isFinished = true;
            return;
        }
        else
        {
            currentQuestion++;
        }
    }
}
