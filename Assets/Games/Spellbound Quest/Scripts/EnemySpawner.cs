using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Settings")]
    [Tooltip("Enemy prefab to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Array of spawn points where enemies can appear.")]
    public Transform[] spawnPoints;

    [Header("Respawn Settings")]
    [Tooltip("Delay before respawning the enemy.")]
    public float respawnDelay = 2f;

    // Reference to the currently spawned enemy
    private GameObject currentEnemy;

    // Flag to track if a respawn is in progress
    private bool isRespawning = false;

    private void Start()
    {
        // Spawn the first enemy at a random spawn point
        SpawnEnemyAtRandomPoint();
    }

    private void Update()
    {
        // Check if the enemy is destroyed and no respawn is currently in progress
        if (currentEnemy == null && !isRespawning)
        {
            StartCoroutine(RespawnEnemyWithDelay());
        }
    }

    private void SpawnEnemyAtRandomPoint()
    {
        // Ensure there are valid spawn points
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned.");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn the enemy and track it
        currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"Enemy spawned at {spawnPoint.name}.");
    }

    private IEnumerator RespawnEnemyWithDelay()
    {
        isRespawning = true; // Set the flag to prevent multiple respawn attempts
        Debug.Log($"Enemy destroyed. Respawning after {respawnDelay} seconds.");

        yield return new WaitForSeconds(respawnDelay);

        // Spawn the enemy at a new random location
        SpawnEnemyAtRandomPoint();

        isRespawning = false; // Reset the flag after respawning
    }
}
