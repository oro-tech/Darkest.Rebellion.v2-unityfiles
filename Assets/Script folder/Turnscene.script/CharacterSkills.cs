using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSkills : MonoBehaviour
{
    public Button skill1Button; // Assign in inspector, optional for AI characters
    public Button skill2Button;

    public int skill1Damage = 20;
    public int skill2Damage = 10;

    public Character targetCharacter; // The enemy character this skill targets

    private bool skill1Used;
    private bool skill2Used;

    void Start()
    {
        if (skill1Button != null)
            skill1Button.onClick.AddListener(UseSkill1);

        if (skill2Button != null)
            skill2Button.onClick.AddListener(UseSkill2);
    }

    public void UseSkill1()
    {
        if (skill1Used) return;
        if (targetCharacter != null && targetCharacter.isAlive)
        {
            targetCharacter.TakeDamage(skill1Damage);
            skill1Used = true;
            StartCoroutine(FlashRed(targetCharacter.gameObject));
        }
    }

    public void UseSkill2()
    {
        if (skill2Used) return;
        if (targetCharacter != null && targetCharacter.isAlive)
        {
            targetCharacter.TakeDamage(skill2Damage);
            skill2Used = true;
            StartCoroutine(FlashRed(targetCharacter.gameObject));
        }
    }

    public void ResetSkillUsage()
    {
        skill1Used = false;
        skill2Used = false;
    }

    public IEnumerator ExecuteAITurn()
    {
        if (!skill1Used)
        {
            UseSkill1();
            yield return new WaitForSeconds(1f);
        }

        if (!skill2Used)
        {
            UseSkill2();
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator FlashRed(GameObject target)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = originalColor;
        }
        else
        {
            Renderer rend = target.GetComponent<Renderer>();
            if (rend != null)
            {
                Color originalColor = rend.material.color;
                rend.material.color = Color.red;
                yield return new WaitForSeconds(0.2f);
                rend.material.color = originalColor;
            }
        }
    }
}
