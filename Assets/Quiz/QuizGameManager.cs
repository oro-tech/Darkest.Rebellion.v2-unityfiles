using UnityEngine;

public class QuizGameManager : MonoBehaviour
{
    QuizScript quiz;
    QuizEndScreen endScreen;
    bool hasShownEnd = false;  // ✅ Prevents multiple calls

    void Awake()
    {
        quiz = FindObjectOfType<QuizScript>();
        endScreen = FindObjectOfType<QuizEndScreen>();
    }

    void Start()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if (quiz.isComplete && !hasShownEnd)  // ✅ Only once
        {
            quiz.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(true);
            endScreen.ShowFinalScore();

            hasShownEnd = true; // ✅ Stop repeating
        }
    }

    public void OnReplayLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
