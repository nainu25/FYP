using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 120f;
    public int lives = 5;
    private float timeRemaining;

    public SnakeController sc;


    public TMP_Text timerText;
    public TMP_Text livesText;


    private bool gameEnded = false;

    void Start()
    {
        timeRemaining = gameDuration;
        gameEnded = false;
        livesText.text = "Lives: " + lives;
    }

    void Update()
    {
        Timer();
    }

    public void EndGame()
    {
        gameEnded = true;
        Debug.Log("Time's up! Game Over!");


        Time.timeScale = 0f;
    }

    void Timer()
    {
        if (gameEnded)
        {
            return;
        }
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString() + "s";
            }
        }
        else
        {
            EndGame();
        }
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives > 0)
        {
            sc.ResetState();
        }
        else
        {
            EndGame();
        }
    }
}


