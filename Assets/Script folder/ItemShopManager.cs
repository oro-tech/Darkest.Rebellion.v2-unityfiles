using UnityEngine;
using TMPro;

public class ItemShopManager : MonoBehaviour
{
    public static ItemShopManager Instance;

    [Header("UI Reference")]
    public TMP_Text playerCoinsText;

    public int playerCoins;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadCoins();
        UpdateCoinsUI();
    }

    public bool TrySpendCoins(int amount)
    {
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            SaveCoins();
            UpdateCoinsUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    public void AddCoins(int amount)
    {
        playerCoins += amount;
        SaveCoins();
        UpdateCoinsUI();
    }

    private void LoadCoins()
    {
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 1000);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        PlayerPrefs.Save();
    }

    private void UpdateCoinsUI()
    {
        if (playerCoinsText != null)
            playerCoinsText.text = "Coins: " + playerCoins;
    }
}
