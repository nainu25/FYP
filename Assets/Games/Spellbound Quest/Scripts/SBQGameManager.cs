using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SBQGameManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool gameEnded = false;
    private bool isOpen = false;

    [Header("Panels")]
    public GameObject bookPanel;
    public GameObject pausePanel;
    public GameObject endPanel;
    public Button nextButton;

    [Header("Timer")]
    public TMP_Text timerText;
    public float gameDuration = 10f;
    private float timeRemaining;
    private bool timerRunning = false;

    [Header("Lives")]
    public TMP_Text livesText;
    public int lives;

    [Header("Coins")]
    public TMP_Text coinsText;
    public int coins;

    [Header("Game Info")]
    public int round;
    public int level;
    public TMP_Text scoreText;

    private AudioController audioController;

    public int age;



    private void Start()
    {
        audioController = FindObjectOfType<AudioController>();
        //age = PlayerPrefs.GetInt("Age");
        InitializeGame();
    }

    private void Update()
    {
        HandleTimer();
    }

    private void InitializeGame()
    {
        SetPanelActive(bookPanel, false);
        SetPanelActive(pausePanel, false);
        SetPanelActive(endPanel, false);

        StartTimer();
        UpdateLivesText();
        UpdateCoinsText();

        Time.timeScale = 1;
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

    private void HandleTimer()
    {
        if (gameEnded || !timerRunning || isPaused) return;

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
            timerText.text = $"{Mathf.Ceil(timeRemaining)}s";
        }
    }

    public void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    public void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ToggleBook()
    {
        if (isOpen)
            CloseBook();
        else
            OpenBook();
    }

    public void Retry()
    {
        LoadScene($"SBQ Level {level}");
    }

    public void NextLevel()
    {
        if (level == 5)
        {
            LoadScene("Game Selector");
        }
        else if (level >= 0 && level < 5)
        {
            LoadScene($"SBQ Level {level + 1}");
        }
    }


    public void Home()
    {
        LoadScene("Main Menu");
    }

    public void OpenBook()
    {
        SetPanelActive(bookPanel, true);
        isOpen = true;
    }

    public void CloseBook()
    {
        SetPanelActive(bookPanel, false);
        isOpen = false;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        timerRunning = false;

        Debug.Log("Game Paused");
        SetPanelActive(pausePanel, true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        timerRunning = true;

        Debug.Log("Game Resumed");
        SetPanelActive(pausePanel, false);
    }

    public void EndGame()
    {
        gameEnded = true;
        timerRunning = false;
        Time.timeScale = 0f;

        Debug.Log("Game Over!");
        SetPanelActive(endPanel, true);
        if(lives > 0 )
        {
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }
        SavePlayerPrefs();
        DisplayScore();
    }

    private void DisplayScore()
    {
        if (scoreText != null)
        {
            int levelCoins = PlayerPrefs.GetInt($"SBQ Lv{level} Coins", 0);
            scoreText.text = (levelCoins * 100).ToString();
        }
    }

    private void SavePlayerPrefs()
    {
        string coinsKey = $"SBQ Lv{level} Coins";
        string livesKey = $"SBQ Lv{level} Lives";

        PlayerPrefs.SetInt(coinsKey, coins);
        PlayerPrefs.SetInt(livesKey, lives);
        PlayerPrefs.Save();
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }
}
