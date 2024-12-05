using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingWall : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 moveDirection = Vector2.right; // Initial movement direction
    [SerializeField] private float moveSpeed = 2f;                  // Speed of movement
    [SerializeField] private float moveDistance = 5f;              // Distance before reversing

    private Vector2 startPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        // Cache components and initial position
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensure the Rigidbody doesn't interfere with physics
    }

    private void FixedUpdate()
    {
        MoveWall();
        CheckAndReverseDirection();
    }

    /// <summary>
    /// Moves the wall in the current direction.
    /// </summary>
    private void MoveWall()
    {
        Vector2 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// Checks if the wall has moved the specified distance and reverses its direction if necessary.
    /// </summary>
    private void CheckAndReverseDirection()
    {
        if (Vector2.Distance(startPosition, transform.position) >= moveDistance)
        {
            moveDirection = -moveDirection; // Reverse direction
            startPosition = transform.position; // Update starting position
        }
    }
}
