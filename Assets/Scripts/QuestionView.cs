using TMPro; 
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

/// <summary>
/// UI adapter thing
/// </summary>
public class QuestionView : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private AnswerButton answerButtonPrefab;
    [SerializeField] private RectTransform panel;
    [SerializeField] private float offScreenBuffer = 50f;

    private RectTransform canvasRect;
    private Action<int> currentCallback;

    private void Awake()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        panel.anchoredPosition = GetOffScreen(true);
    }

    private Vector2 GetOffScreen(bool isLeft)
    {
        if (isLeft)
            return new Vector2(-(canvasRect.rect.width / 2f + panel.rect.width / 2f + offScreenBuffer), panel.anchoredPosition.y);
        else
            return new Vector2(canvasRect.rect.width / 2f + panel.rect.width / 2f + offScreenBuffer, panel.anchoredPosition.y);
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
        for (int i = 0; i < answers.Length; i++)
        {
            var button = Instantiate(answerButtonPrefab, answersContainer);
            button.Initialize(i, answers[i].answerText, OnAnswerSelected);
        }

        panel.DOAnchorPos(Vector2.zero, 2).SetDelay(1);
        FolderAnimation.Instance.SlideIn(answersContainer.childCount == 2);
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

        panel.DOAnchorPos(GetOffScreen(false), moveOffScreenTime)
            .OnComplete(() => { 
                gameObject.SetActive(false);
                panel.anchoredPosition = GetOffScreen(true);
            });

        await Task.Delay((int)(moveOffScreenTime * 1000f / 2f));//only wait for half move time because else stamp is slow af
    }
}
