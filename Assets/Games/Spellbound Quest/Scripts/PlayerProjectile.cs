using Unity.VisualScripting;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    EnemyController ec;

    void Start()
    {
        Destroy(gameObject, lifetime);
        ec = FindObjectOfType<EnemyController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            if(ec.enemyLives>0) 
            {
                ec.enemyLives--;
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
