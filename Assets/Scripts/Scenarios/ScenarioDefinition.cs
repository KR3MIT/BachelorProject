using UnityEngine;

/// <summary>
/// scriptable object that defines a run, like a viewpoint idk
/// </summary>
[CreateAssetMenu(fileName = "Scenario", menuName = "Scriptable Objects/Scenario")]
public class ScenarioDefinition : ScriptableObject
{
    //viewpoint?

    public QuestionDefinition startQuestion;
}
