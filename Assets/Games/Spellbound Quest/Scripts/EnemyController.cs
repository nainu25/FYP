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
    public int enemyLives = 3;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SBQGm = FindObjectOfType<SBQGameManager>();
        inRange = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !bookOpened)
        {
            inRange = true;
            if (CheckRange())
            {
                SBQGm.OpenBook();
                bookOpened = true;
            }
        }
        else if (distanceToPlayer > detectionRange)
        {
            inRange = false;
            bookOpened = false;
        }
    }

    public bool CheckRange()
    {
        return inRange;
    }

    public void Attack()
    {
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - throwPoint.position).normalized;
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
