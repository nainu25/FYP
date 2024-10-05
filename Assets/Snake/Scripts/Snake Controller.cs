using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SnakeController : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public int speed = 20;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;
    

    private readonly List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;
    private Transform snakeHead;

    public GameManager gm;

    private void Start()
    {
        snakeHead = transform.GetChild(0);
        ResetState();
    }

    private void Update()
    {
        InputHandler();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Time.time < nextUpdate)
        {
            return;
        }

        if (input != Vector2Int.zero)
        {
            direction = input;
        }

        RotateHead();

        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        int x = Mathf.RoundToInt(transform.position.x) + direction.x * speed;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y * speed;
        transform.position = new Vector2(x, y);

        nextUpdate = Time.time + 0.112f + Time.deltaTime;
    }

    private void InputHandler()
    {
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = Vector2Int.left;
            }
        }
    }

    private void RotateHead()
    {
        float rotationZ = 0f;

        if (direction == Vector2Int.right)
        {
            rotationZ = 0f;
        }
        else if (direction == Vector2Int.up)
        {
            rotationZ = 90f;
        }
        else if (direction == Vector2Int.left)
        {
            rotationZ = 180f;
        }
        else if (direction == Vector2Int.down)
        {
            rotationZ = 270f;
        }
        snakeHead.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;

        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            ResetState();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls)
            {
                Traverse(other.transform);
            }
            else
            {
                gm.LoseLife();
            }
        }
    }
    

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f)
        {
            position.x = Mathf.RoundToInt(-wall.position.x - direction.x);
        }
        else if (direction.y != 0f)
        {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }

        transform.position = position;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Optionally stop the game
        Time.timeScale = 0f;
    }

}
