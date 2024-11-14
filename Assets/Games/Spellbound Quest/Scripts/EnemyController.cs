using UnityEngine;

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
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private float nextAttackTime = 0f;

    bool inRange;

    SBQGameManager SBQGm;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        SBQGm = FindObjectOfType<SBQGameManager>();
        inRange = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if(distanceToPlayer<=detectionRange)
        {
            inRange = true;
            while(inRange)
            {
                SBQGm.OpenBook();
                inRange = !inRange;
            }
            /*if (Time.time >= nextAttackTime)
            {
                *//*Attack();
                nextAttackTime = (Time.time + attackCooldown);*//*
            }*/
        }
        else
        {
            inRange = false;
        }
        /*if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > stoppingDistance && distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopMoving();

                if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        else
        {
            StopMoving();
        }*/




    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

    }

    public bool CheckRange()
    {

        if (inRange)
        {
            SBQGm.OpenBook();
            return true;
        }
        else
        {
            return false;
        }
    }

    void Attack()
    {
        StopMoving();

        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - throwPoint.position).normalized;
        rockRb.velocity = direction * rockSpeed;
        
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
