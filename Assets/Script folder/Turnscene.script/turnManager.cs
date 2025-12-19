using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private HeroData heroData;

    [Header("UI References")]
    public TMP_Text turnText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;

    [Header("Mana UI")]
    public TMP_Text playerManaText;
    public TMP_Text enemyManaText;

    [Header("Turn Timer UI")]
    public TMP_Text turnTimerText;

    [Header("Battle Result UI")]
    public GameObject battleResultPanel;
    public TMP_Text resultText;
    public Button backButton;
    public Button nextOrRetryButton;

    [Header("Config References")]
    public TurnConfig config;

    [Header("Skill Slot Spawners")]
    public Transform skillSlot1;
    public Transform skillSlot2;

    [Header("Spawning")]
    public Transform[] playerSpawnSlots;
    public Transform[] enemySpawnSlots;
    public GameObject[] enemyPrefabs;

    [System.NonSerialized] public List<Character> playerTeam = new List<Character>();
    [System.NonSerialized] public List<Character> enemyTeam = new List<Character>();

    [Header("Coin Reward System")]
    public TMP_Text rewardCoinsText;  // UI text showing the reward after battle
    public int winRewardCoins = 200;  // Amount of coins gained when winning
    public int loseRewardCoins = 75;  // Amount of coins gained when losing


    public enum TurnPhase { Player, Enemy }
    public TurnPhase currentTurn = TurnPhase.Player;

    private int playerActionCount = 0;
    private int enemyActionCount = 0;
    private int round = 1;

    private Coroutine turnTimerCoroutine;
    private int currentCampaignLevel = 1;

    private List<Button> activeSkillButtons = new List<Button>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentCampaignLevel = PlayerPrefs.GetInt("CurrentCampaignLevel", 1);

        SpawnPlayerTeam();
        SpawnEnemyTeam();
        UpdateUI();

        if (battleResultPanel != null)
            battleResultPanel.SetActive(false);

        StartNextTurn();
    }

    void SpawnPlayerTeam()
    {
        var heroes = SelectedTeamStorage.Instance.selectedHeroes;

        for (int i = 0; i < heroes.Count && i < playerSpawnSlots.Length; i++)
        {
            heroData = heroes[i];
            GameObject go = Instantiate(heroData.battlePrefab, playerSpawnSlots[i].position, Quaternion.identity);
            Character c = go.GetComponentInChildren<Character>();

            if (c == null)
            {
                Debug.LogError($"[ERROR] Character.cs missing on prefab {go.name}");
                continue;
            }

            CharacterPosition position = CharacterPosition.Front;
            if (i == 1) position = CharacterPosition.Middle;
            else if (i == 2) position = CharacterPosition.Back;

            c.Init(heroData.heroName, heroData.maxHealth, heroData.role, heroData.damageReduction,
                   heroData.damageMultiplier, heroData.healingEffectiveness,
                   heroData.specialSkill1, heroData.specialSkill2,
                   heroData.attackConfig, position, heroData);

            playerTeam.Add(c);
        }
    }

    void SpawnEnemyTeam()
    {
        GameObject go1 = Instantiate(enemyPrefabs[0], enemySpawnSlots[0].position, Quaternion.identity);
        Character c1 = go1.GetComponentInChildren<Character>();
        c1.position = CharacterPosition.Front;
        enemyTeam.Add(c1);
    }

    void StartNextTurn()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        // ✅ Check immediately before starting next turn
        CheckBattleResult();

        if (battleResultPanel != null && battleResultPanel.activeSelf)
            return;

        UpdateUI();

        if (currentTurn == TurnPhase.Player)
        {
            playerActionCount = 0;
            foreach (var player in playerTeam)
                if (player.isAlive) player.ResetSkillUsage();

            EnableSkillButtons();
            turnText.text = $"Round {round} - Player Turn";
            if (turnTimerText != null)
                turnTimerText.text = $"Time Left: {config.turnTimeLimit:F0}s";

            turnTimerCoroutine = StartCoroutine(PlayerTurnTimer());
        }
        else
        {
            enemyActionCount = 0;
            foreach (var enemy in enemyTeam)
                if (enemy.isAlive) enemy.ResetSkillUsage();

            DisableButtons();
            turnText.text = $"Round {round} - Enemy Turn";
            if (turnTimerText != null)
                turnTimerText.text = $"Enemy Acting... ({config.turnTimeLimit:F0}s)";

            turnTimerCoroutine = StartCoroutine(EnemyTurnTimer());
        }
    }

    private void UnlockNextCampaign()
    {
        int currentCampaignLevel = PlayerPrefs.GetInt("CurrentCampaignLevel", 1);
        int unlockedCampaigns = PlayerPrefs.GetInt("UnlockedCampaigns", 1);

        int nextToUnlock = currentCampaignLevel + 1;

        // Fix: Only unlock if current campaign is completed
        if (nextToUnlock > unlockedCampaigns)
        {
            PlayerPrefs.SetInt("UnlockedCampaigns", nextToUnlock);
            PlayerPrefs.Save();
            Debug.Log($"✅ Unlocked next campaign level {nextToUnlock}");
        }
        else
        {
            Debug.Log($"Level {nextToUnlock} already unlocked (unlockedCampaigns = {unlockedCampaigns}).");
        }
    }


    void UpdateUI()
    {
        string playerNames = string.Join(" | ", playerTeam.Select(p => $"{p.characterName}: {p.currentHealth}"));
        playerHealthText.text = playerNames;

        string enemyNames = string.Join(" | ", enemyTeam.Select(e => $"{e.characterName}: {e.currentHealth}"));
        enemyHealthText.text = enemyNames;

        if (playerManaText != null)
        {
            string playerMana = string.Join(" | ", playerTeam.Select(p => $"{p.characterName}: {p.currentMana}/{p.maxMana} "));
            playerManaText.text = playerMana;
        }

        if (enemyManaText != null)
        {
            string enemyMana = string.Join(" | ", enemyTeam.Select(e => $"{e.characterName}: {e.currentMana}/{e.maxMana} "));
            enemyManaText.text = enemyMana;
        }
    }

    public void OnSkill1()
    {
        if (currentTurn != TurnPhase.Player || playerActionCount >= config.maxActionsPerTurn) return;

        Character activePlayer = playerTeam.FirstOrDefault(p => p.isAlive);
        Character target = enemyTeam.FirstOrDefault(e => e.isAlive);

        if (activePlayer == null || target == null) return;

        if (activePlayer.heroData?.skill1Config != null)
        {
            Debug.Log($"⚔️ {activePlayer.characterName} used Skill 1 on {target.characterName}");
            activePlayer.ApplySkillEffect(activePlayer.heroData.skill1Config, target);
        }

        playerActionCount++;
        UpdateUI();
        CheckBattleResult(); // ✅ Immediate result check after damage

        if (playerActionCount >= config.maxActionsPerTurn)
            EndCurrentTurn();
    }

    public void OnSkill2()
    {
        if (currentTurn != TurnPhase.Player || playerActionCount >= config.maxActionsPerTurn) return;

        Character activePlayer = playerTeam.FirstOrDefault(p => p.isAlive);
        Character target = enemyTeam.FirstOrDefault(e => e.isAlive);

        if (activePlayer == null || target == null) return;

        if (activePlayer.heroData?.skill2Config != null)
        {
            Debug.Log($"🔥 {activePlayer.characterName} used Skill 2 on {target.characterName}");
            activePlayer.ApplySkillEffect(activePlayer.heroData.skill2Config, target);
        }

        playerActionCount++;
        UpdateUI();
        CheckBattleResult(); // ✅ Immediate result check after damage

        if (playerActionCount >= config.maxActionsPerTurn)
            EndCurrentTurn();
    }

    void EndCurrentTurn()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        currentTurn = (currentTurn == TurnPhase.Player) ? TurnPhase.Enemy : TurnPhase.Player;
        StartNextTurn();
    }

    void EnableSkillButtons()
    {
        foreach (Transform child in skillSlot1) Destroy(child.gameObject);
        foreach (Transform child in skillSlot2) Destroy(child.gameObject);
        activeSkillButtons.Clear();

        if (heroData.SkillButtonPrefab1 != null && skillSlot1 != null)
        {
            GameObject skillButton1 = Instantiate(heroData.SkillButtonPrefab1, skillSlot1);
            SkillButtonUI ui = skillButton1.GetComponent<SkillButtonUI>();
            if (ui != null)
            {
                ui.Setup(heroData.skill1Config, playerTeam[0], this);
                Button btn = skillButton1.GetComponent<Button>();
                if (btn != null)
                {
                    activeSkillButtons.Add(btn);
                    btn.interactable = true;
                }
            }
        }

        if (heroData.SkillButtonPrefab2 != null && skillSlot2 != null)
        {
            GameObject skillButton2 = Instantiate(heroData.SkillButtonPrefab2, skillSlot2);
            SkillButtonUI ui = skillButton2.GetComponent<SkillButtonUI>();
            if (ui != null)
            {
                ui.Setup(heroData.skill2Config, playerTeam[0], this);
                Button btn = skillButton2.GetComponent<Button>();
                if (btn != null)
                {
                    activeSkillButtons.Add(btn);
                    btn.interactable = true;
                }
            }
        }
    }

    void DisableButtons()
    {
        foreach (var btn in activeSkillButtons)
        {
            if (btn == null) continue;
            btn.interactable = false;
            ColorBlock cb = btn.colors;
            cb.disabledColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            btn.colors = cb;
        }
    }

    IEnumerator PlayerTurnTimer()
    {
        float elapsed = 0f;
        float remaining = config.turnTimeLimit;

        while (elapsed < config.turnTimeLimit)
        {
            if (turnTimerText != null)
                turnTimerText.text = $"Time Left: {remaining - elapsed:F0}s";

            yield return new WaitForSeconds(1f);
            elapsed++;
        }

        if (turnTimerText != null)
            turnTimerText.text = "Time's up!";

        EndCurrentTurn();
    }

    IEnumerator EnemyTurnTimer()
    {
        float elapsed = 0f;
        float remaining = config.turnTimeLimit;
        bool enemyActionsFinished = false;

        // Run enemy actions concurrently
        StartCoroutine(EnemyActionsRoutine(() => enemyActionsFinished = true));

        while (elapsed < config.turnTimeLimit)
        {
            if (turnTimerText != null)
                turnTimerText.text = $"Enemy Acting... ({remaining - elapsed:F0}s)";

            yield return new WaitForSeconds(1f);
            elapsed++;

            if (enemyActionsFinished)
                break;
        }

        if (turnTimerText != null)
            turnTimerText.text = "Enemy Done!";

        EndCurrentTurn();
    }

    private IEnumerator EnemyActionsRoutine(System.Action onComplete)
    {
        enemyActionCount = 0;

        foreach (var enemy in enemyTeam)
        {
            if (!enemy.isAlive || enemyActionCount >= config.maxActionsPerTurn) continue;

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                yield return StartCoroutine(ai.ExecuteTurn(playerTeam, () =>
                {
                    enemyActionCount++;
                    UpdateUI();
                    CheckBattleResult(); // ✅ Immediate check after each enemy action
                }));
            }

            if (enemyActionCount >= config.maxActionsPerTurn)
                break;
        }

        onComplete?.Invoke();
    }

    private bool AllCharactersDefeated(List<Character> team)
    {
        return team.All(c => !c.isAlive);
    }

    public Character GetTargetBasedOnPosition(Character attacker)
    {
        switch (attacker.position)
        {
            case CharacterPosition.Front:
                return enemyTeam.FirstOrDefault(e => e.position == CharacterPosition.Front);
            case CharacterPosition.Middle:
                return enemyTeam.FirstOrDefault(e => e.position == CharacterPosition.Front || e.position == CharacterPosition.Middle);
            case CharacterPosition.Back:
                return enemyTeam.FirstOrDefault(e => e.position == CharacterPosition.Back);
            default:
                return null;
        }
    }

    // ✅ New function for instant battle result checking
    public void CheckBattleResult()
    {
        if (battleResultPanel != null && battleResultPanel.activeSelf) return;

        if (AllCharactersDefeated(playerTeam))
        {
            ShowBattleResult(false);
            return;
        }

        if (AllCharactersDefeated(enemyTeam))
        {
            ShowBattleResult(true);
            UnlockNextCampaign();
            return;
        }
    }

    private void ShowBattleResult(bool playerWon)
    {
        DisableButtons();

        // Show result panel
        if (battleResultPanel != null)
            battleResultPanel.SetActive(true);

        if (resultText != null)
            resultText.text = playerWon ? "VICTORY" : "DEFEAT!";

        // --- 🪙 Reward Coins ---
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 2000); // Default 2000 if not set
        int reward = playerWon ? winRewardCoins : loseRewardCoins;

        currentCoins += reward;

        // Save updated coin count
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();

        // Display reward on UI
        if (rewardCoinsText != null)
        {
            rewardCoinsText.text = $"+{reward} ";
        }

        Debug.Log($"🪙 Battle ended. PlayerWon = {playerWon}. Reward = {reward}. Total Coins = {currentCoins}");

        // --- UI Buttons ---
        if (backButton != null)
            backButton.gameObject.SetActive(true);

        if (nextOrRetryButton != null)
        {
            nextOrRetryButton.gameObject.SetActive(true);
            nextOrRetryButton.GetComponentInChildren<TMP_Text>().text =
                playerWon ? "Next" : "Retry";
        }
    }


    public bool IsPlayerTurn() => currentTurn == TurnPhase.Player;
}
