using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 10f;
    private float timeRemaining;

    public int lives;
    public TMP_Text timerText;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    
    private bool gameEnded = false;
    private bool timerRunning = false;
    public int level;
    public int score;

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
            timerText.text = Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    public void UpdateScoreText()
    {
       if(scoreText!=null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = lives.ToString();

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
        //lives = 5;
        livesText.text = lives.ToString();
        ResetTimer();
        gameEnded = false;
        Time.timeScale = 1f;
    }
}

