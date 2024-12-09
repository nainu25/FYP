using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class SnakeController : MonoBehaviour
{
    [Header("Snake Settings")]
    public Transform segmentPrefab;
    public int initialSize = 4;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public float segmentOffset = 0.5f;

    [Header("UI Controls")]
    public Button upButton;
    public Button rightButton;
    public Button downButton;
    public Button leftButton;

    [Header("Game Manager")]
    public GameManager gm;

    private List<Transform> segments = new List<Transform>();
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int input = Vector2Int.right;
    private float nextUpdate;
    private Transform snakeHead;
    private bool canMove = false;

    private void Awake()
    {
        Time.timeScale = 0f; // Paused initially
    }

    private void Start()
    {
        InitializeSnake();
        RegisterButtonListeners();
        StartCoroutine(EnableMovementAfterDelay(2f));
    }

    private void Update()
    {
        HandleKeyboardInput();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Time.timeScale = 1f;
            MoveSnake();
        }
    }

    private void InitializeSnake()
    {
        snakeHead = transform.GetChild(0);
        ResetState();
    }

    private void RegisterButtonListeners()
    {
        if (upButton) upButton.onClick.AddListener(() => SetDirection(Vector2Int.up));
        if (rightButton) rightButton.onClick.AddListener(() => SetDirection(Vector2Int.right));
        if (downButton) downButton.onClick.AddListener(() => SetDirection(Vector2Int.down));
        if (leftButton) leftButton.onClick.AddListener(() => SetDirection(Vector2Int.left));
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    private void HandleKeyboardInput()
    {
        if (direction.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) input = Vector2Int.up;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) input = Vector2Int.down;
        }
        else if (direction.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) input = Vector2Int.right;
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) input = Vector2Int.left;
        }
    }



    private void MoveSnake()
    {
        if (Time.time < nextUpdate) return;

        if (input != Vector2Int.zero && input != -direction)
        {
            direction = input;
        }

        RotateSnakeHead();

        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        Vector2 newPosition = (Vector2)transform.position + (Vector2)direction * segmentOffset;
        transform.position = newPosition;

        if (gm.level >= 3 && CheckSelfCollision())
        {
            gm.LoseLife();
            ResetState();
        }

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    private void RotateSnakeHead()
    {
        float rotationZ = direction == Vector2Int.up ? 90f :
                          direction == Vector2Int.left ? 180f :
                          direction == Vector2Int.down ? 270f : 0f;

        snakeHead.rotation = Quaternion.Euler(0f, 0f, rotationZ);
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

    public void Grow()
    {
        Transform newSegment = Instantiate(segmentPrefab);
        newSegment.position = segments[segments.Count - 1].position;
        segments.Add(newSegment);
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        input = direction;
        RotateSnakeHead();
        transform.position = Vector3.zero;

        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            Grow();
        }
    }

    private void SetDirection(Vector2Int newDirection)
    {
        if (newDirection != -direction)
        {
            input = newDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Wall"))
        {
            gm.LoseLife();
            ResetState();
        }
    }
}
