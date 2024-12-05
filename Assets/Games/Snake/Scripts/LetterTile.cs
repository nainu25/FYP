using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    private LetterPronunciationManager manager;
    private GameManager gm;
    private int errorCount = 0;

    /// <summary>
    /// Initializes the letter tile with a specific letter and pronunciation manager.
    /// </summary>
    /// <param name="letter">The letter represented by this tile.</param>
    /// <param name="manager">The pronunciation manager instance.</param>
    public void Setup(string letter, LetterPronunciationManager manager)
    {
        this.letter = letter;
        this.manager = manager;
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

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
        if (collision.CompareTag("Player"))
        {
            if (letter != manager.CurrentLetter)
            {
                HandleIncorrectSelection();
            }
            else
            {
                manager.CorrectSelection();
            }
        }
    }

    /// <summary>
    /// Handles logic for an incorrect letter selection.
    /// </summary>
    private void HandleIncorrectSelection()
    {
        Debug.Log($"Incorrect selection: {letter}. Expected: {manager.CurrentLetter}");

        errorCount++;
        Debug.Log($"Error Count: {errorCount}");

        if (gm != null)
        {
            SaveErrorCount(gm.level);
        }
    }

    /// <summary>
    /// Saves the error count for the current level to PlayerPrefs.
    /// </summary>
    /// <param name="level">The current game level.</param>
    private void SaveErrorCount(int level)
    {
        string key = $"Error Count Lv {level}";
        PlayerPrefs.SetInt(key, errorCount);
        PlayerPrefs.Save();

        Debug.Log($"Error count for level {level} saved as {errorCount}.");
    }
}
