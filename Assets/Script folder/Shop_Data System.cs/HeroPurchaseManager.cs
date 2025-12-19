using System.Collections.Generic;
using UnityEngine;

public class HeroPurchaseManager : MonoBehaviour
{
    public static HeroPurchaseManager Instance;

    private HashSet<string> purchasedHeroes = new HashSet<string>();
    private RuntimeSaveManager saveManager;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveManager = FindObjectOfType<RuntimeSaveManager>();
        if (saveManager == null)
        {
            GameObject go = new GameObject("RuntimeSaveManager");
            saveManager = go.AddComponent<RuntimeSaveManager>();
        }

        LoadPurchasedHeroes();
    }

    public bool IsHeroPurchased(HeroData hero)
    {
        bool isPurchased = purchasedHeroes.Contains(hero.heroName);
        Debug.Log($"[HeroPurchaseManager] Checking {hero.heroName}: {isPurchased}");
        return isPurchased;
    }

    public void PurchaseHero(HeroData hero)
    {
        if (!purchasedHeroes.Contains(hero.heroName))
        {
            purchasedHeroes.Add(hero.heroName);
            saveManager.SaveHeroPurchase(hero.heroName, true);

            Debug.Log($"[HeroPurchaseManager] Saved {hero.heroName} as purchased.");
        }
    }

    private void LoadPurchasedHeroes()
    {
        purchasedHeroes.Clear();
        List<string> loaded = saveManager.LoadPurchasedHeroes();
        foreach (string heroName in loaded)
        {
            purchasedHeroes.Add(heroName);
            Debug.Log($"[HeroPurchaseManager] Loaded {heroName} as purchased from save.");
        }
    }

    public void ResetPurchases()
    {
        purchasedHeroes.Clear();
        saveManager.SaveHeroPurchase("", false); // clear all
        Debug.Log("[HeroPurchaseManager] Purchases reset.");
    }
}
