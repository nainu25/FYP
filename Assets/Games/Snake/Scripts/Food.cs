using UnityEngine;

public class Food : MonoBehaviour
{
    private LetterTile letterTile;
    private LetterPronunciationManager pronunciationManager;

    void Start()
    {
        // Initialize references
        letterTile = GetComponent<LetterTile>();
        pronunciationManager = FindObjectOfType<LetterPronunciationManager>();

        if (letterTile == null)
        {
            Debug.LogWarning($"LetterTile component is missing on {gameObject.name}");
        }

        if (pronunciationManager == null)
        {
            Debug.LogError("LetterPronunciationManager not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the snake's head
        if (collision.gameObject.name == "SnakeHead")
        {
            SnakeController snakeController = collision.GetComponentInParent<SnakeController>();
            if (snakeController != null)
            {
                snakeController.Grow();
            }
            else
            {
                Debug.LogWarning("SnakeController not found on parent object.");
            }

            HandleCollision();
        }
    }

    private void HandleCollision()
    {
        if (pronunciationManager != null && letterTile != null)
        {
            if (letterTile.letter == pronunciationManager.CurrentLetter)
            {
                pronunciationManager.CorrectSelection();
            }
        }
        else
        {
            Debug.LogWarning("Unable to process collision due to missing components.");
        }

        // Destroy the food object regardless of correctness
        Destroy(gameObject);
    }
}
