using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellBookManager : MonoBehaviour
{
    private SBQGameManager sbqGameManager;
    private EnemyController enemyController;
    private PlayerController playerController;

    [Header("UI Elements")]
    public GameObject[] images;
    public Button[] optionButtons;

    [Header("Round Data")]
    private readonly string[][] roundOptions =
    {
        new string[] { "Cat", "Kat", "Cet" },
        new string[] { "Son", "Sun", "San" },
        new string[] { "Red", "Reb", "Rad" }
    };

    private readonly int[] correctOptionIndices = { 0, 1, 0 }; // Correct answers for rounds

    private int correctOptionIndex;

    private void Start()
    {
        sbqGameManager = GetComponent<SBQGameManager>();
        enemyController = FindObjectOfType<EnemyController>();
        playerController = FindObjectOfType<PlayerController>();

        if (sbqGameManager != null)
        {
            sbqGameManager.round = 1;
            DisplayQuestionForCurrentRound();
        }

        if (enemyController != null)
        {
            enemyController.OnAttackCompleted += HandleEnemyAttackCompleted;
        }

        if (playerController != null)
        {
            playerController.OnAttackCompleted += HandlePlayerAttackCompleted;
        }
    }

    private void OnDestroy()
    {
        if (enemyController != null)
        {
            enemyController.OnAttackCompleted -= HandleEnemyAttackCompleted;
        }

        if (playerController != null)
        {
            playerController.OnAttackCompleted -= HandlePlayerAttackCompleted;
        }
    }

    private void DisplayQuestionForCurrentRound()
    {
        if (sbqGameManager.round > roundOptions.Length || sbqGameManager.round > images.Length)
        {
            Debug.LogWarning("Invalid round setup. Ensure all rounds and images are configured.");
            sbqGameManager.EndGame();
            return;
        }

        Debug.Log($"Displaying question for round: {sbqGameManager.round}");
        CloseAllImages();

        // Activate the corresponding image for the current round
        images[sbqGameManager.round - 1].SetActive(true);

        // Get options and correct index for the current round
        string[] options = roundOptions[sbqGameManager.round - 1];
        correctOptionIndex = correctOptionIndices[sbqGameManager.round - 1];

        // Set up option buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(i < options.Length);
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    private void OnOptionSelected(int chosenIndex)
    {
        if (chosenIndex == correctOptionIndex)
        {
            Debug.Log($"Correct choice! Round: {sbqGameManager.round}");
            sbqGameManager.round++;
            playerController.Attack();
        }
        else
        {
            Debug.Log($"Incorrect choice! Enemy attacks.");
            enemyController.Attack();
            optionButtons[chosenIndex].gameObject.SetActive(false);
        }

        sbqGameManager.CloseBook();
    }

    private void CloseAllImages()
    {
        foreach (GameObject img in images)
        {
            img.SetActive(false);
        }
    }

    private void HandleEnemyAttackCompleted()
    {
        Debug.Log("Enemy attack completed. Reopening the book.");
        sbqGameManager.OpenBook();
    }

    private void HandlePlayerAttackCompleted()
    {
        Debug.Log("Player attack completed. Preparing for the next round.");
        if (sbqGameManager.round <= images.Length)
        {
            DisplayQuestionForCurrentRound();
            sbqGameManager.OpenBook();
        }
        else
        {
            Debug.Log("All rounds completed. Ending the game.");
            sbqGameManager.EndGame();
        }
    }
}
