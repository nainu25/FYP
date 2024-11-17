using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SnakeController : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    int speed = 1;
    public float speeder = 0.5f;
    public int initialSize = 4;

    private readonly List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;
    private Transform snakeHead;

    public GameManager gm;

    private bool canMove = false;

    public Button upButton;
    public Button rightButton;
    public Button downButton;
    public Button leftButton;

    private void Awake()
    {
        Time.timeScale = 0;
    }
    private void Start()
    {
        snakeHead = transform.GetChild(0);
        ResetState();
        StartCoroutine(WaitBeforeMove(2f));

        upButton.onClick.AddListener(() => SetDirection(Vector2Int.up));
        rightButton.onClick.AddListener(() => SetDirection(Vector2Int.right));
        downButton.onClick.AddListener(() => SetDirection(Vector2Int.down));
        leftButton.onClick.AddListener(() => SetDirection(Vector2Int.left));
    }

    private void Update()
    {
        InputHandler();
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            Time.timeScale = 1f;
            Move();
        }
    }

    private IEnumerator WaitBeforeMove(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;

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

        if (gm.level == 3)
        {
            if (CheckSelfCollision())
            {
                gm.LoseLife();
                ResetState();

            }
        }

        nextUpdate = Time.time + speeder;
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

    private void SetDirection(Vector2Int newDirection)
    {
        if (newDirection != -direction) // Prevent reversing direction
        {
            input = newDirection;
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

    private bool CheckSelfCollision()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            if (snakeHead.position == segments[i].position)
            {
                Debug.Log("Self-collision detected!");
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gm.LoseLife();
            ResetState();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            gm.LoseLife();
            ResetState();
        }
    }
}

