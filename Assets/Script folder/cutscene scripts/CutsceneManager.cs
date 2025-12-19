using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;  // ✅ Singleton reference
    public PlayableDirector director;        // Timeline Director
    public GameObject attacker;              // Optional: dynamic binding
    public GameObject target;                // Optional: dynamic binding

    private void Awake()
    {
        // ✅ Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ✅ Ensure a PlayableDirector exists
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }
    }

    // ✅ Plays a cutscene and waits for it to finish
    public IEnumerator Play(PlayableAsset asset)
    {
        if (director == null)
        {
            Debug.LogError("❌ PlayableDirector missing on CutsceneManager!");
            yield break;
        }

        if (asset == null)
        {
            Debug.LogWarning("⚠️ Tried to play null PlayableAsset in CutsceneManager.");
            yield break;
        }

        // Assign playable asset
        director.playableAsset = asset;

        // ✅ Optional dynamic binding setup (useful for attack/target animations)
        foreach (var output in asset.outputs)
        {
            if (output.streamName.Contains("Attacker") && attacker != null)
                director.SetGenericBinding(output.sourceObject, attacker.GetComponent<Animator>());
            else if (output.streamName.Contains("Target") && target != null)
                director.SetGenericBinding(output.sourceObject, target.GetComponent<Animator>());
        }

        director.Play();

        Debug.Log($"▶️ Cutscene '{asset.name}' started.");
        yield return new WaitForSeconds((float)director.duration);
        Debug.Log($"✅ Cutscene '{asset.name}' finished.");
    }
}
