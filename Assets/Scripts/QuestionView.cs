using TMPro; 
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct PaperData
{
    public RectTransform paper;
    public TMPTextFormatter answerText;
    public AnswerButton answerButton;

    public PaperData(RectTransform paper, TMPTextFormatter answerText, AnswerButton answerButton)
    {
        this.paper = paper;
        this.answerText = answerText;
        this.answerButton = answerButton;
    }
}

/// <summary>
/// UI adapter thing
/// </summary>
public class QuestionView : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Transform answersContainer;
    //[SerializeField] private AnswerButton answerButtonPrefab;
    [SerializeField] private RectTransform documentPanel;
    [SerializeField] private float offScreenBuffer = 50f;

    [SerializeField] private GameObject paperContainer;
    [SerializeField] private List<PaperData> paperDatas;

    //[SerializeField] private List<RectTransform> papers;
    //[SerializeField] private List<TMPTextFormatter> answerTexts;
    //private List<AnswerButton> answerButtons = new List<AnswerButton>();
    
    private RectTransform canvasRect;
    private Action<int> currentCallback;

    private void Awake()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        documentPanel.anchoredPosition = GetOffScreen(true);
    }

    private Vector2 GetOffScreen(bool isLeft)
    {
        if (isLeft)
            return new Vector2(-(canvasRect.rect.width / 2f + documentPanel.rect.width / 2f + offScreenBuffer), documentPanel.anchoredPosition.y);
        else
            return new Vector2(canvasRect.rect.width / 2f + documentPanel.rect.width / 2f + offScreenBuffer, documentPanel.anchoredPosition.y);
    }

    /// <summary>
    /// Shows the question and calls onAnswerSelected exactly once when the player picks an answer.
    /// </summary>
    public void Show(QuestionStep question, Action<int> onAnswerSelected)
    {
        currentCallback = onAnswerSelected;
        gameObject.SetActive(true);

        questionText.text = question.question;

        for (int i = answersContainer.childCount - 1; i >= 0; i--)
            Destroy(answersContainer.GetChild(i).gameObject);

        var answers = question.answers;

        //randomize answers order
        for(int i = answers.Count() - 1; i >= 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = answers[i];
            answers[i] = answers[randomIndex];
            answers[randomIndex] = temp;
        }
        Debug.Log("answers order: " + string.Join(", ", answers.Select(a => a.viewpoint)));

        for (int i = 0; i < answers.Length; i++)
        {
            //fill paper 1s text with the 1st random answer and so on
            paperDatas[i].answerText.SetText(answers[i].answerText);
            //link button to answer index, so when clicked can callback with the index of the answer
            paperDatas[i].answerButton.Initialize(i, OnAnswerSelected);
        }

        MoveToTop(0);//always green first

        LayoutRebuilder.ForceRebuildLayoutImmediate(answersContainer.GetComponent<RectTransform>());

        documentPanel.DOAnchorPos(Vector2.zero, 2).SetDelay(1);
        FolderAnimation.Instance.SlideIn(answers.Count() == 2);
    }

    private async void OnAnswerSelected(int index)
    {
        await Hide();

        await FolderAnimation.Instance.MoveStamp(index);
        FolderAnimation.Instance.SlideOut();

        currentCallback?.Invoke(index);
        currentCallback = null;
    }

    public async Task Hide()
    {
        float moveOffScreenTime = 1f;

        documentPanel.DOAnchorPos(GetOffScreen(false), moveOffScreenTime)
            .OnComplete(() => { 
                gameObject.SetActive(false);
                documentPanel.anchoredPosition = GetOffScreen(true);
            });

        await Task.Delay((int)(moveOffScreenTime * 1000f / 2f));//only wait for half move time because else stamp is slow af
    }

    public async void MoveToTop(int index)
    {
        //get last child
        var lastChild = paperContainer.transform.GetChild(paperContainer.transform.childCount - 1);

        await lastChild.DOJump(paperDatas[index].paper.position, 100, 1, .3f).AsyncWaitForCompletion();


        paperDatas[index].paper.SetAsLastSibling();

        foreach (var paperData in paperDatas)
        {
            if (paperData.paper == paperDatas[index].paper)
            {
                paperData.answerText.tooltipEnabled = true;
            }
            else
            {
                paperData.answerText.tooltipEnabled = false;
            }
        }
    }

}
