using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SkillButtonUI : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TMP_Text skillNameText;
    public TMP_Text cooldownText;

    private SkillConfig skillConfig;
    private Character activeCharacter;
    private TurnManager turnManager;
    private Animator animator;
    private Button button;

    private bool isOnCooldown = false;
    private bool isExecuting = false;
    private float cooldownRemaining;

    // ✅ Finds inactive cutscenes in the scene
    private GameObject FindInactiveCutscene(string cutsceneName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == cutsceneName)
            {
                if (obj.scene.IsValid() && obj.scene.isLoaded)
                    return obj;
            }
        }

        return null;
    }

    // ✅ Called by TurnManager to initialize this button
    public void Setup(SkillConfig skill, Character character, TurnManager manager)
    {
        skillConfig = skill;
        activeCharacter = character;
        turnManager = manager;
        animator = activeCharacter.GetComponent<Animator>();
        button = GetComponent<Button>();

        // Prevent duplicate listeners
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonPressed);

        if (icon != null && skillConfig.icon != null)
            icon.sprite = skillConfig.icon;

        if (skillNameText != null)
            skillNameText.text = skillConfig.skillName;

        if (cooldownText != null)
            cooldownText.gameObject.SetActive(false);
    }

    // ✅ When skill button is pressed
    public void OnButtonPressed()
    {
        if (isOnCooldown || isExecuting) return;

        // Try to play a cutscene if defined
        if (!string.IsNullOrEmpty(skillConfig.cutsceneName))
        {
            GameObject cutsceneObject = FindInactiveCutscene(skillConfig.cutsceneName);
            if (cutsceneObject != null)
            {
                StartCoroutine(PlayCutsceneAndCooldownRoutine(cutsceneObject));
                return;
            }
            else
            {
                Debug.LogWarning($"❌ Cutscene '{skillConfig.cutsceneName}' not found in scene.");
            }
        }

        // Otherwise play animation and execute skill normally
        StartCoroutine(ExecuteSkillAndCooldown());
    }

    // ✅ Executes the skill normally (no cutscene)
    private IEnumerator ExecuteSkillAndCooldown()
    {
        isExecuting = true;
        button.interactable = false;

        if (animator != null && !string.IsNullOrEmpty(skillConfig.animationTrigger))
            animator.SetTrigger(skillConfig.animationTrigger);

        yield return new WaitForSeconds(0.2f); // short delay before applying skill

        ExecuteSkillLogic();
        StartCoroutine(CooldownRoutine());

        isExecuting = false;
    }

    // ✅ Plays a cutscene and then executes the skill logic
    private IEnumerator PlayCutsceneAndCooldownRoutine(GameObject cutsceneObject)
    {
        isExecuting = true;
        button.interactable = false;

        cutsceneObject.SetActive(true);
        PlayableDirector director = cutsceneObject.GetComponent<PlayableDirector>();

        if (director != null)
        {
            Debug.Log($"🎬 Playing cutscene: {skillConfig.cutsceneName}");
            director.Play();
            yield return new WaitForSeconds((float)director.duration);
        }

        cutsceneObject.SetActive(false);
        Debug.Log($"✅ Cutscene finished: {skillConfig.cutsceneName}");

        ExecuteSkillLogic();
        StartCoroutine(CooldownRoutine());

        isExecuting = false;
    }

    // ✅ Executes the actual skill logic — auto-targets the correct side
    private void ExecuteSkillLogic()
    {
        if (turnManager == null)
        {
            Debug.LogError("⚠️ TurnManager reference missing in SkillButtonUI!");
            return;
        }

        // Determine which skill is being used
        if (skillConfig == activeCharacter.heroData.skill1Config)
        {
            Debug.Log($"▶️ Executing Skill 1: {skillConfig.skillName}");
            turnManager.OnSkill1();
        }
        else if (skillConfig == activeCharacter.heroData.skill2Config)
        {
            Debug.Log($"▶️ Executing Skill 2: {skillConfig.skillName}");
            turnManager.OnSkill2();
        }
        else
        {
            Debug.LogWarning("⚠️ Unknown skill triggered in SkillButtonUI.");
        }
    }

    // ✅ Handles cooldown visuals and logic
    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        cooldownRemaining = skillConfig.cooldownTime;
        button.interactable = false;

        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(true);
            cooldownText.text = $"{cooldownRemaining:F0}s";
        }

        Debug.Log($"⏳ Cooldown started for {skillConfig.skillName} ({skillConfig.cooldownTime}s)");

        while (cooldownRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            cooldownRemaining -= 1f;

            if (cooldownText != null)
                cooldownText.text = $"{Mathf.Max(0, cooldownRemaining):F0}s";
        }

        isOnCooldown = false;

        // ✅ Only re-enable if it’s currently the player’s turn
        if (turnManager != null && turnManager.IsPlayerTurn())
        {
            button.interactable = true;
        }

        if (cooldownText != null)
            cooldownText.gameObject.SetActive(false);

        Debug.Log($"✅ {skillConfig.skillName} is ready again!");
    }
}
