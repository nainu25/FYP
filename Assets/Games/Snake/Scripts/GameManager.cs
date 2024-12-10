using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 10f;
    public int initialLives = 5;

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text livesText;
    public TMP_Text scoreText;

    [Header("End Panel")]
    public GameObject endPanel;
    public TMP_Text endScoreText;
    public Button nextbutton;

    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;

    private LetterTile lt;
    public AudioController ac;
    private int lives;
    public int score;
    public int level;

    DataSaver dataSaver;

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
        ac = FindFirstObjectByType<AudioController>();
        dataSaver = FindFirstObjectByType<DataSaver>();

        // Update UI
        UpdateLivesUI();
        UpdateScoreText();
        ResetTimer();
        ToggleEndPanel();

        // Resume game
        gameEnded = false;
        Time.timeScale = 1f;

        lt = new LetterTile();
        lt.SaveErrorCount(level, 0);
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

    public void UpdateEndPanelScore()
    {
        if (endScoreText != null)
        {
            endScoreText.text = score.ToString();
        }
    }

    public void ToggleEndPanel()
    {
        if (endPanel != null)
        {
            endPanel.SetActive(!endPanel.activeSelf);
            if(lives>0)
            {
                nextbutton.gameObject.SetActive(true);
            }
            else
            {
                nextbutton.gameObject.SetActive(false);
            }
        }
    }


    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            EndGame();
            ac.PlayAudio("Game Over");
        }
        else
        {
            ResetTimer();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene($"Snake Game Lv {level}");
    }

    public void Home()
    {
        SceneManager.LoadScene("Game Selector");
    }

    public void EndGame()
    {
        gameEnded = true;
        timerRunning = false;

        ToggleEndPanel();
        UpdateEndPanelScore();
        

        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        InitializeGame();
    }
}
