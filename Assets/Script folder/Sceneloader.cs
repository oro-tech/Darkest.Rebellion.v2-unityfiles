using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneloader : MonoBehaviour
{
    public string Scene;
    // Call this method to manually load the HesoList scene
   public void testScene()
    {
        // Load the HesoList scene
        SceneManager.LoadScene(Scene);

        // Log to confirm the scene load attempt
        Debug.Log("Attempting to load HesoList scene...");
    }
}
