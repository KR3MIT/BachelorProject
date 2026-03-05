using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TooltipEntry
{
    public string keyword;
    [TextArea] public string definition;
}

[CreateAssetMenu(fileName = "TooltipDefinition", menuName = "Scriptable Objects/TooltipDefinition")]
public class TooltipDefinition : ScriptableObject
{
    public List<TooltipEntry> entries;

    public bool TryGetDefinition(string keyword, out string definition)
    {
        foreach (var entry in entries)
        {
            if (string.Equals(entry.keyword, keyword, StringComparison.OrdinalIgnoreCase))
            {
                definition = entry.definition;
                return true;
            }
        }
        definition = null;
        return false;
    }
}
