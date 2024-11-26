using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("Time in seconds before the projectile is automatically destroyed.")]
    public float lifetime = 2f;

    private EnemyController enemyController;

    private void Start()
    {
        // Destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);

        // Attempt to find the EnemyController in the scene
        enemyController = FindObjectOfType<EnemyController>();

        if (enemyController == null)
        {
            Debug.LogWarning("EnemyController not found in the scene. Ensure there is one in the hierarchy.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hit an enemy
        if (collision.CompareTag("Enemy"))
        {
            HandleEnemyHit(collision.gameObject);
        }
    }

    private void HandleEnemyHit(GameObject enemy)
    {
        Debug.Log("Projectile hit an enemy.");
        Destroy(gameObject); // Destroy the projectile on impact

        if (enemyController == null)
        {
            Debug.LogError("EnemyController reference is missing. Cannot process enemy hit.");
            return;
        }

        // Reduce enemy's lives and check if it's destroyed
        UpdateEnemyLives(enemy);
    }

    private void UpdateEnemyLives(GameObject enemy)
    {
        // Decrease the enemy's life count
        enemyController.enemyLives--;
        Debug.Log($"Enemy Lives Remaining: {enemyController.enemyLives}");

        // Destroy the enemy if its lives reach zero
        if (enemyController.enemyLives <= 0)
        {
            Destroy(enemy);
            Debug.Log("Enemy destroyed.");
        }
    }
}