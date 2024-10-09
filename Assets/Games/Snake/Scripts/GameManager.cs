using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 10f;
    private float timeRemaining;

    public int lives = 5;
    public TMP_Text timerText;
    public TMP_Text livesText;
    
    private bool gameEnded = false;
    private bool timerRunning = false;

    void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        Timer();
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

    public void Timer()
    {
        if (gameEnded || !timerRunning)
        {
            return;
        }

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
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            EndGame();
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
        lives = 5;
        livesText.text = "Lives: " + lives;
        ResetTimer();
        gameEnded = false;
        Time.timeScale = 1f;
    }
}

