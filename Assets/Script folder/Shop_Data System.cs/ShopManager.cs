using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;
    public GameObject heroSlotPrefab;
    public TMP_Text playerCoinsText;
    public TMP_Text purchaseFeedbackText;
    public HeroData[] allHeroes;

    public int playerCoins;
    private HeroPurchaseManager heroPurchaseManager;

    void Start()
    {
        heroPurchaseManager = HeroPurchaseManager.Instance;

        // Load coins (default 500 if not saved yet)
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 2000);

        UpdatePlayerCoinsUI();
        PopulateShop();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPurchaseData();
        }
#endif
    }

    void UpdatePlayerCoinsUI()
    {
        playerCoinsText.text = "Coins: " + playerCoins.ToString();
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        PlayerPrefs.Save();
    }

    void PopulateShop()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var hero in allHeroes)
        {
            GameObject slotGO = Instantiate(heroSlotPrefab, contentParent);
            HeroSlot_Shop slotUI = slotGO.GetComponent<HeroSlot_Shop>();

            if (slotUI != null)
            {
                slotUI.Setup(hero, this);

                if (heroPurchaseManager.IsHeroPurchased(hero))
                    slotUI.SetSelectable(false);
            }
        }
    }

    public void OnHeroSelected(HeroData selectedHero)
    {
        Debug.Log($"Hero selected: {selectedHero.heroName}");

        if (playerCoins >= selectedHero.price)
        {
            playerCoins -= selectedHero.price;
            SaveCoins();
            UpdatePlayerCoinsUI();

            heroPurchaseManager.PurchaseHero(selectedHero);

            purchaseFeedbackText.text = $"You purchased {selectedHero.heroName}!";
            StartCoroutine(HidePurchaseFeedback());

            DisableHeroSlot(selectedHero);
        }
        else
        {
            purchaseFeedbackText.text = "Not enough coins!";
            StartCoroutine(HidePurchaseFeedback());
        }
    }

    private void DisableHeroSlot(HeroData hero)
    {
        foreach (var slot in contentParent.GetComponentsInChildren<HeroSlot_Shop>())
        {
            if (slot.GetHeroData() == hero)
            {
                slot.SetSelectable(false);
            }
        }
    }

    private IEnumerator HidePurchaseFeedback()
    {
        yield return new WaitForSeconds(2f);
        purchaseFeedbackText.text = "";
    }

#if UNITY_EDITOR
    public void ResetPurchaseData()
    {
        Debug.Log("Resetting all purchase data...");

        // Reset coins
        playerCoins = 5000;
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);

        // Reset hero purchases via HeroPurchaseManager
        heroPurchaseManager.ResetPurchases();

        PlayerPrefs.Save();

        UpdatePlayerCoinsUI();
        PopulateShop();

        purchaseFeedbackText.text = "All purchases reset!";
    }
#endif
}
