using System.Linq;  // Import LINQ

using System.Collections.Generic;

[System.Serializable]
public class AttackConfig
{
    public string characterName;          // Character's name
    public CharacterPosition attackPosition;  // Position of the character (front, middle, back)
    public int attackRange;               // Number of enemies this character can target (1, 2, etc.)
    public string targetTag;              // Tag to identify the target (e.g., "EnemyFront", "PlayerBack")

    // Method to return the correct attack behavior based on the character’s position
    public List<Character> GetTargets(List<Character> availableTargets)
    {
        // Find all characters matching the target tag (e.g., "EnemyFront")
        return availableTargets.Where(t => t.CompareTag(targetTag)).ToList();
    }
}
