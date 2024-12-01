using System.Collections;
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

    [Header("Respawn Settings")]
    [Tooltip("Delay before respawning the enemy of type 0.")]
    public float respawnDelay = 2f;

    private GameObject currentEnemyType0;

    private void Start()
    {
        // Spawn initial enemies
        SpawnEnemy(0, 0); // Spawn first enemy at first spawn point
        SpawnEnemy(1, 1); // Spawn second type of enemy at second spawn point
        SpawnEnemy(2, 1); // Spawn another enemy of type 1 at third spawn point
    }

    private void Update()
    {
        // Check if the enemy of type 0 is destroyed and respawn if needed
        if (currentEnemyType0 == null && isSpawned)
        {
            isSpawned = false; // Prevent multiple respawn attempts
            StartCoroutine(RespawnEnemyType0());
        }
    }

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
        GameObject spawnedEnemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, spawnPoint.rotation);

        if (enemyIndex == 0)
        {
            currentEnemyType0 = spawnedEnemy; // Track the spawned enemy of type 0
        }

        isSpawned = true;
        Debug.Log($"Enemy of type {enemyIndex} spawned at spawn point {spawnIndex}.");
    }

    private IEnumerator RespawnEnemyType0()
    {
        Debug.Log($"Enemy of type 0 destroyed. Respawning after {respawnDelay} seconds.");
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy(0, 0); // Respawn enemy of type 0 at spawn point 0
    }
}
