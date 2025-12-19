using TMPro;
using UnityEngine;

public class LobbyCoinDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text coinText;

    void Start()
    {
        UpdateCoinDisplay();
    }

    public void UpdateCoinDisplay()
    {
        // Load saved coins (default to 1000 if not found)
        int coins = PlayerPrefs.GetInt("PlayerCoins", 2000);
        coinText.text = "Coins: " + coins.ToString();
    }
}
