using Unity.VisualScripting;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    EnemyController ec;
    EnemySpawner es;

    void Start()
    {
        Destroy(gameObject, lifetime);
        ec = FindObjectOfType<EnemyController>();
        es = FindObjectOfType<EnemySpawner>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hitt");
            Destroy(gameObject);
            //Destroy(collision.gameObject);
            //es.isSpawned = false;
            if (ec.enemyLives > 0)
            {
                ec.enemyLives--;
                Debug.Log("EL: " + ec.enemyLives);
            }
            if (ec.enemyLives == 0)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
