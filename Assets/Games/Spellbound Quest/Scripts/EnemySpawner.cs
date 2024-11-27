using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Settings")]
    [Tooltip("Array of enemy prefabs to spawn.")]
    public GameObject[] enemyPrefabs;

    [Tooltip("Array of spawn points where enemies can appear.")]
    public Transform[] spawnPoints;

    [Tooltip("Tracks if any enemy has been spawned.")]
    public bool isSpawned;

    private void Start()
    {
        // Example: Spawn enemies at specific spawn points
        SpawnEnemy(0, 0); // Spawn first enemy at first spawn point
        SpawnEnemy(1, 1); // Spawn second type of enemy at second spawn point
        SpawnEnemy(2, 1); // Spawn another enemy of type 1 at third spawn point
    }

    /// <summary>
    /// Spawns an enemy at the specified spawn point.
    /// </summary>
    /// <param name="spawnIndex">Index of the spawn point.</param>
    /// <param name="enemyIndex">Index of the enemy prefab.</param>
    private void SpawnEnemy(int spawnIndex, int enemyIndex)
    {
        // Validate spawn point and enemy prefab arrays
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawn points array is empty or not assigned.");
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs array is empty or not assigned.");
            return;
        }

        // Validate indices
        if (spawnIndex < 0 || spawnIndex >= spawnPoints.Length)
        {
            Debug.LogError($"Spawn index {spawnIndex} is out of bounds. Ensure it is within the range of spawn points array.");
            return;
        }

        if (enemyIndex < 0 || enemyIndex >= enemyPrefabs.Length)
        {
            Debug.LogError($"Enemy index {enemyIndex} is out of bounds. Ensure it is within the range of enemy prefabs array.");
            return;
        }

        // Get spawn point and instantiate the enemy
        Transform spawnPoint = spawnPoints[spawnIndex];
        Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, spawnPoint.rotation);

        isSpawned = true;
        Debug.Log($"Enemy of type {enemyIndex} spawned at spawn point {spawnIndex}.");
    }
}
