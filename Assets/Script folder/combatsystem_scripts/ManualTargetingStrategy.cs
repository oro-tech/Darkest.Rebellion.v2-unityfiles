using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManualTargetingStrategy", menuName = "Targeting/ManualTargeting")]
public class ManualTargetingStrategy : TargetingStrategy
{
    public override List<Character> GetTargets(Character attacker, List<Character> allEnemies)
    {
        // Return manually selected targets from the attacker
        return attacker.selectedTargets;
    }
}
