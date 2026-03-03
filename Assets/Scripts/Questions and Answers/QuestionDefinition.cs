using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// scriptable object that defines a single question and the possible answers.
/// </summary>
[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Objects/Question")]
public class QuestionDefinition : ScriptableObject
{
    [TextArea] public string question;

    public AnswerOption[] answers;

    public enum ViewpointType
    {
        Realist,
        Liberalist,
    }
}

[Serializable]
public class AnswerOption
{
    [TextArea] public string answerText;
    public QuestionDefinition.ViewpointType viewpoint;

    public List<Explanation> explanations;
}

[Serializable]
public class Explanation
{
    [TextArea] public string explanationText;
    public QuestionDefinition.ViewpointType viewpoint;
}
