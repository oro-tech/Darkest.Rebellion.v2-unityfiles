using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlot_Shop : MonoBehaviour
{
    public Image portraitImage;     // Drag in Inspector
    public TMP_Text nameText;       // Drag in Inspector
    public TMP_Text roleText;       // Drag in Inspector
    public TMP_Text priceText;      // Drag in Inspector
    public Button heroButton;       // Drag in Inspector

    private HeroData data;
    private ShopManager shopManager;

    void Awake()
    {
        // Fallback: auto-find a Button if not assigned
        if (heroButton == null)
        {
            heroButton = GetComponentInChildren<Button>();
            if (heroButton == null)
                Debug.LogError("[HeroSlot_Shop] No Button found in prefab!");
        }
    }

    public void Setup(HeroData hero, object manager)
    {
        data = hero;

        if (portraitImage != null)
            portraitImage.sprite = hero.portrait;

        if (nameText != null)
            nameText.text = hero.heroName;

        if (roleText != null)
            roleText.text = hero.role.ToString();

        if (priceText != null)
            priceText.text = hero.price + " Coins";

        if (manager is ShopManager)
            shopManager = (ShopManager)manager;

        if (heroButton != null)
        {
            heroButton.onClick.RemoveAllListeners();
            heroButton.onClick.AddListener(() => shopManager.OnHeroSelected(data));
        }
    }

    public HeroData GetHeroData() => data;

    public void SetSelectable(bool isSelectable)
    {
        if (heroButton != null)
            heroButton.interactable = isSelectable;
    }

    public void UpdatePriceText(string text)
    {
        if (priceText != null)
            priceText.text = text;
    }
}
