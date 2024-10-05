using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    public LetterPronunciationManager pronunciationManager;

    public void Setup(string letter, LetterPronunciationManager pronunciationManager)
    {
        this.letter = letter;
        this.pronunciationManager = pronunciationManager;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SnakeHead"))
        {
            if (letter == pronunciationManager.CurrentLetter) // Use the current letter pronounced
            {
                // Handle correct selection
                Destroy(gameObject);
                pronunciationManager.CorrectSelection();
            }
            else
            {
                // Handle incorrect selection if necessary
                Destroy(gameObject);
            }
        }
    }*/
}
