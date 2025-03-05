using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RnCGameManager : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject taskPanel; 
    public TMP_Text taskText; 
    public Button startTimer;
    public Button completeTaskButton; 

    private float taskStartTime;
    private bool taskActive = false;
    private int taskIndex = 1; 
    private string[] readingTasks =
    {
        "The quick brown fox jumps over the lazy dog.",
        "Unity is a powerful game engine used by developers worldwide.",
        "Reading speed helps measure comprehension and fluency."
    };

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        taskPanel.SetActive(false); 
        startTimer.onClick.AddListener(StartTimer);
        completeTaskButton.onClick.AddListener(CompleteTask);
    }

    private void Update()
    {
        if (!taskActive && player.turns > 0 && player.turns % 3 == 0) 
        {
            ShowTaskPanel();
        }
    }

    void ShowTaskPanel()
    {
        taskPanel.SetActive(true);
        string task = GetReadingTask();
        taskText.text = task;
        Time.timeScale = 0; 
        taskActive = true;
    }

    void StartTimer()
    {
        taskStartTime = Time.realtimeSinceStartup; 
    }

    void CompleteTask()
    {
        float timeTaken = Time.realtimeSinceStartup - taskStartTime; 
        int wordCount = CountWords(taskText.text); 
        float wordsPerMinute = (wordCount / timeTaken) * 60; 

        Debug.Log($"Task {taskIndex} - Time Taken: {timeTaken:F2}s, Words: {wordCount}, WPM: {wordsPerMinute:F2}");

        
        taskPanel.SetActive(false);
        Time.timeScale = 1;
        taskActive = false;
        taskIndex++;
    }

    string GetReadingTask()
    {
        return readingTasks[(taskIndex - 1) % readingTasks.Length]; 
    }

    int CountWords(string text)
    {
        return text.Split(' ').Length; 
    }
}
