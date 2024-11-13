using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingWall : MonoBehaviour
{
    public Transform waypointA;
    public Transform waypointB;
    public float moveSpeed = 2f;

    private Transform currentTarget;
    private bool movingToWaypointA = false;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        currentTarget = waypointA;
    }

    private void Update()
    {
        MoveWall();
    }

    private void MoveWall()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            movingToWaypointA = !movingToWaypointA;
            currentTarget = movingToWaypointA ? waypointA : waypointB;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            movingToWaypointA = !movingToWaypointA;
            currentTarget = movingToWaypointA ? waypointA : waypointB;
        }
    }
}
