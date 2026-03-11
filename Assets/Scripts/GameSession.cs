using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// runtime class for a run, advances the questions
/// </summary>
public class GameSession
{
    public ViewpointType viewpoint { get; private set; }

    public TimelineDefinition timeline { get; private set; }

    //callbacks for ui and stuff
    public IGameUI ui;

    private readonly Dictionary<ViewpointType, int> questionSelectionCounts;

    public GameSession(ViewpointType viewpoint, IGameUI ui, TimelineDefinition timeline)
    {
        this.viewpoint = viewpoint;
        this.ui = ui;
        this.timeline = timeline;

        questionSelectionCounts  = new Dictionary<ViewpointType, int>();
        foreach (ViewpointType vp in Enum.GetValues(typeof(ViewpointType)))
        {
            questionSelectionCounts[vp] = 0;
        }
    }

    public void RecordQuestionSelection(ViewpointType viewpoint)
    {
        questionSelectionCounts[viewpoint]++;
    }

    public Dictionary<ViewpointType, int> GetQuestionSelectionCounts()
    {
        return new Dictionary<ViewpointType, int>(questionSelectionCounts);
    }

    public void ChangeApproval(ApprovalChangeType approvalType)
    {
        int questionCount = 0;
        foreach (var step in timeline.steps)
        {
            if(step is QuestionStep questionStep)
            {
                questionCount++;
            }
        }
        float approvalChangeAmount = 50f / questionCount;

        if (approvalType == ApprovalChangeType.Add)
            UIApprovalRating.Instance.AddApproval(approvalChangeAmount);
        else if(approvalType == ApprovalChangeType.Remove)
            UIApprovalRating.Instance.RemoveApproval(approvalChangeAmount);
        else if(approvalType == ApprovalChangeType.SmallRemove)
            UIApprovalRating.Instance.RemoveApproval(approvalChangeAmount / 2f);
    }
}
public enum ApprovalChangeType
{
    Add,
    Remove,
    SmallRemove,
}
