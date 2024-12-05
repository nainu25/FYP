using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 10f;
    public int initialLives = 5;
    
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text livesText;
    public TMP_Text scoreText;

    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;

    private int lives;
    public int score;
    public int level;

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (!gameEnded && timerRunning)
        {
            UpdateTimer();
        }
    }

    private void InitializeGame()
    {
        // Initialize lives and score
        lives = initialLives;
        score = PlayerPrefs.GetInt("Score", 0);

        // Update UI
        UpdateLivesUI();
        UpdateScoreText();
        ResetTimer();

        // Resume game
        gameEnded = false;
        Time.timeScale = 1f;
    }

    public void StartTimer()
    {
        timerRunning = true;
        timeRemaining = gameDuration;
        UpdateTimerUI();
    }

    public void ResetTimer()
    {
        timeRemaining = gameDuration;
        timerRunning = false;
        UpdateTimerUI();
    }

    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            LoseLife();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            EndGame();
        }
        else
        {
            ResetTimer();
        }
    }

    public void EndGame()
    {
        gameEnded = true;
        timerRunning = false;

        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        InitializeGame();
    }
}
