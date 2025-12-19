using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController_1 : MonoBehaviour
{
    [Header("Instruction Settings")]
    public string destinationScene;   // Name of the next scene to load
    public float autoLoadDelay = 0f;  // 0 = require button click, >0 = auto load after delay

    private bool isReadyToLoad = false;

    void Start()
    {
        if (autoLoadDelay > 0)
        {
            Invoke(nameof(LoadNextScene), autoLoadDelay);
        }
    }

    public void ShowContinueButton()
    {
        isReadyToLoad = true;
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(destinationScene))
        {
            SceneManager.LoadScene(destinationScene);
        }
        else
        {
            Debug.LogWarning("Destination scene not set in SceneLoader!");
        }
    }
}
