using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class BonifacioSkillbtn : MonoBehaviour
{
    public Image icon;
    public TMP_Text skillNameText;

    private SkillConfig skillConfig;
    private Character activeCharacter;
    private TurnManager turnManager;
    private TargetingSystem targetingSystem;  // Reference to the Targeting System
    private Animator animator;

    // Setup method to initialize the skill button
    public void Setup(SkillConfig skill, Character character, TurnManager manager, TargetingSystem targeting)
    {
        skillConfig = skill;
        activeCharacter = character;
        turnManager = manager;
        targetingSystem = targeting;



        // Ensure the button triggers the correct skill on click
        GetComponent<Button>().onClick.AddListener(OnButtonPressed);
    }

    // When the skill button is pressed
    public void OnButtonPressed()
    {
        if (turnManager == null || skillConfig == null || activeCharacter == null)
            return;


        // 🔹 Apply skill logic
        if (skillConfig.targetingStrategy is ManualTargetingStrategy)
        {
            EnableTargetSelection(); // Wait for player to pick target
        }
        else
        {
            if (skillConfig == activeCharacter.heroData.specialSkill1)
                turnManager.OnSkill1();
            else if (skillConfig == activeCharacter.heroData.specialSkill2)
                turnManager.OnSkill2();
        }
    }

    // Method to enable target selection for manual targeting
    private void EnableTargetSelection()
    {
        Debug.Log("Manual target selection enabled.");
        targetingSystem.SetActiveCharacter(activeCharacter);
    }
}
