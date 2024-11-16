using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    SBQGameManager SBQGm;

    void Start()
    {
        Destroy(gameObject, lifetime);
        SBQGm = FindObjectOfType<SBQGameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            if(SBQGm.lives>0)
            {
                SBQGm.lives--;
                SBQGm.UpdateLivesText();
            }
            else
            {
                Destroy(collision.gameObject);
                SBQGm.EndGame();
            }
        }
    }
}
