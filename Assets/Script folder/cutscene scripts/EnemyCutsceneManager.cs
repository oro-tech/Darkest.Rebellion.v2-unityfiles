using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCutsceneSet
{
    [Header("Target Info")]
    public string targetName; // e.g. "JoseRizal"

    [Header("Cutscene IDs (match GameObject names)")]
    public string skill1CutsceneId; // e.g. "SilangVsRizal_Cutscene1"
    public string skill2CutsceneId; // e.g. "SilangVsRizal_Cutscene2"
}

public class EnemyCutsceneManager : MonoBehaviour
{
    [Header("Cutscenes Per Target (by ID)")]
    public List<EnemyCutsceneSet> cutscenesForTargets = new List<EnemyCutsceneSet>();

    // Get cutscene GameObject name (ID) for a given target and skill
    public string GetCutsceneId(string targetName, int skillIndex)
    {
        foreach (var set in cutscenesForTargets)
        {
            if (set.targetName == targetName)
            {
                if (skillIndex == 1) return set.skill1CutsceneId;
                if (skillIndex == 2) return set.skill2CutsceneId;
            }
        }
        return null;
    }
}
