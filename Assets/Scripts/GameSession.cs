using UnityEngine;
/// <summary>
/// runtime class for a run, advances the questions
/// </summary>
public class GameSession
{
    //public Viewpoint viewpoint???
    public QuestionDefinition currentQuestion { get; private set; }
    public bool isFinished;

    public GameSession(QuestionDefinition startQuestion)
    {
        currentQuestion = startQuestion;
    }

    public void AnswerQuestion(int answerIndex)
    {
        var answers = currentQuestion.answers;
        var answer = answers[answerIndex];

        //if answer support view = good and then what?

        if(answer.nextQuestion == null)
        {
            isFinished = true;
            return;
        }
        else
        {
            currentQuestion = answer.nextQuestion;
        }
    }
}
