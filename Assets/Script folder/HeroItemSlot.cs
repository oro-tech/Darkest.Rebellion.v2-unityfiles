using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroItemSlot : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemIcon;
    public Button buyButton;
    public Button equipButton;

    [Header("Item Info")]
    public string itemName;
    public int price;
    public Sprite icon;

    private HeroData assignedHero;

    public void Setup(HeroData hero, string name, int cost, Sprite sprite)
    {
        assignedHero = hero;
        itemName = name;
        price = cost;
        icon = sprite;

        itemNameText.text = itemName;
        priceText.text = cost.ToString();
        itemIcon.sprite = sprite;

        UpdateUI();
    }

    public void OnBuy()
    {
        if (ItemShopManager.Instance.TrySpendCoins(price))
        {
            if (!assignedHero.purchasedItems.Contains(itemName))
                assignedHero.purchasedItems.Add(itemName);

            Debug.Log($"{assignedHero.heroName} purchased {itemName}");
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to purchase " + itemName);
        }
    }

    public void OnEquip()
    {
        if (!assignedHero.equippedItems.Contains(itemName))
        {
            assignedHero.equippedItems.Add(itemName);
            Debug.Log($"{assignedHero.heroName} equipped {itemName}");
        }
    }

    private void UpdateUI()
    {
        bool purchased = assignedHero.purchasedItems.Contains(itemName);

        buyButton.gameObject.SetActive(!purchased);
        equipButton.gameObject.SetActive(purchased);
    }
}
