using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneController : MonoBehaviour
{
    public GameObject heroListingPanel;  // The Hero Listing Panel (UI)
    public string heroListingSceneName = "HeroList";  // The Hero Listing Scene name
    public string teamSelectionSceneName = "CampaignMap_Scene";  // The Team Selection Scene

    void Start()
    {
        // Initially, make sure the Hero Listing Panel is hidden
        heroListingPanel.SetActive(false);
    }

    // Show the Hero Listing Panel as an overlay
    public void ShowHeroListing()
    {
        // Load the Hero Listing scene additively (so it doesn't replace the current scene)
        SceneManager.LoadSceneAsync(heroListingSceneName, LoadSceneMode.Additive).completed += OnHeroListSceneLoaded;

        // Show the Hero Listing UI Panel (overlay it on the Lobby)
        heroListingPanel.SetActive(true);
    }

    private void OnHeroListSceneLoaded(AsyncOperation asyncOperation)
    {
        // This is called when the Hero List scene has finished loading.
        Debug.Log("Hero List Scene Loaded");
        // Perform any additional setup or checks after the scene is loaded.
    }

    // Close the Hero Listing Panel (hide overlay)
    public void CloseHeroListing()
    {
        // Unload the Hero Listing scene (if needed, depending on your use case)
        SceneManager.UnloadSceneAsync(heroListingSceneName);

        // Hide the Hero Listing Panel (overlay disappears)
        heroListingPanel.SetActive(false);
    }

    // Load Team Selection Scene (additive)
    public void LoadTeamSelection()
    {
        // Load the Team Selection scene additively
        SceneManager.LoadSceneAsync(teamSelectionSceneName, LoadSceneMode.Additive).completed += OnTeamSelectionSceneLoaded;
    }

    private void OnTeamSelectionSceneLoaded(AsyncOperation asyncOperation)
    {
        // This is called when the Team Selection scene has finished loading.
        Debug.Log("Team Selection Scene Loaded");
        // Perform any additional setup or checks after the scene is loaded.
    }
}
