using System.Collections.Generic;
using UnityEngine;

public class RuntimeSaveManager : MonoBehaviour
{
    private const string PURCHASED_HEROES_KEY = "purchased_heroes";  // Key for PlayerPrefs

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Save the hero purchase state to PlayerPrefs for runtime
    public void SaveHeroPurchase(string heroName, bool isPurchased)
    {
        List<string> purchasedHeroes = LoadPurchasedHeroes();

        if (isPurchased)
        {
            if (!string.IsNullOrEmpty(heroName) && !purchasedHeroes.Contains(heroName))
                purchasedHeroes.Add(heroName);
        }
        else
        {
            if (string.IsNullOrEmpty(heroName))
            {
                // Special case: reset all heroes
                purchasedHeroes.Clear();
            }
            else if (purchasedHeroes.Contains(heroName))
            {
                purchasedHeroes.Remove(heroName);
            }
        }

        PlayerPrefs.SetString(PURCHASED_HEROES_KEY, string.Join(",", purchasedHeroes));
        PlayerPrefs.Save();
    }

    // Load the list of purchased heroes from PlayerPrefs
    public List<string> LoadPurchasedHeroes()
    {
        string savedData = PlayerPrefs.GetString(PURCHASED_HEROES_KEY, "");
        List<string> purchasedHeroes = new List<string>();

        if (!string.IsNullOrEmpty(savedData))
            purchasedHeroes.AddRange(savedData.Split(','));

        return purchasedHeroes;
    }
}
