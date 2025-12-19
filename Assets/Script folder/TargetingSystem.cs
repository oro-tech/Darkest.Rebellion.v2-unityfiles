using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    private Character activeCharacter;  // The character using the skill

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // When left mouse button is clicked
        {
            TryTargetEnemy();
        }
    }

    // Raycast to select an enemy character
    void TryTargetEnemy()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);  // Create a ray from the camera through the mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  // Check if the ray hits an object
        {
            Character target = hit.collider.GetComponent<Character>();  // Get the Character component of the hit object

            if (target != null)  // If the hit object is a valid enemy (Character)
            {
                // Store the target in the active character's selectedTargets list
                activeCharacter.SelectTarget(target);
                Debug.Log($"Target selected: {target.characterName}");

                // Optionally, apply the skill to the selected target
                activeCharacter.ApplySkillEffect(activeCharacter.currentSkill1, target);  // Example: Apply skill 1
            }
        }
    }

    // Set the character that is currently selecting a target
    public void SetActiveCharacter(Character character)
    {
        activeCharacter = character;  // Set the character for target selection
    }
}
