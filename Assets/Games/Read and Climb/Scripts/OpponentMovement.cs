using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpponentMovement : MonoBehaviour
{
    public List<Transform> boardPositions;
    public int currentPosition = 0;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    private Dictionary<int, int> snakes = new Dictionary<int, int>();
    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    void Start()
    {
        transform.position = boardPositions[currentPosition].position;

        snakes.Add(28, 8);
        snakes.Add(37, 14);
        snakes.Add(46, 4);
        snakes.Add(52, 32);
        snakes.Add(61, 36);
        snakes.Add(85, 53);
        snakes.Add(91, 69);
        snakes.Add(96, 24);

        ladders.Add(1, 22);
        ladders.Add(7, 33);
        ladders.Add(19, 76);
        ladders.Add(31, 67);
        ladders.Add(40, 78);
        ladders.Add(73, 87);
        ladders.Add(81, 99);
        ladders.Add(84, 94);
    }

    public void TakeTurn(int diceResult)
    {
        if (!isMoving)
        {
            Debug.Log("Opponent rolled: " + diceResult);
            StartCoroutine(MoveOpponent(diceResult));
        }
    }

    IEnumerator MoveOpponent(int steps)
    {
        int targetPosition = currentPosition + steps;
        if (targetPosition >= boardPositions.Count - 1)
        {
            targetPosition = boardPositions.Count - 1;
        }

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
        float duration = 0.3f;


        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = target;
    }

    void CheckForSnakesOrLadders()
    {
        if (snakes.ContainsKey(currentPosition))
        {
            int newPos = snakes[currentPosition];
            Debug.Log("Opponent bitten by a snake! Moving down to " + newPos);
            StartCoroutine(MoveToPosition(newPos));
        }
        else if (ladders.ContainsKey(currentPosition))
        {
            int newPos = ladders[currentPosition];
            Debug.Log("Opponent climbed a ladder! Moving up to " + newPos);
            StartCoroutine(MoveToPosition(newPos));
        }
        int tempPos = currentPosition + 1;
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
    }
}
