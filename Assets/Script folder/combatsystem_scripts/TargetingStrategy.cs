using UnityEngine;
using System.Collections.Generic;

public abstract class TargetingStrategy : ScriptableObject
{
    public abstract List<Character> GetTargets(Character attacker, List<Character> allEnemies);
}
