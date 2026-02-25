using UnityEngine;
/// <summary>
/// overall manager
/// </summary>
public class GameManager : MonoBehaviour
{

    public ScenarioDefinition scenario;

    [SerializeField] private QuestionView questionView;

    private GameSession session;


    public void Start()
    {
        //viewpoint = scenario.viewpoint=???
        session = new GameSession(scenario.startQuestion);

        questionView.answerSelected += OnAnswerSelected;
        questionView.Show(session.currentQuestion);

    }

    private void OnAnswerSelected(int answerIndex)
    {
        session.AnswerQuestion(answerIndex);

        if (session.isFinished)
        {
            Debug.Log("Game Finished!");
            //end screen panel? idk
            return;
        }

        questionView.Show(session.currentQuestion);
    }
}
