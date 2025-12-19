using UnityEngine;
using UnityEngine.SceneManagement;

public class Campaign_sceneController : MonoBehaviour
{
    public string teamSelectionSceneName1 = "TeamSelectionScene";  // The Team Selection Scene
    public string teamSelectionSceneName2 = "TeamSelection2Scene";  // The Team Selection Scene
    public string lobbySceneName = "Lobby_Scene";  // The Lobby Scene

    // Load Team Selection Scene (additive)
    public void LoadTeamSelection()
    {
        // Load the Team Selection scene additively
        SceneManager.LoadScene(teamSelectionSceneName1, LoadSceneMode.Additive);
    }
    public void LoadTeamSelection2()
    {
        // Load the Team Selection scene additively
        SceneManager.LoadScene(teamSelectionSceneName2, LoadSceneMode.Additive);
    }

    // Return to Lobby Scene
    public void ReturnToLobby()
    {
        // Unload the current scene (if needed) and load the Lobby Scene
        SceneManager.LoadScene(lobbySceneName, LoadSceneMode.Single);
    }


}

