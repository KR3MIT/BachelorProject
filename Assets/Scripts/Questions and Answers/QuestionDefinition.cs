using System;
using UnityEngine;

/// <summary>
/// scriptable object that defines a single question and the possible answers.
/// </summary>
[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Objects/Question")]
public class QuestionDefinition : ScriptableObject
{
    [TextArea] public string question;

    public AnswerOption[] answers;

}

[Serializable]
public class AnswerOption
{
    [TextArea] public string text;

    //if question has a next question
    public QuestionDefinition nextQuestion;

    //how should i support the diferent views? score/bool/idk???
    //public bool supportsview?


}
