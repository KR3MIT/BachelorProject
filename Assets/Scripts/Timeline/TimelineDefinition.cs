using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the sequence of game steps
/// </summary>

[CreateAssetMenu(fileName = "Timeline", menuName = "Scriptable Objects/Timeline")]
public class TimelineDefinition : ScriptableObject
{
    public List<TimelineStep> steps;
}
