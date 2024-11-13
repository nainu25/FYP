using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy GameObject to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 3f; // Time interval between spawns

    private bool isSpawning = true;

    void Start()
    {
        //StartCoroutine(SpawnEnemies());
        SpawnEnemy();
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("No spawn points or enemy prefab assigned.");
            return;
        }

        // Select a random spawn point from the array
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Instantiate the enemy at the chosen spawn point
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }
}
