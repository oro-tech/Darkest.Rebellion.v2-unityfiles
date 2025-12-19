using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public string lobbySceneName = "Lobby_Scene";

    public void ReturnToLobby()
    {
        // Clear any persistent objects
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj != null && obj.scene.name != null && obj.scene.name != lobbySceneName)
            {
                if (obj.hideFlags == HideFlags.None && obj.transform.parent == null)
                {
                    Destroy(obj);
                }
            }
        }

        // Load Lobby as the only active scene
        SceneManager.LoadScene(lobbySceneName, LoadSceneMode.Single);
    }
}
