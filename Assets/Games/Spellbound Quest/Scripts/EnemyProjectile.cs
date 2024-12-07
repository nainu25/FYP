using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    SBQGameManager SBQGm;
    AudioController audioController;

    void Start()
    {
        Destroy(gameObject, lifetime);
        SBQGm = FindObjectOfType<SBQGameManager>();
        audioController = FindObjectOfType<AudioController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            if(SBQGm.lives >= 1)
            {
                SBQGm.lives--;
                SBQGm.UpdateLivesText();
            }
            else
            {
                Destroy(collision.gameObject);
                audioController.PlayAudio("Game Over");
                SBQGm.EndGame();
            }
        }
    }
}
