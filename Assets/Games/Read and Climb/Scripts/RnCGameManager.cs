using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RnCGameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private SpeechRecognitionTest srt;
    [SerializeField] private GameObject pausePanel;

    private float avgScore;
    private float[] scores;
    private const int taskCount = 7; // Define the total number of tasks
    private bool isPaused = false;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        avgScore = 0;
        scores = new float[taskCount]; // Initialize array

        if (pausePanel != null)
            pausePanel.SetActive(false); // Ensure pause panel is hidden at start
    }

    public void Continue() => LoadScene("Game Selector");

    public void Home() => LoadScene("Main Menu");

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Ensure time is reset when loading a new scene
        SceneManager.LoadScene(sceneName);
    }

    public void CalculateScore()
    {
        float totalScore = 0;
        int validScores = 0;

        for (int i = 0; i < taskCount; i++)
        {
            string key = $"Task{i + 1}_Accuracy";

            if (PlayerPrefs.HasKey(key))
            {
                scores[i] = PlayerPrefs.GetFloat(key);
                totalScore += scores[i];
                validScores++;
            }
            else
            {
                scores[i] = -1; // Indicates missing score
                Debug.LogWarning($"Missing score for {key}");
            }
        }

        avgScore = (validScores > 0) ? totalScore / validScores : 0;
        scoreText.text = $"{avgScore:F2}%"; // Display with 2 decimal places
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            isPaused = true;

            if (pausePanel != null)
                pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // Resume the game
            isPaused = false;

            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }
}
