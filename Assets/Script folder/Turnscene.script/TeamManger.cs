using Enemy;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TeamManager : MonoBehaviour
{
    public List<Character> playerTeam;
    public List<Character> enemyTeam;

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;

    public enum BattleTurn { Player, Enemy }
    public BattleTurn currentTurn = BattleTurn.Player;

    void Start()
    {
        BeginPlayerTurn();
    }

    void BeginPlayerTurn()
    {
        currentTurn = BattleTurn.Player;
        currentPlayerIndex = GetNextAliveIndex(playerTeam, currentPlayerIndex);
        // Trigger input for current player
    }

    void BeginEnemyTurn()
    {
        currentTurn = BattleTurn.Enemy;
        currentEnemyIndex = GetNextAliveIndex(enemyTeam, currentEnemyIndex);
        // AI control logic
    }

    int GetNextAliveIndex(List<Character> team, int startIndex)
    {
        for (int i = 0; i < team.Count; i++)
        {
            int index = (startIndex + i) % team.Count;
            if (team[index].isAlive)
                return index;
        }
        return -1; // All dead
    }

    void CheckForVictory()
    {
        // If one team's characters are all dead ? end game
    }
}
