using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackSystem : MonoBehaviour
{
    // This method handles the attack based on the character's position
    public void Attack(Character attacker, Character target, SkillConfig skill)
    {
        if (attacker == null || target == null || skill == null)
        {
            Debug.LogWarning("Attack failed. Missing attacker, target, or skill.");
            return;
        }

        // Apply skill to target based on attack logic
        attacker.ApplySkillEffect(skill, target);
    }

    // This method handles the target selection logic based on the character's position
    public Character SelectTargetBasedOnPosition(Character attacker, List<Character> possibleTargets)
    {
        switch (attacker.position)
        {
            case CharacterPosition.Front:
                return possibleTargets.FirstOrDefault(t => t.position == CharacterPosition.Front);  // Front targets front
            case CharacterPosition.Middle:
                return possibleTargets.FirstOrDefault(t => t.position == CharacterPosition.Middle || t.position == CharacterPosition.Front);  // Middle targets front or middle
            case CharacterPosition.Back:
                return possibleTargets.FirstOrDefault(t => t.position == CharacterPosition.Back);  // Back targets back
            default:
                return null;
        }
    }
}
