using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;
    public bool isSpawned;


    void Start()
    {
        SpawnEnemy(0);
        SpawnEnemy(1);
    }

    private void SpawnEnemy(int spawnIndex)
    {
        if (spawnPoints.Length == 0 || enemyPrefab.Length == null)
        {
            Debug.LogWarning("No spawn points or enemy prefab assigned.");
            return;
        }
        Transform spawnPoint = spawnPoints[spawnIndex];

        Instantiate(enemyPrefab[spawnIndex], spawnPoint.position, spawnPoint.rotation);
        isSpawned = true;
    }
}
