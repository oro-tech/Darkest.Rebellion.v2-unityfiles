using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    public GameObject cloudPrefab; // The cloud prefab
    public float spawnInterval = 2f; // Time between spawns
    public RectTransform[] spawnPoints; // Array of spawn points

    private void Start()
    {
        // Start spawning clouds
        InvokeRepeating("SpawnCloud", 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points available.");
            return;
        }

        // Choose a random spawn point
        RectTransform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate as UI element under the same parent (Canvas)
        GameObject cloud = Instantiate(cloudPrefab, spawnPoint.parent);
        GameObject cloud1 = Instantiate(cloudPrefab, spawnPoint.parent);

        // Match the localPosition of the spawnPoint
        cloud.GetComponent<RectTransform>().localPosition = spawnPoint.localPosition;

        Debug.Log("Spawned cloud at UI position: " + spawnPoint.localPosition);
    }

    private void OnDrawGizmos()
    {
        // Draw spawn points in the scene view
        Gizmos.color = Color.red;
        foreach (RectTransform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
