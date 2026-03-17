using DG.Tweening;
using JetBrains.Annotations;
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

}

[Serializable]
public struct TutorialData
{
    public Vector2 position;
    [TextArea] public string tutorialText;
    public TutorialTrigger trigger;
}

public class TutorialView : MonoBehaviour
{
    public static TutorialView Instance;
    [SerializeField] private TMP_Text tutorialText;

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

    public async void StartTutorial(List<TutorialData> steps, Action onTutorialComplete)
    {
        tutorialSteps = steps;
        currentStepIndex = 0;
        this.onTutorialComplete = onTutorialComplete;

        await Task.Delay(1500);

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

    void ShowStep(int stepIndex) 
    {
        tutorialText.text = tutorialSteps[stepIndex].tutorialText;

        rect.anchoredPosition = tutorialSteps[stepIndex].position;

        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.3f);
    }

    public void EndTutorial()
    {
        canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        { 
            gameObject.SetActive(false);
            onTutorialComplete?.Invoke();        
        });
    }
}
