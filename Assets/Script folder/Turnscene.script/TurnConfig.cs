using UnityEngine;

[CreateAssetMenu(fileName = "TurnConfig", menuName = "Configs/TurnConfig")]
public class TurnConfig : ScriptableObject
{
    [Header("Turn Settings")]
    [Tooltip("Time limit per turn in seconds")]
    public float turnTimeLimit;

    [Tooltip("Maximum number of actions per turn")]
    public int maxActionsPerTurn;
}
