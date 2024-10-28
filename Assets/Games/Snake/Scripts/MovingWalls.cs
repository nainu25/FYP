using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingWall : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public bool moveAlongX;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingForward = true;

    private void Start()
    {
        if (moveAlongX)
        {
            moveDirection = Vector2.right;
        }
        else
        {
            moveDirection = Vector2.up;
        }
        startPosition = transform.position;
        targetPosition = startPosition + moveDirection.normalized * moveDistance;

        // Ensure Rigidbody2D is set to Kinematic
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        MoveWall();
    }

    private void MoveWall()
    {
        Vector2 currentTarget = movingForward ? targetPosition : startPosition;

        transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("Obstacle"))
        {
            movingForward = !movingForward;
        }
    }
}
