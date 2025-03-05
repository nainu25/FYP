using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognitionTest : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private TextMeshProUGUI text;

    public PlayerMovement player;
    public GameObject taskPanel; 
    public TMP_Text taskText; 
    public Button startTimer;
    public Button completeTaskButton; 

    private float taskStartTime;
    public bool taskActive = false;
    private int taskIndex = 1;
    private string[] readingTasks =
    {
        "The quick brown fox jumps over the lazy dog.",
        "Unity is a powerful game engine used by developers worldwide.",
        "Reading speed helps measure comprehension and fluency."
    };

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    private void Awake()
    {
        taskPanel.SetActive(false); // Hide at start
        startTimer.onClick.AddListener(StartTimer);
        //completeTaskButton.onClick.AddListener(CompleteTask);
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        stopButton.interactable = false;
    }

    private void Update()
    {
        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
        if (!taskActive && player.tempTurn > 0 && player.tempTurn % 3 == 0)
        {
            ShowTaskPanel();
        }
    }

    private void StartRecording()
    {
        text.color = Color.green;
        text.text = "Recording...";
        startButton.interactable = false;
        stopButton.interactable = true;
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording()
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        StopTimer();
        SendRecording();
        CompleteTask();

    }

    private string recognizedText = "";

    private void SendRecording()
    {
        text.color = Color.yellow;
        text.text = "Sending...";
        stopButton.interactable = false;

        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            text.color = Color.green;
            text.text = response;
            startButton.interactable = true;

            recognizedText = response;
            MeasureAccuracy(); 

        }, error => {
            text.color = Color.red;
            text.text = error;
            startButton.interactable = true;
        });
    }

    private void MeasureAccuracy()
    {
        string originalText = taskText.text.ToLower();
        string spokenText = recognizedText.ToLower();

        int originalWordCount = CountWords(originalText);
        int matchedWords = CountMatchingWords(originalText, spokenText);

        float accuracy = (float)matchedWords / originalWordCount * 100;

        Debug.Log($"Accuracy: {accuracy:F2}%");
    }

    private int CountMatchingWords(string original, string spoken)
    {
        string[] originalWords = original.Split(' ');
        string[] spokenWords = spoken.Split(' ');

        int matchCount = 0;

        foreach (string word in spokenWords)
        {
            if (originalWords.Contains(word))
            {
                matchCount++;
            }
        }

        return matchCount;
    }
    public bool AllTasksCompleted()
    {
        return taskIndex > readingTasks.Length;
    }




    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    public void ShowTaskPanel()
    {
        taskPanel.SetActive(true);
        string task = GetReadingTask();
        taskText.text = task;
        Time.timeScale = 0;
        taskActive = true;
        Debug.Log(taskActive);
    }

    void StartTimer()
    {
        taskStartTime = Time.realtimeSinceStartup;
    }

    void StopTimer()
    {
        float timeTaken = Time.realtimeSinceStartup - taskStartTime;
        int wordCount = CountWords(taskText.text);
        float wordsPerMinute = (wordCount / timeTaken) * 60;

        Debug.Log($"Task {taskIndex} - Time Taken: {timeTaken:F2}s, Words: {wordCount}, WPM: {wordsPerMinute:F2}");
    }

    void CompleteTask()
    {
        taskPanel.SetActive(false);
        Time.timeScale = 1;
        taskIndex++;

        taskActive = false;

        player.tempTurn++;
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
