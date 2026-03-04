using System;
using UnityEngine;

/// <summary>
///base class for timeline steps
/// </summary>
public abstract class TimelineStep : ScriptableObject
{
    /// <summary>
    ///when the step should begin onComplete when the step is donee
    /// </summary>
    public abstract void Begin(GameSession session, Action onComplete);
}