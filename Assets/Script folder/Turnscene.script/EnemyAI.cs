using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Character character; // The enemy this AI controls
    private TurnManager manager;
    private EnemyCutsceneManager cutsceneManager;

    private void Awake()
    {
        cutsceneManager = GetComponent<EnemyCutsceneManager>();
    }

    public void Init(TurnManager mgr)
    {
        manager = mgr;
    }

    // Main AI execution
    public IEnumerator ExecuteTurn(List<Character> playerTeam, System.Action onComplete)
    {
        if (character == null)
        {
            Debug.LogError("❌ Character not assigned in EnemyAI!");
            yield break;
        }

        if (playerTeam == null || playerTeam.Count == 0)
        {
            Debug.LogError("❌ Player team is null or empty.");
            yield break;
        }

        Debug.Log($"🧠 {character.characterName} is taking its turn...");
        yield return new WaitForSeconds(1.5f); // simulate thinking

        Character target = ChooseTarget(playerTeam);
        if (target == null)
        {
            Debug.LogWarning($"⚠️ No valid target found for {character.characterName}!");
            yield break;
        }

        // --- Skill 1 ---
        yield return ExecuteSkillWithCutscene(character.currentSkill1, target, 1);
        yield return new WaitForSeconds(1f);

        // --- Skill 2 ---
        yield return ExecuteSkillWithCutscene(character.currentSkill2, target, 2);

        onComplete?.Invoke();
    }

    private IEnumerator ExecuteSkillWithCutscene(SkillConfig skill, Character target, int skillIndex)
    {
        if (skill == null)
        {
            Debug.LogWarning($"⚠️ {character.characterName} tried to use a null skill!");
            yield break;
        }

        Debug.Log($"🧩 {character.characterName} preparing to use {skill.skillName} on {target.characterName}");

        // Step 1: Check EnemyCutsceneManager for target-specific cutscene ID
        string cutsceneId = cutsceneManager != null
            ? cutsceneManager.GetCutsceneId(target.characterName, skillIndex)
            : null;

        // Step 2: Play assigned cutscene GameObject
        if (!string.IsNullOrEmpty(cutsceneId))
        {
            GameObject cutsceneObj = FindCutsceneById(cutsceneId);
            if (cutsceneObj != null)
            {
                Debug.Log($"🎬 {character.characterName} playing cutscene '{cutsceneId}' (Skill {skillIndex})");
                yield return PlayCutsceneAndApplySkill(cutsceneObj, skill, target);
                yield break;
            }
            else
            {
                Debug.LogWarning($"⚠️ Cutscene with ID '{cutsceneId}' not found in scene!");
            }
        }

        // Step 3: Fallback to SkillConfig cutscene ID (if set)
        if (skill.useCutscene && !string.IsNullOrEmpty(skill.cutsceneName))
        {
            GameObject fallbackObj = FindCutsceneById(skill.cutsceneName);
            if (fallbackObj != null)
            {
                Debug.Log($"🎬 Playing fallback skill cutscene: {skill.cutsceneName}");
                yield return PlayCutsceneAndApplySkill(fallbackObj, skill, target);
                yield break;
            }
        }

        // Step 4: No cutscene found → play animation trigger instead
        Animator anim = character.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(skill.animationTrigger))
        {
            anim.SetTrigger(skill.animationTrigger);
        }

        yield return new WaitForSeconds(0.3f);
        character.ApplySkillEffect(skill, target);
        Debug.Log($"💥 {character.characterName} executed {skill.skillName} (no cutscene) on {target.characterName}");
    }

    private IEnumerator PlayCutsceneAndApplySkill(GameObject cutsceneObj, SkillConfig skill, Character target)
    {
        cutsceneObj.SetActive(true);

        Animator cutsceneAnim = cutsceneObj.GetComponent<Animator>();
        float cutsceneTime = 1.5f;

        if (cutsceneAnim != null)
        {
            AnimatorClipInfo[] clips = cutsceneAnim.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
                cutsceneTime = clips[0].clip.length;

            cutsceneAnim.Play(clips[0].clip.name, 0, 0);
        }

        yield return new WaitForSeconds(cutsceneTime);

        cutsceneObj.SetActive(false);
        character.ApplySkillEffect(skill, target);
        Debug.Log($"✅ Cutscene '{cutsceneObj.name}' finished — applied {skill.skillName}!");
    }

    private GameObject FindCutsceneById(string id)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == id && obj.scene.isLoaded && obj.hideFlags == HideFlags.None)
                return obj;
        }
        return null;
    }

    private Character ChooseTarget(List<Character> team)
    {
        var alive = team.FindAll(c => c.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)];
    }
}
