using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    public static DamageFlash Instance;

    public GameObject playerCapsule;
    public GameObject enemyCapsule;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FlashPlayer()
    {
        if (playerCapsule != null)
            StartCoroutine(FlashRed(playerCapsule));
    }

    public void FlashEnemy()
    {
        if (enemyCapsule != null)
            StartCoroutine(FlashRed(enemyCapsule));
    }

    private IEnumerator FlashRed(GameObject target)
    {
        // 2D SpriteRenderer
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = originalColor;
            yield break;
        }

        // 3D MeshRenderer fallback
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
