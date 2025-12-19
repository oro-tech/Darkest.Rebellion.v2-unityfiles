using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelectionManager : MonoBehaviour
{
    [Header("References")]
    public Transform contentParent;          // The container for hero slots in the shop
    public GameObject heroSlotPrefab;        // Prefab for the hero slot UI
    public Transform selectedTeamPanel;      // Panel where selected heroes are shown
    public GameObject teamSlotPrefab;        // Prefab for the selected team slots
    public Button startBattleButton;         // Start battle button
    public Button clearSelectionButton;      // Button to clear selection
    public List<HeroData> allHeroes;         // List of all heroes

    [Header("Role Icons")]
    public Sprite tankIcon;
    public Sprite damageIcon;
    public Sprite supportIcon;

    [Header("Hotkey Settings")]
    public KeyCode clearSelectionKey = KeyCode.C;     // Press C to clear selection
    public KeyCode startBattleKey = KeyCode.Return;   // Press Enter to start battle

    private HeroData selectedHero = null;             // Track the selected hero
    private HeroPurchaseManager heroPurchaseManager;  // Reference to purchase manager

    void Start()
    {
        Debug.Log("[TeamSelectionManager] Start() called");

        startBattleButton.interactable = false;
        SelectedTeamStorage.Instance.ClearTeam();  // Reset team selection

        heroPurchaseManager = FindObjectOfType<HeroPurchaseManager>();
        Debug.Log($"[TeamSelectionManager] heroPurchaseManager found? {(heroPurchaseManager != null)}");

        if (heroSlotPrefab == null)
            Debug.LogError("[TeamSelectionManager] ERROR: heroSlotPrefab is not assigned in Inspector!");

        if (contentParent == null)
            Debug.LogError("[TeamSelectionManager] ERROR: contentParent is not assigned in Inspector!");

        if (allHeroes == null || allHeroes.Count == 0)
            Debug.LogWarning("[TeamSelectionManager] WARNING: allHeroes list is empty or not assigned!");

        // Populate the shop with hero slots
        foreach (var hero in allHeroes)
        {
            Debug.Log($"[TeamSelectionManager] Processing hero: {(hero != null ? hero.heroName : "NULL HERO")}");

            if (hero == null)
            {
                Debug.LogError("[TeamSelectionManager] ERROR: Null hero in allHeroes list!");
                continue;
            }

            GameObject slotGO = Instantiate(heroSlotPrefab, contentParent);
            Debug.Log($"[TeamSelectionManager] Instantiated slot for {hero.heroName}");

            HeroSlotUI slotUI = slotGO.GetComponent<HeroSlotUI>();
            if (slotUI == null)
            {
                Debug.LogError("[TeamSelectionManager] ERROR: heroSlotPrefab is missing HeroSlotUI component!");
                continue;
            }

            slotUI.Setup(hero, this, HeroSlotUI.Context.TeamSelection);
            Debug.Log($"[TeamSelectionManager] Setup completed for {hero.heroName}");

            // Role icon setup
            Image roleIcon = slotUI.transform.Find("RoleIcon")?.GetComponent<Image>();
            if (roleIcon != null)
            {
                roleIcon.sprite = GetRoleIcon(hero.role);
                roleIcon.gameObject.SetActive(true);
            }

            // Disable hero if not purchased
            if (heroPurchaseManager == null)
            {
                Debug.LogError("[TeamSelectionManager] ERROR: heroPurchaseManager not found in scene!");
            }
            else if (!heroPurchaseManager.IsHeroPurchased(hero))
            {
                slotUI.SetSelectable(false);
                Debug.Log($"[TeamSelectionManager] Hero {hero.heroName} disabled (not purchased)");
                continue;
            }

            // Lock check
            if (SelectedTeamStorage.Instance.heroLocked && !SelectedTeamStorage.Instance.selectedHeroes.Contains(hero))
            {
                slotUI.SetSelectable(false);
                Debug.Log($"[TeamSelectionManager] Hero {hero.heroName} locked (team already chosen)");
            }
        }

        // Button click listeners
        startBattleButton.onClick.AddListener(StartBattle);
        clearSelectionButton.onClick.AddListener(ClearHeroSelection);

        Debug.Log("[TeamSelectionManager] Start() finished setup");
    }

    // Handle hero selection
    // Handle hero selection
    public void OnHeroSelected(HeroData hero)
    {
        if (SelectedTeamStorage.Instance.AddHero(hero))
        {
            selectedHero = hero;

            GameObject slot = Instantiate(teamSlotPrefab, selectedTeamPanel);

            TMP_Text nameText = slot.GetComponentInChildren<TMP_Text>();
            Image roleIcon = slot.transform.Find("RoleIcon")?.GetComponent<Image>();
            Image heroIcon = slot.transform.Find("HeroIcon")?.GetComponent<Image>(); // 🔹 new reference

            if (nameText != null)
                nameText.text = hero.heroName;

            if (roleIcon != null)
            {
                roleIcon.sprite = GetRoleIcon(hero.role);
                roleIcon.gameObject.SetActive(true);
            }

            // ✅ Set the hero portrait dynamically
            if (heroIcon != null && hero.portrait != null)
            {
                heroIcon.sprite = hero.portrait;
                heroIcon.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[TeamSelectionManager] No hero icon found for {hero.heroName}!");
            }

            // Lock all hero slots after selection
            foreach (var slotUI in contentParent.GetComponentsInChildren<HeroSlotUI>())
            {
                if (slotUI != null)
                    slotUI.SetSelectable(false);
            }

            startBattleButton.interactable = true;
        }
    }


    // Clear the selected hero and unlock all slots
    public void ClearHeroSelection()
    {
        if (selectedHero != null)
        {
            Debug.Log("[TeamSelectionManager] Clearing selected hero.");
            SelectedTeamStorage.Instance.RemoveHero(selectedHero);
            selectedHero = null;

            foreach (Transform child in selectedTeamPanel)
                Destroy(child.gameObject);

            foreach (var slotUI in contentParent.GetComponentsInChildren<HeroSlotUI>())
            {
                if (slotUI != null)
                {
                    if (heroPurchaseManager.IsHeroPurchased(slotUI.GetHeroData()))
                        slotUI.SetSelectable(true);
                    else
                        slotUI.SetSelectable(false);
                }
            }

            startBattleButton.interactable = false;
        }
        else
        {
            Debug.Log("[TeamSelectionManager] No hero selected to clear.");
        }
    }

    // Get the role icon sprite based on the hero's role
    Sprite GetRoleIcon(CharacterRole role)
    {
        switch (role)
        {
            case CharacterRole.Tank: return tankIcon;
            case CharacterRole.Damage: return damageIcon;
            case CharacterRole.Support: return supportIcon;
            default: return null;
        }
    }

    // Start battle depending on which selection scene we are in
    void StartBattle()
    {
        int count = SelectedTeamStorage.Instance.selectedHeroes.Count;

        if (count < 1)
        {
            Debug.LogWarning("Cannot start: no heroes selected.");
            return;
        }

        Debug.Log($"[TeamSelection] Starting battle with {count} selected hero(es).");

        string sceneToCheck = null;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.name == "TeamSelectionScene" || loadedScene.name == "TeamSelection2Scene")
            {
                sceneToCheck = loadedScene.name;
            }
        }

        if (sceneToCheck == "TeamSelectionScene")
        {
            SceneManager.LoadScene("BattleField_Scene");
        }
        else if (sceneToCheck == "TeamSelection2Scene")
        {
            SceneManager.LoadScene("BattleField2_Scene");
        }
        else
        {
            Debug.LogError("Unknown scene: unable to load the appropriate battle scene.");
        }
    }

    // ✅ Hotkey Support
    private void Update()
    {
        // Hotkey for clearing selection
        if (Input.GetKeyDown(clearSelectionKey))
        {
            Debug.Log($"[Hotkey] {clearSelectionKey} pressed → Clear Selection");
            ClearHeroSelection();
        }

        // Hotkey for starting battle
        if (Input.GetKeyDown(startBattleKey) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (startBattleButton.interactable)
            {
                Debug.Log($"[Hotkey] {startBattleKey} pressed → Start Battle");
                StartBattle();
            }
            else
            {
                Debug.Log($"[Hotkey] {startBattleKey} pressed but Start Battle is disabled.");
            }
        }
    }
}
