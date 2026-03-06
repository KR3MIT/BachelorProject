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

    public event Action<int> answerSelected;

    private RectTransform canvasRect;

    private void Awake()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        panel.anchoredPosition = GetOffScreen(true);
    }

    private Vector2 GetOffScreen(bool isLeft)
    {
        if (isLeft)
        {
            return new Vector2(-(canvasRect.rect.width / 2f + panel.rect.width / 2f + offScreenBuffer), panel.anchoredPosition.y);
        }
        else
        {
            return new Vector2 (canvasRect.rect.width / 2f + panel.rect.width / 2f + offScreenBuffer, panel.anchoredPosition.y); 
        }
    }

    public void Show(QuestionStep question)
    {
        gameObject.SetActive(true);

        questionText.text = question.question;

        for (int i = answersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(answersContainer.GetChild(i).gameObject);
        }

        var answers = question.answers;
        for (int i = 0; i < answers.Length; i++)
        {
            var button = Instantiate(answerButtonPrefab, answersContainer);
            button.Initialize(i, answers[i].answerText, OnAnswerSelected);
        }

        panel.DOAnchorPos(Vector2.zero, 2);
    }

    public async Task Hide() 
    { 
        await panel.DOAnchorPos(GetOffScreen(false), 2)
            .OnComplete(() => gameObject.SetActive(false)).AsyncWaitForCompletion();

        panel.anchoredPosition = GetOffScreen(true);
    }

    private void OnAnswerSelected(int index)
    {
        answerSelected?.Invoke(index);
    }
}
