using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// button script, when clicked callback with the index of the answer, so question view can call game session to advance the question
/// </summary>
public class AnswerButton : MonoBehaviour
{
    private Button button;

    private int index;
    private Action<int> onClick;

    public void Initialize(int index, Action<int> onClick)
    {
        this.index = index;
        this.onClick = onClick;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        button.onClick.AddListener(() => TutorialView.Instance.CompleteStep(TutorialTrigger.AnswerSelected));
    }

    private void OnButtonClicked()
    {
        onClick?.Invoke(index);
    }
}
