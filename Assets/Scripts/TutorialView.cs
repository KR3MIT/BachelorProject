using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[Serializable]
public enum TutorialTrigger
{
    Any,
    Click,
    ChangeAnswer,
    AnswerSelected,

}

[Serializable]
public struct TutorialData
{
    public Vector2 position;
    [TextArea] public string tutorialText;
    public TutorialTrigger trigger;
    public int delay;
}

public class TutorialView : MonoBehaviour
{
    public static TutorialView Instance;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private QuestionView questionView; //sorry for my transgressions but idk how else to do this. Forgive me

    private List<TutorialData> tutorialSteps;
    private int currentStepIndex = 0;
    private Action onTutorialComplete;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    private void Awake()
    {
        Instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space)) { CompleteStep(TutorialTrigger.Any); }

        if(Input.GetKeyDown(KeyCode.Mouse0) && currentStepIndex == 0) 
        { 
            CompleteStep(TutorialTrigger.Click);
        }
    }

    public async Awaitable StartTutorial(List<TutorialData> steps, Action onTutorialComplete)
    {
        tutorialSteps = steps;
        currentStepIndex = 0;
        this.onTutorialComplete = onTutorialComplete;

        await Awaitable.WaitForSecondsAsync(1.5f);


        gameObject.SetActive(true);
        ShowStep(0);
    }

    /// <summary>
    /// call this anywhere a step is completed
    /// </summary>
    public void CompleteStep(TutorialTrigger tutorialTrigger)
    {
        if (!gameObject.activeSelf) { return; }

        if (tutorialTrigger != TutorialTrigger.Any)//any trigger will complete the step always
        {
            if (tutorialSteps[currentStepIndex].trigger != tutorialTrigger)//if not any need specific
            {
                return;
            }
        }


        Debug.Log("Tutorial step completed: " + tutorialSteps[currentStepIndex].tutorialText);

        currentStepIndex++;

        if(currentStepIndex < tutorialSteps.Count)
        {
            ShowStep(currentStepIndex);
        }
        else
        {
            EndTutorial();
        }
    }

    async Awaitable ShowStep(int stepIndex) 
    {
        switch (tutorialSteps[stepIndex].trigger)
        {
            case TutorialTrigger.Click:
                questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.Everything);
                break;
            case TutorialTrigger.ChangeAnswer:
                questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.SelectButton);
                break;
            case TutorialTrigger.AnswerSelected:
                questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.HoverButtons);
                break;
            case TutorialTrigger.Any:
                questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.Nothing);
                break;
            default:
                questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.Nothing);
                break;
        }

        canvasGroup.alpha = 0f;

        await Awaitable.WaitForSecondsAsync(tutorialSteps[stepIndex].delay/1000);
        //await Task.Delay(tutorialSteps[stepIndex].delay);

        tutorialText.text = tutorialSteps[stepIndex].tutorialText;
        rect.anchoredPosition = tutorialSteps[stepIndex].position;

        canvasGroup.DOFade(1f, 0.3f);
    }

    public void EndTutorial()
    {
        questionView.SetElementNotInteractable(QuestionView.SetNotInteractable.Nothing);

        canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        { 
            gameObject.SetActive(false);
            onTutorialComplete?.Invoke();        
        });
    }
}
