using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    private LetterPronunciationManager manager;
    int errorCount;

    public void Setup(string letter, LetterPronunciationManager manager)
    {
        this.letter = letter;
        this.manager = manager;
    }

    private void Start()
    {
        errorCount = PlayerPrefs.GetInt("Error Count", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (letter != manager.CurrentLetter)
            {
                Debug.Log($"Incorrect selection: {letter}. Expected: {manager.CurrentLetter}");
                errorCount++;
                Debug.Log(errorCount);
                PlayerPrefs.SetInt("Error Count", errorCount);
                PlayerPrefs.Save();
            }
            else
            {
                manager.CorrectSelection();
            }
        }
    }
}
