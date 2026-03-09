using System;
using UnityEngine;
/// <summary>
/// runtime class for a run, advances the questions
/// </summary>
public class GameSession
{
    public ViewpointType viewpoint { get; private set; }
    public int approvalRating { get; private set; }

    //callbacks for ui and stuff
    public IGameUI ui;

    public GameSession(ViewpointType viewpoint, IGameUI ui)
    {
        this.viewpoint = viewpoint;
        this.ui = ui;
        approvalRating = 0;
    }

    public void AddApproval(int amount)
    {
        approvalRating += amount;
    }
}
