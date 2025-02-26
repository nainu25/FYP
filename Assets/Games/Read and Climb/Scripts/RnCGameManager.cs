using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RnCGameManager : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject taskPanel; // Assign in Inspector
    public TMP_Text taskText; // Assign in Inspector
    public Button startTimer;
    public Button completeTaskButton; // Assign in Inspector

    private float taskStartTime;
    private bool taskActive = false;
    private int taskIndex = 1; // Task number counter
    private string[] readingTasks =
    {
        "The quick brown fox jumps over the lazy dog.",
        "Unity is a powerful game engine used by developers worldwide.",
        "Reading speed helps measure comprehension and fluency."
    };

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        taskPanel.SetActive(false); // Hide at start
        startTimer.onClick.AddListener(StartTimer);
        completeTaskButton.onClick.AddListener(CompleteTask);
    }

    private void Update()
    {
        if (!taskActive && player.turns > 0 && player.turns % 3 == 0) // Show every 3 turns
        {
            ShowTaskPanel();
        }
    }

    void ShowTaskPanel()
    {
        taskPanel.SetActive(true);
        string task = GetReadingTask();
        taskText.text = task;
        Time.timeScale = 0; // Pause game
        taskActive = true;
    }

    void StartTimer()
    {
        taskStartTime = Time.realtimeSinceStartup; // Start timer
    }

    void CompleteTask()
    {
        float timeTaken = Time.realtimeSinceStartup - taskStartTime; // Calculate time taken
        int wordCount = CountWords(taskText.text); // Count words
        float wordsPerMinute = (wordCount / timeTaken) * 60; // Calculate WPM

        Debug.Log($"Task {taskIndex} - Time Taken: {timeTaken:F2}s, Words: {wordCount}, WPM: {wordsPerMinute:F2}");

        // Hide panel & resume game
        taskPanel.SetActive(false);
        Time.timeScale = 1;
        taskActive = false;
        taskIndex++;
    }

    string GetReadingTask()
    {
        return readingTasks[(taskIndex - 1) % readingTasks.Length]; // Cycle through tasks
    }

    int CountWords(string text)
    {
        return text.Split(' ').Length; // Count words in the task text
    }
}
