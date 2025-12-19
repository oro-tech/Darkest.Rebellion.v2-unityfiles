using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Configs/SkillConfig")]
public class SkillConfig : ScriptableObject
{
    [Header("Skill Info")]
    [Tooltip("The display name of the skill.")]
    public string skillName = "Unnamed Skill";

    [Tooltip("The icon representing this skill in the UI.")]
    public Sprite icon;

    [TextArea]
    [Tooltip("A brief description of the skill.")]
    public string description;

    [Header("Skill Type Settings")]
    [Tooltip("Base power or effect value of this skill.")]
    public int baseValue;

    [Tooltip("If true, this skill deals damage.")]
    public bool isDamageType = true;

    [Tooltip("If true, this skill heals the target instead of damaging.")]
    public bool isHealingSkill = false;

    [Tooltip("If true, this skill is a support type (buff, shield, etc.).")]
    public bool isSupportSkill = false;

    [Header("Targeting & Animation")]
    [Tooltip("Strategy for determining how this skill targets enemies or allies.")]
    public TargetingStrategy targetingStrategy;

    [Tooltip("Animator trigger name for this skill animation (used if no cutscene is defined).")]
    public string animationTrigger;

    [Header("Cutscene Settings")]
    [Tooltip("Optional: Name of the cutscene GameObject in the scene that contains a PlayableDirector.")]
    public string cutsceneName;

    [Tooltip("If true, this skill will use a Timeline cutscene instead of a standard animation trigger.")]
    public bool useCutscene = false;

    [Header("Cooldown Settings")]
    [Tooltip("Cooldown duration (in seconds) before the skill can be reused.")]
    [Range(0f, 60f)]
    public float cooldownTime = 5f;  // Default cooldown of 5 seconds

    [Header("Mana Settings")]
    [Tooltip("The amount of mana required to use this skill.")]
    public int manaCost = 10;

    [Header("Damage Settings")]
    [Tooltip("Optional: Used if you want fixed damage separate from baseValue.")]
    public int damage;

    // 🧠 Optional validation for debugging setup issues
    public void Validate()
    {
        if (string.IsNullOrEmpty(skillName))
            Debug.LogError($"[SkillConfig] Missing skill name for {name}");

        if (icon == null)
            Debug.LogWarning($"[SkillConfig] Missing icon for skill '{skillName}'");

        if (targetingStrategy == null)
            Debug.LogWarning($"[SkillConfig] Missing targeting strategy for skill '{skillName}'");

        if (useCutscene && string.IsNullOrEmpty(cutsceneName))
            Debug.LogWarning($"[SkillConfig] Skill '{skillName}' is set to use a cutscene but no cutsceneName is provided.");
    }
}
