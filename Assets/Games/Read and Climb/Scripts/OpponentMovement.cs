using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMovement : MonoBehaviour
{
    public List<Transform> boardPositions;
    public int currentPosition = 0;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    public AudioSource audioSource;

    private Dictionary<int, int> snakes = new Dictionary<int, int>
    {
        {28, 8}, {37, 14}, {46, 4}, {52, 32},
        {61, 36}, {85, 53}, {91, 69}, {96, 24}
    };

    private Dictionary<int, int> ladders = new Dictionary<int, int>
    {
        {1, 22}, {7, 33}, {19, 76}, {31, 67},
        {40, 78}, {73, 87}, {81, 99}, {84, 94}
    };

    void Start()
    {
        transform.position = boardPositions[currentPosition].position;
    }

    public void TakeTurn(int diceResult)
    {
        if (!isMoving)
        {
            Debug.Log("Opponent rolled: " + diceResult);
            StartCoroutine(MoveOpponent(diceResult));
        }
    }

    private IEnumerator MoveOpponent(int steps)
    {
        isMoving = true;
        int targetPosition = Mathf.Min(currentPosition + steps, boardPositions.Count - 1);

        for (int i = currentPosition + 1; i <= targetPosition; i++)
        {
            yield return StartCoroutine(SmoothMove(boardPositions[i].position));
        }

        currentPosition = targetPosition;
        CheckForSnakesOrLadders();
        audioSource.Play();
        isMoving = false;
    }

    private IEnumerator SmoothMove(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = target;
    }

    private void CheckForSnakesOrLadders()
    {
        if (snakes.TryGetValue(currentPosition, out int snakeTarget))
        {
            Debug.Log($"Opponent bitten by a snake! Moving down to {snakeTarget}");
            StartCoroutine(MoveToPosition(snakeTarget));
        }
        else if (ladders.TryGetValue(currentPosition, out int ladderTarget))
        {
            Debug.Log($"Opponent climbed a ladder! Moving up to {ladderTarget}");
            StartCoroutine(MoveToPosition(ladderTarget));
        }
    }

    private IEnumerator MoveToPosition(int newPos)
    {
        yield return StartCoroutine(SmoothMove(boardPositions[newPos].position));
        currentPosition = newPos;
    }

    public int GetCurrentPosition()
    {
        return currentPosition;
    }
}
