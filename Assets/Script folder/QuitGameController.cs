using UnityEngine;

public class QuitGameController : MonoBehaviour
{
    // Quit the game
    public void QuitGame()
    {
        // Quit the application
#if UNITY_EDITOR
        // If we are in the editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If we are in a built game, quit the application
        Application.Quit();
#endif
    }
}
