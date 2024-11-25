using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Add this if you use TextMeshPro

public class SBQGameManager : MonoBehaviour
{
    private bool isPaused = false;

    [Header("Panels")]
    public GameObject bookPanel;
    public GameObject pausePanel;
    public GameObject endPanel;

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

    public int round;
    public int level;

    public TMP_Text scoreText;

    SpellBookManager sbm;


    private bool isOpen;

    void Start()
    {
        sbm = gameObject.GetComponent<SpellBookManager>();
        bookPanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        StartTimer();
        UpdateLivesText();
        isOpen = false;
        Time.timeScale = 1;
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

    public void Retry()
    {
        if(level == 1)
        {
            SceneManager.LoadScene("SBQ Level 1");
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("SBQ Level 2");
        }
        else if (level == 3)
        {
            SceneManager.LoadScene("SBQ Level 3");
        }
        else if (level == 4)
        {
            SceneManager.LoadScene("SBQ Level 4");
        }
        else if (level == 5)
        {
            SceneManager.LoadScene("SBQ Level 5");
        }
    }

    public void NextLevel()
    {
        if(level==1)
        {
            SceneManager.LoadScene("SBQ Level 2");
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
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
        SavePlayerPrefs();
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        endPanel.SetActive(true);
        scoreText.text = (PlayerPrefs.GetInt("SBQ Lv1 Coins") * 100).ToString();

    }
    
    void SavePlayerPrefs()
    {
        if(level==1)
        {
            PlayerPrefs.SetInt("SBQ Lv1 Coins", coins);
            PlayerPrefs.SetInt("SBQ Lv1 Lives", lives);
            PlayerPrefs.Save();
        }
        else if(level == 2)
        {
            PlayerPrefs.SetInt("SBQ Lv2 Coins", coins);
            PlayerPrefs.SetInt("SBQ Lv2 Lives", lives);
            PlayerPrefs.Save();
        }
        else if(level == 3)
        {
            PlayerPrefs.SetInt("SBQ Lv3 Coins", coins);
            PlayerPrefs.SetInt("SBQ Lv3 Lives", lives);
            PlayerPrefs.Save();
        }
        else if(level == 4)
        {
            PlayerPrefs.SetInt("SBQ Lv4 Coins", coins);
            PlayerPrefs.SetInt("SBQ Lv4 Lives", lives);
            PlayerPrefs.Save();
        }
        else if(level == 5) 
        {
            PlayerPrefs.SetInt("SBQ Lv5 Coins", coins);
            PlayerPrefs.SetInt("SBQ Lv5 Lives", lives);
            PlayerPrefs.Save();
        }
    }
}
