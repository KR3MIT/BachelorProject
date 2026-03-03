using TMPro; 
using UnityEngine;
using System;

/// <summary>
/// UI adapter thing
/// </summary>
public class QuestionView : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private AnswerButton answerButtonPrefab;

    public event Action<int> answerSelected;

    public void Show(QuestionDefinition question)
    {
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
    }

    private void OnAnswerSelected(int index)
    {
        answerSelected?.Invoke(index);
    }
}
