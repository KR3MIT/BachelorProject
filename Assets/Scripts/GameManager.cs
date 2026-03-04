using UnityEngine;
/// <summary>
/// overall manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private QuestionView questionView;
    [SerializeField] private TimelineDefinition timeline;
    private GameSession session;

    public void Start()
    {
        session = new GameSession(timeline, QuestionDefinition.ViewpointType.Realist);
        questionView.answerSelected += OnAnswerSelected;
        questionView.Show(session.timeline.questions[0]);
    }

    private void OnAnswerSelected(int answerIndex)
    {
        //session.AnswerQuestion(answerIndex);

        //if (session.isFinished)
        //{
        //    Debug.Log("Game Finished!");
        //    //end screen panel? idk
        //    return;
        //}

        //questionView.Show(session.currentQuestion);
    }
}
