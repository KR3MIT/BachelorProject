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
    public ButtonHover buttonHover;
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
    [SerializeField] private Vector3 initialLeftPosition;

    //[SerializeField] private List<RectTransform> papers;
    //[SerializeField] private List<TMPTextFormatter> answerTexts;
    //private List<AnswerButton> answerButtons = new List<AnswerButton>();

    private RectTransform canvasRect;
    private Action<int> currentCallback;

    private void Awake()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        initialLeftPosition = paperDatas[0].paper.position;
    }

    private void Start()
    {
        documentPanel.anchoredPosition = GetOffScreen(true);
    }

    private Vector2 GetOffScreen(bool isBottom)
    {
        if (isBottom)
            return new Vector2(documentPanel.anchoredPosition.x, -(canvasRect.rect.height / 2f + documentPanel.rect.height / 2f + offScreenBuffer));
        else
            return new Vector2(documentPanel.anchoredPosition.x, canvasRect.rect.height / 2f + documentPanel.rect.height / 2f + offScreenBuffer);
    }

    /// <summary>
    /// Shows the question and calls onAnswerSelected when the player picks an answer.
    /// </summary>
    public async void Show(QuestionStep question, Action<int> onAnswerSelected)
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

        paperDatas[0].paper.SetAsLastSibling();//green first
        
        if(answers.Count() == 2)
            paperDatas[paperDatas.Count() - 1].paper.gameObject.SetActive(false);
        else 
            paperDatas[paperDatas.Count() - 1].paper.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(answersContainer.GetComponent<RectTransform>());

        FolderAnimation.Instance.SlideIn(answers.Count() == 2);

        foreach (var p in paperDatas) p.buttonHover.isActive = false;
        await documentPanel.DOAnchorPos(Vector2.zero, 1).SetDelay(1).AsyncWaitForCompletion();
        foreach (var p in paperDatas) p.buttonHover.isActive = true;


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
        var lastChild = paperContainer.transform.GetChild(paperContainer.transform.childCount - 1); //get last child

        if(lastChild == paperDatas[index].paper)
        {
            return;
        }

        paperDatas[index].paper.transform.SetSiblingIndex(lastChild.GetSiblingIndex() - 1);//set chosen child to be behind the last child

        await lastChild.DOMoveX(initialLeftPosition.x - 500f, .3f).AsyncWaitForCompletion();//move last child

        paperDatas[index].paper.SetAsLastSibling();//set chosen child to be the last child

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

        lastChild.DOMoveX(initialLeftPosition.x, .3f);
    }

}
