using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// button script, when clicked callback with the index of the answer, so question view can call game session to advance the question
/// </summary>
public class AnswerButton : MonoBehaviour
{
    public Button button { get; private set; }

    private int index;
    private Action<int> onClick;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Initialize(int index, Action<int> onClick)
    {
        this.index = index;
        this.onClick = onClick;

        button.onClick.AddListener(OnButtonClicked);
        button.onClick.AddListener(() => TutorialView.Instance.CompleteStep(TutorialTrigger.AnswerSelected));
    }

    private void OnButtonClicked()
    {
        onClick?.Invoke(index);
    }
}
