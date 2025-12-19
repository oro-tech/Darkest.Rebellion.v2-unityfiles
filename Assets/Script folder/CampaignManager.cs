using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CampaignManager : MonoBehaviour
{
    [Header("Campaign Buttons (Assign in Inspector)")]
    public Button[] buttons;

    private void Awake()
    {
        int unlockedCampaigns = PlayerPrefs.GetInt("UnlockedCampaigns", 1);
        Debug.Log($"[CampaignManager] Loaded: UnlockedCampaigns = {unlockedCampaigns}");

        // Ensure only unlocked campaigns are interactable
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i < unlockedCampaigns);
        }
    }

    public void OpenCampaign(int index)
    {
        int unlockedCampaigns = PlayerPrefs.GetInt("UnlockedCampaigns", 1);

        // Prevent opening locked campaigns
        if (index >= unlockedCampaigns)
        {
            Debug.LogWarning($"[CampaignManager] Campaign {index + 1} is locked!");
            return;
        }

        PlayerPrefs.SetInt("CurrentCampaignLevel", index + 1);
        PlayerPrefs.Save();

        Debug.Log($"[CampaignManager] Opening Campaign {index + 1}");

        // Load different team selection scenes based on campaign
        if (index == 0)
            SceneManager.LoadScene("TeamSelectionScene");
        else if (index == 1)
            SceneManager.LoadScene("TeamSelection2Scene");
        else
            SceneManager.LoadScene("TeamSelectionScene"); // fallback if more levels are added
    }

    public void ResetCampaignData()
    {
        PlayerPrefs.DeleteKey("UnlockedCampaigns");
        PlayerPrefs.DeleteKey("CurrentCampaignLevel");
        PlayerPrefs.Save();

        Debug.Log("✅ Campaign progress reset. Only Level 1 will be unlocked.");
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        // 🔁 Press "R" to reset campaign data
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[CampaignManager] Hotkey R pressed — resetting campaign data...");
            ResetCampaignData();
        }
    }
}
