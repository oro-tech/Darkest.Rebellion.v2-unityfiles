using System.Collections.Generic;
using UnityEngine;

public class SelectedTeamStorage : MonoBehaviour
{
    public static SelectedTeamStorage Instance;

    public List<HeroData> selectedHeroes = new List<HeroData>();
    public int maxTeamSize = 1;  // Allow only one hero to be selected
    public bool heroLocked = false; // Flag to indicate if the hero selection is locked

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddHero(HeroData hero)
    {
        if (selectedHeroes.Count >= maxTeamSize || selectedHeroes.Contains(hero) || heroLocked)
            return false;

        selectedHeroes.Add(hero);
        heroLocked = true; // Lock hero selection after one hero is selected
        return true;
    }

    // Remove a hero from the selected team
    public bool RemoveHero(HeroData hero)
    {
        if (selectedHeroes.Contains(hero))
        {
            selectedHeroes.Remove(hero);
            heroLocked = false; // Unlock hero selection once a hero is removed
            return true;
        }
        return false; // Hero not found in the selected list
    }

    public void ClearTeam()
    {
        selectedHeroes.Clear();
        heroLocked = false;  // Unlock the hero selection when clearing the team
    }
}
