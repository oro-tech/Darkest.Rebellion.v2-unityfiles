using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData itemData;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            button = GetComponentInChildren<Button>();
        }

        if (button != null)
        {
            button.onClick.AddListener(OnItemClicked);
        }
        else
        {
            Debug.LogWarning($"{name}: No Button component found!");
        }
    }

    private void OnItemClicked()
    {
        if (itemData != null)
        {
            Debug.Log($"Clicked on item: {itemData.itemName}");
            ItemDescriptionUI.Instance.ShowItemDescription(itemData);
        }
        else
        {
            Debug.LogWarning($"{name}: No ItemData assigned!");
        }
    }
}
