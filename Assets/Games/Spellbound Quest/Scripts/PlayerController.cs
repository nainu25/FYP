using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject rockPrefab;
    public Transform throwPoint;
    public float rockSpeed = 20f;
    private Transform enemy;

    public event Action OnAttackCompleted;
    private SBQGameManager SBQGm;
    bool isPunching;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SBQGm = FindObjectOfType<SBQGameManager>();
        SBQGm.coins = 0;
        isPunching = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (isMovingLeft)
            moveInput = -1f;
        else if (isMovingRight)
            moveInput = 1f;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(moveInput) > 0)
        {
            animator.SetBool("IsWalking", true);
            

        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        if(Input.GetMouseButtonDown(0))
        {
            if(!isPunching)
            {
                Punch();
            }
        }
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsJumping", !isGrounded);

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || isJumping))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
            animator.SetTrigger("Jump");
        }
    }

    void Punch()
    {
        isPunching = true;
        animator.SetBool("IsPunching", true);
        Debug.Log("Punch triggered!");
        StartCoroutine(ResetPunch());
    }

    private IEnumerator ResetPunch()
    {
        yield return new WaitForSeconds(0.5f);

        isPunching = false;
        animator.SetBool("IsPunching", false);
        Debug.Log("Punch reset!");
    }


    void UpdateAnimations()
    {
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }
    }

    /*public void Attack()
    {
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        Vector2 direction = (enemy.position - throwPoint.position).normalized;
        rockRb.velocity = direction * rockSpeed;
        StartCoroutine(AttackRoutine());
    }*/

    public void Attack()
    {
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        Vector2 direction;
        if (spriteRenderer.flipX)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }

        rockRb.velocity = direction * rockSpeed;

        StartCoroutine(AttackRoutine());
    }


    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Player attack completed.");
        OnAttackCompleted?.Invoke();
    }

    public void MoveLeftDown() { isMovingLeft = true; }
    public void MoveLeftUp() { isMovingLeft = false; }
    public void MoveRightDown() { isMovingRight = true; }
    public void MoveRightUp() { isMovingRight = false; }
    public void JumpButtonDown() { if (isGrounded) isJumping = true; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            SBQGm.coins++;
            SBQGm.UpdateCoinsText();
            Destroy(collision.gameObject);
            Debug.Log("Coins: " + SBQGm.coins);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy2"))
        {
            if (isPunching)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
