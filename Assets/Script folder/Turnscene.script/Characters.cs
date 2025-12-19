using UnityEngine;
using System.Collections.Generic;

public enum CharacterPosition
{
    Front,
    Middle,
    Back
}

public enum CharacterRole
{
    Tank,
    Damage,
    Support
}

public class Character : MonoBehaviour
{
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public bool isAlive => currentHealth > 0;
    public CharacterRole role;
    public CharacterPosition position;

    [Header("Core Multipliers")]
    [Range(0f, 1f)] public float damageReduction;       // 0.2 = 20% reduction
    public float damageMultiplier;      // 0.3 = 30% more outgoing damage
    public float healingEffectiveness;  // 0.25 = 25% more healing

    [Header("Mana Settings")]
    public int maxMana = 100;
    public int currentMana = 100;
    [Tooltip("How much mana this character regains at the start of their turn.")]
    public int manaRegenPerTurn = 10;

    [Header("Skills / Configs")]
    public SkillConfig currentSkill1;
    public SkillConfig currentSkill2;
    public AttackConfig attackConfig;

    public List<Character> selectedTargets = new List<Character>();
    public HeroData heroData;

    // Initialize the character with the provided data
    public void Init(string name, int hp, CharacterRole role,
                     float dmgReduction, float dmgMultiplier, float healEffectiveness,
                     SkillConfig skill1, SkillConfig skill2, AttackConfig attackConfig,
                     CharacterPosition position, HeroData heroData)
    {
        characterName = name;
        maxHealth = hp;
        currentHealth = hp;
        this.role = role;
        this.position = position;
        damageReduction = dmgReduction;
        damageMultiplier = dmgMultiplier;
        healingEffectiveness = healEffectiveness;

        currentSkill1 = skill1;
        currentSkill2 = skill2;
        this.attackConfig = attackConfig;
        this.heroData = heroData;

        // Initialize Mana
        currentMana = maxMana;

        ApplyRoleModifiers();
    }

    // Apply role-specific modifiers (once when character is created)
    public void ApplyRoleModifiers()
    {
        switch (role)
        {
            case CharacterRole.Tank:
                damageReduction += 0.2f;  // Tanks reduce incoming damage
                damageReduction = Mathf.Clamp01(damageReduction); // prevent over 100%
                break;
            case CharacterRole.Damage:
                damageMultiplier += 0.3f; // Damage roles do more outgoing damage
                break;
            case CharacterRole.Support:
                healingEffectiveness += 0.25f; // Supports heal better
                break;
        }
    }

    // Apply a skill effect (damage or healing) to a target character
    public void ApplySkillEffect(SkillConfig skill, Character target)
    {
        if (target == null || skill == null)
        {
            Debug.LogWarning("Cannot apply skill effect: target or skill is null.");
            return;
        }

        // --- ✅ Check Mana Availability ---
        if (!HasEnoughMana(skill.manaCost))
        {
            Debug.LogWarning($"{characterName} tried to use {skill.skillName} but does not have enough mana! " +
                             $"({currentMana}/{skill.manaCost})");
            return;
        }

        // --- Spend Mana Before Applying the Effect ---
        SpendMana(skill.manaCost);

        int finalValue = skill.baseValue;

        // --- Outgoing modifiers (from caster/attacker) ---
        if (skill.isDamageType)
        {
            finalValue = Mathf.RoundToInt(finalValue * (1 + damageMultiplier));
        }

        if (skill.isHealingSkill)
        {
            finalValue = Mathf.RoundToInt(finalValue * (1 + healingEffectiveness));
        }

        // --- Apply effect to target ---
        if (skill.isDamageType)
        {
            target.TakeDamage(finalValue, this); // pass attacker for better logs
        }
        else if (skill.isHealingSkill)
        {
            target.Heal(finalValue, this);
        }

        Debug.Log($"{characterName} used {skill.skillName} on {target.characterName} " +
                  $"and spent {skill.manaCost} mana. Remaining mana: {currentMana}/{maxMana}");
    }

    // --- Damage and Healing ---
    public void TakeDamage(int amount, Character attacker = null)
    {
        float clampedReduction = Mathf.Clamp01(damageReduction);

        float finalDamage = amount * (1 - clampedReduction);
        int damageToApply = Mathf.Max(1, Mathf.RoundToInt(finalDamage)); // never heal, min 1 dmg

        currentHealth = Mathf.Max(0, currentHealth - damageToApply);

        string attackerName = attacker != null ? attacker.characterName : "Unknown";
        Debug.Log($"{characterName} took {damageToApply} damage from {attackerName} " +
                  $"(reduced by {clampedReduction * 100}%). HP left: {currentHealth}/{maxHealth}");
    }

    public void Heal(int amount, Character healer = null)
    {
        float finalHeal = amount * (1 + healingEffectiveness);
        int healToApply = Mathf.RoundToInt(finalHeal);

        currentHealth = Mathf.Min(maxHealth, currentHealth + healToApply);

        string healerName = healer != null ? healer.characterName : "Unknown";
        Debug.Log($"{characterName} healed {healToApply} HP by {healerName}. " +
                  $"HP now: {currentHealth}/{maxHealth}");
    }

    // --- ✅ Mana System Methods ---
    public bool HasEnoughMana(int cost)
    {
        return currentMana >= cost;
    }

    public void SpendMana(int amount)
    {
        currentMana = Mathf.Max(0, currentMana - amount);
        Debug.Log($"{characterName} spent {amount} mana. Remaining: {currentMana}/{maxMana}");
    }

    public void RegenerateMana()
    {
        int before = currentMana;
        currentMana = Mathf.Min(maxMana, currentMana + manaRegenPerTurn);
        int gained = currentMana - before;

        if (gained > 0)
            Debug.Log($"{characterName} regenerated {gained} mana (Now: {currentMana}/{maxMana})");
    }

    public void ResetSkillUsage()
    {
        Debug.Log($"{characterName}'s skills have been reset for the next turn.");
    }

    public void SelectTarget(Character target)
    {
        if (!selectedTargets.Contains(target))
        {
            selectedTargets.Add(target);
        }
    }

    public void DeselectTarget(Character target)
    {
        selectedTargets.Remove(target);
    }

    public void ResetCharacterStats()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        selectedTargets.Clear();
    }
}
