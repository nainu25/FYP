using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Board Settings")]
    public List<Transform> boardPositions;
    public float moveSpeed = 5f;
    public ParticleSystem moveEffect;

    [Header("Game State")]
    public int currentPosition = 0;
    private bool isMoving = false;
    public TMP_Text scoreText;
    public GameObject gameWonPanel;

    [Header("Turn Management")]
    public int turns { get; private set; }
    public int tempTurn;

    public AudioSource audioSource;

    private Dictionary<int, int> snakes = new Dictionary<int, int>();
    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    private SpeechRecognitionTest speechRecognition;
    private OpponentMovement opponent;
    private DiceRoll diceRoll;
    DataSaver dataSaver;
    public RnCGameManager rNCGM;
    void Start()
    {
        gameWonPanel.SetActive(false);
        turns = 0;
        tempTurn = 0;

        transform.position = boardPositions[currentPosition].position;

        InitializeBoard();

        dataSaver = FindFirstObjectByType<DataSaver>();

        if(rNCGM !=null)
        {
            Debug.Log("rNCGM working");
        }

        // Cache references to avoid repeated FindObjectOfType calls
        speechRecognition = FindFirstObjectByType<SpeechRecognitionTest>();
        opponent = FindFirstObjectByType<OpponentMovement>();
        diceRoll = FindFirstObjectByType<DiceRoll>();
    }

    private void InitializeBoard()
    {
        // Snakes - Key: Start position, Value: End position
        snakes = new Dictionary<int, int>
        {
            { 28, 8 }, { 37, 14 }, { 46, 4 }, { 52, 32 },
            { 61, 36 }, { 85, 53 }, { 91, 69 }, { 96, 24 }
        };

        // Ladders - Key: Start position, Value: End position
        ladders = new Dictionary<int, int>
        {
            { 1, 22 }, { 7, 33 }, { 19, 76 }, { 31, 67 },
            { 40, 78 }, { 73, 87 }, { 81, 99 }, { 84, 94 }
        };
    }

    public void RollDice(int diceResult)
    {
        if (isMoving) return;

        turns++;
        tempTurn = turns;

        if (speechRecognition.AllTasksCompleted())
        {
            currentPosition = 97; // Directly move to position 98 if tasks are completed
        }

        StartCoroutine(MovePlayer(diceResult));
    }

    private IEnumerator MovePlayer(int steps)
    {
        isMoving = true;

        int targetPosition = Mathf.Min(currentPosition + steps, boardPositions.Count - 1);

        for (int i = currentPosition + 1; i <= targetPosition; i++)
        {
            yield return MoveTo(boardPositions[i].position);
        }

        currentPosition = targetPosition;
        scoreText.text = (currentPosition + 1).ToString();

        if (currentPosition == 99)
        {
            HandleGameCompletion();
            yield break;
        }

        yield return CheckForSnakesOrLadders();
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        

        // Let opponent roll the dice if they haven't won
        if (opponent.GetCurrentPosition() != 99)
        {
            diceRoll.OppRollDice();
        }

        isMoving = false;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;
        float duration = 0.3f;

        moveEffect.Play();

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = target;
        moveEffect.Stop();
    }

    public void HandleGameCompletion()
    {
        if (speechRecognition.AllTasksCompleted())
        {
            Debug.Log("Congratulations! You have completed all tasks and won the game!");
            gameWonPanel.SetActive(true);
            rNCGM.CalculateScore();
            dataSaver.SaveData();
        }
        else
        {
            Debug.Log("You have reached the last position, but you must complete all tasks first!");
            speechRecognition.ShowTaskPanel();
        }
    }

    private IEnumerator CheckForSnakesOrLadders()
    {
        if (snakes.TryGetValue(currentPosition, out int snakeDest))
        {
            Debug.Log($"Bitten by a snake! Moving down to {snakeDest}");
            yield return MoveToPosition(snakeDest);
        }
        else if (ladders.TryGetValue(currentPosition, out int ladderDest))
        {
            Debug.Log($"Climbed a ladder! Moving up to {ladderDest}");
            yield return MoveToPosition(ladderDest);
        }
    }

    private IEnumerator MoveToPosition(int newPos)
    {
        Vector3 targetPos = boardPositions[newPos].position;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        currentPosition = newPos;
        scoreText.text = (currentPosition + 1).ToString();
    }
}
