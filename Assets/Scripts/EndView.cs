using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class EndView : MonoBehaviour
{
    public TMP_Text resultsText;
    public void Show(Dictionary<ViewpointType, int> counts, float approvalRating)
    {
        gameObject.SetActive(true);

        var sb = new StringBuilder();
        sb.AppendLine("Spørgsmåls valg:");

        foreach (ViewpointType vp in Enum.GetValues(typeof(ViewpointType)))
        {
            counts.TryGetValue(vp, out int c);
            sb.AppendLine($"{vp}: {c}");
        }

        var result = sb.ToString();
        resultsText.text = result;
    }
}
