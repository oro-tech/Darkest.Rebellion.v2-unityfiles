using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(2, 5)] public string description;
    // optional, you can remove if not needed
}
