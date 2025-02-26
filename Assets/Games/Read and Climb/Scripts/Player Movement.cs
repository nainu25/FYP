using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public List<Transform> boardPositions; // Stores 100 positions
    public int currentPosition = 0; // Player starts at position 0
    public float moveSpeed = 5f; // Movement speed
    private bool isMoving = false;
    public ParticleSystem moveEffect;

    public int turns { get; set; }

    public TMP_Text scoreText;

    // Dictionary for Snakes and Ladders
    private Dictionary<int, int> snakes = new Dictionary<int, int>();
    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    void Start()
    {
        turns = 0;
        // Set initial position
        transform.position = boardPositions[currentPosition].position;

        // Define Snakes (key = head, value = tail)
        snakes.Add(28, 8);
        snakes.Add(37, 14);
        snakes.Add(46, 4);
        snakes.Add(52, 32);
        snakes.Add(61, 36);
        snakes.Add(85, 53);
        snakes.Add(91, 69);
        snakes.Add(96, 24);

        // Define Ladders (key = bottom, value = top)
        ladders.Add(1, 22);
        ladders.Add(7, 33);
        ladders.Add(19, 76);
        ladders.Add(31, 67);
        ladders.Add(40, 78);
        ladders.Add(73, 87);
        ladders.Add(81, 99);
        ladders.Add(84, 94);
    }

    public void RollDice(int diceResult)
    {
        if (!isMoving)
        {
            turns++;
            StartCoroutine(MovePlayer(diceResult));
        }
    }

    IEnumerator MovePlayer(int steps)
    {
        int targetPosition = currentPosition + steps;
        if (targetPosition >= boardPositions.Count) targetPosition = boardPositions.Count - 1;

        for (int i = currentPosition + 1; i <= targetPosition; i++)
        {
            yield return StartCoroutine(SmoothMove(boardPositions[i].position));
        }

        currentPosition = targetPosition;
        CheckForSnakesOrLadders();
    }

    IEnumerator SmoothMove(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;
        float duration = 0.3f; // Time per step

        moveEffect.Play(); // Play effect at start

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = target;
        moveEffect.Stop(); // Stop effect after reaching
    }


    void CheckForSnakesOrLadders()
    {
        if (snakes.ContainsKey(currentPosition))
        {
            int newPos = snakes[currentPosition];
            Debug.Log("Bitten by a snake! Moving down to " + newPos);
            StartCoroutine(MoveToPosition(newPos));
        }
        else if (ladders.ContainsKey(currentPosition))
        {
            int newPos = ladders[currentPosition];
            Debug.Log("Climbed a ladder! Moving up to " + newPos);
            StartCoroutine(MoveToPosition(newPos));
        }
        int tempPos = currentPosition + 1;
        scoreText.text = tempPos.ToString();
    }

    IEnumerator MoveToPosition(int newPos)
    {
        Vector3 targetPos = boardPositions[newPos].position;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        currentPosition = newPos;
        int tempPos = currentPosition + 1;
        scoreText.text = tempPos.ToString();

    }
}
