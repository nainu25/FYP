using UnityEngine;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float detectionRange = 15f;
    public float stoppingDistance = 7f;

    [Header("Attack Settings")]
    public float attackRange = 10f;
    public float attackCooldown = 5f;
    public GameObject rockPrefab;
    public Transform throwPoint;
    public float rockSpeed = 20f;

    private Transform player;
    private bool inRange;
    private bool bookOpened = false;

    public event Action OnAttackCompleted;

    private SBQGameManager sBQGm;
    [SerializeField] public int enemyLives = 2;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sBQGm = FindObjectOfType<SBQGameManager>();
        inRange = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !inRange)
        {
            inRange = true;
            if (!bookOpened && CheckRange())
            {
                Debug.Log("Player entered detection range. Opening book...");
                sBQGm.OpenBook();
                bookOpened = true;
            }
        }
        else if (distanceToPlayer > detectionRange && inRange)
        {
            inRange = false;
            Debug.Log("Player exited detection range.");
            bookOpened = false;
        }
    }

    public bool CheckRange()
    {
        return inRange;
    }

    public void Attack()
    {
        if (rockPrefab == null || throwPoint == null) return;

        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        if (rock.TryGetComponent(out Rigidbody2D rockRb))
        {
            Vector2 direction = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
            rockRb.linearVelocity = direction * rockSpeed;
        }
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
