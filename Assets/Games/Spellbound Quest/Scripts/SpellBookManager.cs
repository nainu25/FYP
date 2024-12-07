using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookManager : MonoBehaviour
{
    private SBQGameManager sbqGameManager;
    private EnemyController enemyController;
    private PlayerController playerController;

    [Header("UI Elements")]
    public GameObject[] images;
    public Button[] optionButtons;

    [Header("Round Data")]
    private string[][] roundOptions; // Dynamic options
    private int[] correctOptionIndices; // Dynamic correct answers
    private int correctOptionIndex;

    private void Start()
    {
        sbqGameManager = GetComponent<SBQGameManager>();
        enemyController = FindObjectOfType<EnemyController>();
        playerController = FindObjectOfType<PlayerController>();

        if (sbqGameManager != null)
        {
            sbqGameManager.round = 1;
            SetOptionsForLevel(sbqGameManager.level); // Set options based on the current level
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

    private void SetOptionsForLevel(int level)
    {
        switch (level)
        {
            case 1:
                roundOptions = new string[][]
                {
                    new string[] { "Cat", "Kat", "Cet" },
                    new string[] { "Son", "Sun", "San" },
                    new string[] { "Red", "Reb", "Rad" }
                };
                correctOptionIndices = new int[] { 0, 1, 0 };
                break;

            case 2:
                roundOptions = new string[][]
                {
                    new string[] { "Moon", "Moun", "Muun" },
                    new string[] { "Grwen", "Green", "Grean" },
                    new string[] { "Ster", "Star", "Stir" }
                };
                correctOptionIndices = new int[] { 0, 1, 1 };
                break;

            case 3:
                roundOptions = new string[][]
                {
                    new string[] { "Flame", "Flaem", "Fleam" },
                    new string[] { "Stoen", "Stoan", "Stone" },
                    new string[] { "Bright", "Bryght", "Brigth" }
                };
                correctOptionIndices = new int[] { 0, 2, 0 };
                break;

            case 4:
                roundOptions = new string[][]
                {
                    new string[] { "Castle", "Castel", "Castal" },
                    new string[] { "Shadow", "Sahdow", "Shedow" },
                    new string[] { "Ghsot", "Gohst", "Ghost" }
                };
                correctOptionIndices = new int[] { 0, 0, 2 };
                break;

            case 5:
                roundOptions = new string[][]
                {
                    new string[] { "Crysal", "Crystel", "Crystle" },
                    new string[] { "Twinkle", "Twinkel", "Twincle" },
                    new string[] { "Dungoen", "Dungeon", "Dungion" }
                };
                correctOptionIndices = new int[] { 0, 0, 1 };
                break;

            default:
                Debug.LogError("Invalid level! Defaulting to level 1 setup.");
                roundOptions = new string[][] { };
                correctOptionIndices = new int[] { };
                break;
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
        }
        else
        {
            Debug.Log("All rounds completed. Ending the game.");
            sbqGameManager.EndGame();
        }
    }
}
