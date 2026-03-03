using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Timeline", menuName = "Scriptable Objects/Timeline")]
public class TimelineDefinition : ScriptableObject
{
    public List<QuestionDefinition> questions;
}
