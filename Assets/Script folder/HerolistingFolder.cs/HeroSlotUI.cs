using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlotUI : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text roleText;
    public Button heroButton;

    private HeroData data;
    private TeamSelectionManager teamSelectionManager;
    private HeroListingManager heroListingManager;

    public enum Context
    {
        TeamSelection,
        HeroListing
    }

    private Context context;

    void Awake()
    {
        if (heroButton == null)
        {
            heroButton = GetComponentInChildren<Button>();
            if (heroButton == null)
                Debug.LogError("[HeroSlotUI] No Button found in prefab!");
        }
    }

    public void Setup(HeroData hero, object manager, Context ctx)
    {
        data = hero;
        context = ctx;

        if (portraitImage != null)
            portraitImage.sprite = hero.portrait;

        if (nameText != null)
            nameText.text = hero.heroName;

        if (roleText != null)
            roleText.text = hero.role.ToString();

        if (manager is TeamSelectionManager)
            teamSelectionManager = (TeamSelectionManager)manager;
        else if (manager is HeroListingManager)
            heroListingManager = (HeroListingManager)manager;

        if (heroButton != null)
        {
            heroButton.onClick.RemoveAllListeners();

            if (context == Context.TeamSelection && teamSelectionManager != null)
                heroButton.onClick.AddListener(() => teamSelectionManager.OnHeroSelected(data));
            else if (context == Context.HeroListing && heroListingManager != null)
                heroButton.onClick.AddListener(() => heroListingManager.OnHeroSelected(data));
        }
    }

    public HeroData GetHeroData() => data;

    /// <summary>
    /// Allows TeamSelectionManager to enable/disable slot (for purchased check).
    /// </summary>
    public void SetSelectable(bool isSelectable)
    {
        if (heroButton != null)
            heroButton.interactable = isSelectable;
    }
}
