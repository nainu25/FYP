using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingWall : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.right; // Initial movement direction
    public float moveSpeed = 2f;
    public float moveDistance = 5f; // The distance to move before reversing

    private Vector2 startPosition;
    private float movedDistance;

    private void Start()
    {
        startPosition = transform.position; // Record the initial position
    }

    private void Update()
    {
        MoveWall();
        CheckDistanceAndReverse();
    }

    private void MoveWall()
    {
        // Move the wall in the current direction
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void CheckDistanceAndReverse()
    {
        // Calculate how far the wall has moved from its starting point
        movedDistance = Vector2.Distance(startPosition, transform.position);

        // Reverse direction if the wall has moved the specified distance
        if (movedDistance >= moveDistance)
        {
            moveDirection = -moveDirection; // Reverse the direction
            startPosition = transform.position; // Reset the start position
        }
    }
}

