using UnityEngine;
using TMPro;

public class QuizEndScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    // NEW
    [SerializeField] private TextMeshProUGUI earnedCoinsText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    private const string HighScoreKey = "HighScore"; // Key for PlayerPrefs
    private QuizScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<QuizScoreKeeper>();
    }

    void Update()
    {
        // Developer Hotkey: Reset all quiz data
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetQuizData();
        }
    }

    public void ShowFinalScore()
    {
        int currentScore = scoreKeeper.GetCorrectAnswers();
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // Save high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        // Calculate coins before saving
        int earnedCoins = scoreKeeper.GetEarnedCoins();

        // Save earned coins to shop
        scoreKeeper.SaveCoinsToShop();

        // Get updated total coins
        int totalCoins = PlayerPrefs.GetInt("PlayerCoins", 500);

        // Update UI
        finalScoreText.text = "Your Score: " + currentScore;
        highScoreText.text = "Highest Score: " + highScore;

        // NEW UI updates
        if (earnedCoinsText != null)
            earnedCoinsText.text = "Coins Earned: " + earnedCoins;

        if (totalCoinsText != null)
            totalCoinsText.text = "Total Coins: " + totalCoins;
    }

    private void ResetQuizData()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
       
        PlayerPrefs.Save();

        Debug.Log("[RESET] All quiz data cleared (High Score + Coins).");
    }

}
