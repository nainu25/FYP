using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRange = 15f;
    public float stoppingDistance = 7f;
    public float attackRange = 10f;
    public float attackCooldown = 5f;
    public GameObject rockPrefab;
    public Transform throwPoint;
    public float rockSpeed = 20f;
    private Transform player;

    bool inRange;
    bool bookOpened = false;

    public event Action OnAttackCompleted;

    SBQGameManager SBQGm;
    public int enemyLives = 2;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SBQGm = FindObjectOfType<SBQGameManager>();
        inRange = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !inRange) // Player has just entered the range
        {
            inRange = true; // Set inRange to true only when the player enters the range
            if (!bookOpened && CheckRange())
            {
                Debug.Log("Player entered detection range. Opening book...");
                SBQGm.OpenBook();
                bookOpened = true;
            }
        }
        else if (distanceToPlayer > detectionRange && inRange) // Player has just exited the range
        {
            inRange = false; // Reset inRange when the player exits the range
            Debug.Log("Player exited detection range.");
            bookOpened = false;
        }
    }


    public bool CheckRange()
    {
        return inRange;
    }

    /*public void Attack()
    {
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - throwPoint.position).normalized;
        rockRb.velocity = direction * rockSpeed;

        StartCoroutine(AttackRoutine());
    }*/
    public void Attack()
    {
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        // Determine the direction based on the enemy's facing direction
        Vector2 direction = transform.localScale.x < 0 ? Vector2.right : Vector2.left;

        // Set the velocity of the rock
        rockRb.velocity = direction * rockSpeed;

        StartCoroutine(AttackRoutine());
    }


    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Enemy attack completed.");
        OnAttackCompleted?.Invoke();
    }

    public void ResetBookFlag()
    {
        bookOpened = false;
    }
}
