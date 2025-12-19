using UnityEngine;
using System.Collections.Generic;

public class HeroListingManager : MonoBehaviour
{
    [Header("References")]
    public Transform contentParent;
    public GameObject heroSlotPrefab;
    public List<HeroData> allHeroes;
    public HeroDetailPanel detailPanel; // Required!

    void Start()
    {
        foreach (var hero in allHeroes)
        {
            GameObject slotGO = Instantiate(heroSlotPrefab, contentParent);
            HeroSlotUI ui = slotGO.GetComponent<HeroSlotUI>();
            ui.Setup(hero, this, HeroSlotUI.Context.HeroListing); // ✅ proper call
        }
    }


    // OnHeroSelected method to display hero details in the panel
    public void OnHeroSelected(HeroData selectedHero)
    {
        Debug.Log($"Hero clicked: {selectedHero.heroName}");
        detailPanel.DisplayHero(selectedHero); // Display hero in the detail panel
    }
}
