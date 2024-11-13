using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    SBQGameManager SBQGm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SBQGm = FindObjectOfType<SBQGameManager>();
        SBQGm.coins = 0;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        // PC Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Override with touch input if applicable
        if (isMovingLeft)
            moveInput = -1f;
        else if (isMovingRight)
            moveInput = 1f;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // PC Jump input
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || isJumping))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false; // Reset after jump for touch input
        }
    }


    public void MoveLeftDown() { isMovingLeft = true; }
    public void MoveLeftUp() { isMovingLeft = false; }
    public void MoveRightDown() { isMovingRight = true; }
    public void MoveRightUp() { isMovingRight = false; }
    public void JumpButtonDown() { if (isGrounded) isJumping = true; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            SBQGm.coins++;
            SBQGm.UpdateCoinsText();
            Destroy(collision.gameObject);
            Debug.Log("Coins: " + SBQGm.coins);
        }
    }
}
