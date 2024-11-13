using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this if you use TextMeshPro

public class SBQGameManager : MonoBehaviour
{
    private bool isPaused = false;

    [Header("Panels")]
    public GameObject bookPanel;
    public GameObject pausePanel;

    [Header("Timer")]
    public TMP_Text timerText;
    public float gameDuration = 10f;
    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;

    [Header("Lives")]
    public TMP_Text livesText;
    public int lives;

    [Header("Coins")]    
    public TMP_Text coinsText;
    public int coins;

    private bool isOpen;

    void Start()
    {
        bookPanel.SetActive(false);
        pausePanel.SetActive(false);
        StartTimer();
        UpdateLivesText();
        isOpen = false;
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

    private void Timer()
    {
        if (gameEnded || !timerRunning || isPaused)
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
            EndGame();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "" + Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    public void UpdateLivesText()
    {
        if(livesText!=null)
        {
            livesText.text = lives.ToString();
        }
    }

    public void UpdateCoinsText()
    {
        if(coinsText!=null)
        {
            coinsText.text = coins.ToString();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ToggleBook()
    {
        if(isOpen)
        {
            CloseBook();
        }
        else
        {
            OpenBook();
        }
    }

    public void OpenBook()
    {
        bookPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
        isOpen = false;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        timerRunning = false;
        Debug.Log("Game Paused");
        pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        timerRunning = true;
        Debug.Log("Game Resumed");
        pausePanel.SetActive(false);
    }

    public void EndGame()
    {
        gameEnded = true;
        timerRunning = false;
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }
}
