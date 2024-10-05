using UnityEngine;

public class Food : MonoBehaviour
{
    private LetterTile lt;
    private LetterPronunciationManager pronunciationManager;

    void Start()
    {
        if (lt == null)
        {
            lt = GetComponent<LetterTile>();
        }

        if (pronunciationManager == null)
        {
            pronunciationManager = FindObjectOfType<LetterPronunciationManager>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SnakeHead")
        {
            collision.GetComponentInParent<SnakeController>().Grow();
            if (lt.letter == pronunciationManager.CurrentLetter)
            {
                Destroy(gameObject);
                pronunciationManager.CorrectSelection();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
