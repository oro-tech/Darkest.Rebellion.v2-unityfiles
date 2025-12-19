using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionsSeen = 0;

    // Coin reward per correct answer
    public int coinPerCorrectAnswer = 1;

    // Getter
    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    // Setter
    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    public int GetQuestionsSeen()
    {
        return questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt(correctAnswers / (float)questionsSeen * 100);
    }

    // Calculate total earned coins for this quiz
    public int GetEarnedCoins()
    {
        return correctAnswers * coinPerCorrectAnswer;
    }

    // Add earned coins to PlayerPrefs for shop
    public void SaveCoinsToShop()
    {
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 500);
        int earnedCoins = GetEarnedCoins();

        currentCoins += earnedCoins;

        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();

        Debug.Log($"[QuizScoreKeeper] Earned {earnedCoins} coins, Total Coins: {currentCoins}");
    }
}
