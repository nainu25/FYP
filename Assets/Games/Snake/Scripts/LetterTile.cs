using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    private LetterPronunciationManager manager;

    public void Setup(string letter, LetterPronunciationManager manager)
    {
        this.letter = letter;
        this.manager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (letter != manager.CurrentLetter)
            {
                Debug.Log($"Incorrect selection: {letter}. Expected: {manager.CurrentLetter}");
            }
            else
            {
                manager.CorrectSelection();
            }
        }
    }
}
