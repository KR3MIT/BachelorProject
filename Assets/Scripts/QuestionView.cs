using TMPro; 
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI adapter thing
/// </summary>
public class QuestionView : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private AnswerButton answerButtonPrefab;
    [SerializeField] private RectTransform documentPanel;
    [SerializeField] private float offScreenBuffer = 50f;

    [SerializeField] private List<RectTransform> papers;
    [SerializeField] private List<Button> buttons;

    [SerializeField] private GameObject paperContainer;
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
            var button = Instantiate(answerButtonPrefab, answersContainer);
            button.Initialize(i, answers[i].answerText, OnAnswerSelected);
        }
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

    public void MoveToTop(int index)
    {
        papers[index].SetAsLastSibling();
    }

}
