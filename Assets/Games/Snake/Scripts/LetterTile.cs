using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    private LetterPronunciationManager manager;
    private GameManager gm;
    private AudioController audioController;

    public void Setup(string letter, LetterPronunciationManager manager)
    {
        this.letter = letter;
        this.manager = manager;
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        audioController = FindObjectOfType<AudioController>();

        if (gm == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        if (manager == null)
        {
            Debug.LogError("LetterPronunciationManager not set. Did you forget to call Setup?");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SnakeHead")
        {
            if (letter != manager.CurrentLetter)
            {
                HandleIncorrectSelection();
            }
            else
            {
                manager.CorrectSelection();
                audioController.PlayAudio("Eat");
                Debug.Log("Correct selection detected.");
            }
        }
    }

    private void HandleIncorrectSelection()
    {
        audioController.PlayAudio("Wrong Letter");
        int errorCount = GetErrorCount(gm.level);

        Debug.Log($"Incorrect selection: {letter}. Expected: {manager.CurrentLetter}");

        // Increment and save the error count
        errorCount++;
        SaveErrorCount(gm.level, errorCount);

        Debug.Log($"Error Count updated to: {errorCount}");
    }

    private int GetErrorCount(int level)
    {
        string key = $"Error Count Lv {level}";
        return PlayerPrefs.GetInt(key, 0); // Default to 0 if key does not exist
    }

    public void SaveErrorCount(int level, int errorCount)
    {
        string key = $"Error Count Lv {level}";
        PlayerPrefs.SetInt(key, errorCount);
        PlayerPrefs.Save();

        Debug.Log($"Error count for level {level} saved as {errorCount}.");
    }
}
