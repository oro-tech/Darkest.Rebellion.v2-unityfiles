using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewHero", menuName = "Hero/Hero Data")]
public class HeroData : ScriptableObject
{
    [Header("Basic Info")]
    public string heroName;
    public CharacterRole role;
    public Sprite portrait;
    public int price;
    public GameObject[] items;   // Item prefabs for this hero
    public GameObject Book;

    [Header("Stats")]
    public int maxHealth;
    public float damageReduction;
    public float damageMultiplier;
    public float healingEffectiveness;

    [Header("Battle Reference")]
    public GameObject battlePrefab;

    [Header("Skills")]
    public SkillConfig specialSkill1;
    public SkillConfig specialSkill2;
    public AttackConfig attackConfig;
    public SkillConfig skill1Config;
    public SkillConfig skill2Config;

    public GameObject SkillButtonPrefab1;
    public GameObject SkillButtonPrefab2;

    [Header("Turn Order")]
    public int speed;

    [Header("Hero Story")]
    [TextArea(3, 10)]
    public string heroStoryText;
    public GameObject heroStoryPrefab;

    // 🟢 Added for shop logic
    [HideInInspector] public List<string> purchasedItems = new List<string>();
    [HideInInspector] public List<string> equippedItems = new List<string>();

    public void EquipItem(string itemName)
    {
        if (!equippedItems.Contains(itemName))
            equippedItems.Add(itemName);
    }

    public bool HasPurchased(string itemName)
    {
        return purchasedItems.Contains(itemName);
    }

    public bool IsEquipped(string itemName)
    {
        return equippedItems.Contains(itemName);
    }
}
