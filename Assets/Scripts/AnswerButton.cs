using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// button script, when clicked callback with the index of the answer, so question view can call game session to advance the question
/// </summary>
public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    private int index;
    private Action<int> onClick;

    public void Initialize(int index, string text, Action<int> onClick)
    {
        this.index = index;
        label.text = text;
        this.onClick = onClick;

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        onClick?.Invoke(index);
    }
}
