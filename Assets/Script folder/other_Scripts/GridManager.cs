using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameObject playerPrefab;

    private GameObject player;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateGrid();
        CenterCamera();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 spawnPosition = new Vector3(x, y, 0);
                Tile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                spawnedTile.name = $"Tile {x},{y}";

                bool isOffset = (x + y) % 2 == 0;
                spawnedTile.Init(isOffset);
            }
        }
    }

    public void HandleTileClick(Tile tile)
    {
        Vector3 spawnPosition = tile.transform.position + Vector3.back; // Puts player on Z = -1

        if (player == null)
        {
            player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            player.transform.position = spawnPosition;
        }
    }

    void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, -10f);
    }
}
