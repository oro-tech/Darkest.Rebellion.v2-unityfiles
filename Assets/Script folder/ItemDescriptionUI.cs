using UnityEngine;
using TMPro;

public class ItemDescriptionUI : MonoBehaviour
{
    public static ItemDescriptionUI Instance;

    [Header("UI References")]
    public GameObject descriptionPanel; // The panel or background where text shows
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    private void Awake()
    {
        Instance = this;
        descriptionPanel.SetActive(false); // hide by default
    }

    public void ShowItemDescription(ItemData item)
    {
        if (item == null) return;

        descriptionPanel.SetActive(true);
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
