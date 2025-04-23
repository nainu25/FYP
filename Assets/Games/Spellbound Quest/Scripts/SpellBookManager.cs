using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



/// <summary>
/// score is being stored OnOptionSelected method
/// </summary>
public class SpellBookManager : MonoBehaviour
{
    private SBQGameManager sbqGameManager;
    private EnemyController enemyController;
    private PlayerController playerController;
    private AudioController audioController;
    private int errors;

    [Header("UI Elements")]
    public List<GameObject> images; // Single list for all images
    public Button[] optionButtons;

    [Header("Round Data")]
    private List<List<string>> roundOptions; // Dynamic options based on age and level
    private List<int> correctOptionIndices;  // Dynamic correct answers based on age and level
    private int correctOptionIndex;

    int score;

    private void Start()
    {
        sbqGameManager = GetComponent<SBQGameManager>();
        enemyController = FindObjectOfType<EnemyController>();
        playerController = FindObjectOfType<PlayerController>();
        audioController = FindObjectOfType<AudioController>();
        errors = 0;


        if (sbqGameManager != null)
        {
            sbqGameManager.round = 1;
            SetOptionsForLevelAndAge(sbqGameManager.level, sbqGameManager.age); // Age and level-based setup
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

    private void SetOptionsForLevelAndAge(int level, int age)
    {
        roundOptions = new List<List<string>>();
        correctOptionIndices = new List<int>();

        switch (age)
        {
            case 6:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Cat", "Kat", "Cet" });
                        roundOptions.Add(new List<string> { "Dug", "Dag", "Dog" });
                        roundOptions.Add(new List<string> { "Sin", "Sun", "San" });
                        correctOptionIndices.AddRange(new[] { 0, 2, 1 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Beg", "Bag", "Bog" });
                        roundOptions.Add(new List<string> { "Rid", "Rad", "Red" });
                        roundOptions.Add(new List<string> { "Hat", "Het", "Hut" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Cap", "Cip", "Cup" });
                        roundOptions.Add(new List<string> { "Ant", "Ent", "Ont" });
                        roundOptions.Add(new List<string> { "Bed", "Bad", "Bud" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Ship", "Shap", "Shup" });
                        roundOptions.Add(new List<string> { "Fash", "Fish", "Fesh" });
                        roundOptions.Add(new List<string> { "Troe", "Trey", "Tree" });
                        correctOptionIndices.AddRange(new[] { 0, 1, 2 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Put", "Pot", "Pat" });
                        roundOptions.Add(new List<string> { "Bule", "Boll", "Ball" });
                        roundOptions.Add(new List<string> { "Frog", "Frug", "Freg" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 0 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 6!");
                        break;
                }
                break;

            case 7:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Bog", "Bug", "Bag" });
                        roundOptions.Add(new List<string> { "Aye", "Eie", "Eye" });
                        roundOptions.Add(new List<string> { "Bey", "Bay", "Boy" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 2 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Han", "Hon", "Hen" });
                        roundOptions.Add(new List<string> { "Cew", "Cow", "Caw" });
                        roundOptions.Add(new List<string> { "Two", "Toe", "Too" });
                        correctOptionIndices.AddRange(new[] { 2, 1, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Mulk", "Milk", "Malk" });
                        roundOptions.Add(new List<string> { "Stir", "Star", "Ster" });
                        roundOptions.Add(new List<string> { "Moun", "Moon", "Maan" });
                        correctOptionIndices.AddRange(new[] { 1, 1, 1 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Hume", "Home", "Heme" });
                        roundOptions.Add(new List<string> { "Hirse", "Harse", "Horse" });
                        roundOptions.Add(new List<string> { "Rume", "Room", "Reme" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 1 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Dear", "Deer", "Dare" });
                        roundOptions.Add(new List<string> { "White", "Wite", "Whet" });
                        roundOptions.Add(new List<string> { "Photo", "Poto", "Phato" });
                        correctOptionIndices.AddRange(new[] { 1, 0, 0 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 7!");
                        break;
                }
                break;
            
            case 8:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Mop", "Map", "Mip" });
                        roundOptions.Add(new List<string> { "City", "Sity", "Cety" });
                        roundOptions.Add(new List<string> { "Gald", "Gold", "Guld" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Shart", "Shirt", "Shert" });
                        roundOptions.Add(new List<string> { "Hand", "Hend", "Hind" });
                        roundOptions.Add(new List<string> { "Clup", "Clap", "Clip" });
                        correctOptionIndices.AddRange(new[] { 1, 0, 1 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Huppy", "Hippy", "Happy" });
                        roundOptions.Add(new List<string> { "Earth", "Erth", "Arth" });
                        roundOptions.Add(new List<string> { "Magik", "Magic", "Mugic" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 1 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Classroom", "Clasroom", "Clasrum" });
                        roundOptions.Add(new List<string> { "Flouer", "Flower", "Floer" });
                        roundOptions.Add(new List<string> { "Water", "Watar", "Witer" });
                        correctOptionIndices.AddRange(new[] { 0, 1, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Vakation", "Vacetion", "Vacation" });
                        roundOptions.Add(new List<string> { "Important", "Importent", "Impertant" });
                        roundOptions.Add(new List<string> { "Strangor", "Stranger", "Strangar" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 1 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 8!");
                        break;
                }
                break;

            case 9:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Time", "Tame", "Tome" });
                        roundOptions.Add(new List<string> { "Sistor", "Sister", "Sastar" });
                        roundOptions.Add(new List<string> { "Flowers", "Flouwers", "Flouers" });
                        correctOptionIndices.AddRange(new[] { 0, 1, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Pork", "Perk", "Park" });
                        roundOptions.Add(new List<string> { "Uncle", "Unkil", "Ancle" });
                        roundOptions.Add(new List<string> { "Triangel", "Triangle", "Triengle" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Contry", "Country", "Cuntry" });
                        roundOptions.Add(new List<string> { "Balloone", "Balloon", "Balune" });
                        roundOptions.Add(new List<string> { "Weather", "Weathar", "Wether" });
                        correctOptionIndices.AddRange(new[] { 1, 1, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Nacher", "Nature", "Natar" });
                        roundOptions.Add(new List<string> { "Buterfly", "Butterfley", "Butterfly" });
                        roundOptions.Add(new List<string> { "Autumn", "Autem", "Autimn" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Honeybee", "Honibee", "Hunybee" });
                        roundOptions.Add(new List<string> { "Gardun", "Garden", "Gardan" });
                        roundOptions.Add(new List<string> { "Sparrow", "Sperow", "Sperrow" });
                        correctOptionIndices.AddRange(new[] { 0, 1, 0 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 9!");
                        break;
                }
                break;

            case 10:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Crod", "Crowd", "Croud" });
                        roundOptions.Add(new List<string> { "Cundy", "Cendy", "Candy" });
                        roundOptions.Add(new List<string> { "Knife", "Knive", "Knefe" });
                        correctOptionIndices.AddRange(new[] { 1, 2, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Clowd", "Claud", "Cloud" });
                        roundOptions.Add(new List<string> { "Shep", "Ship", "Sheep" });
                        roundOptions.Add(new List<string> { "Plaint", "Plent", "Plant" });
                        correctOptionIndices.AddRange(new[] { 2, 2, 2 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Recipie", "Resipe", "Recipe" });
                        roundOptions.Add(new List<string> { "Caterpillar", "Caterpiller", "Caterpillar" });
                        roundOptions.Add(new List<string> { "Murning", "Morning", "Marning" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 1 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Cral", "Craule", "Crawl" });
                        roundOptions.Add(new List<string> { "Wenesday", "Wensday", "Wednesday" });
                        roundOptions.Add(new List<string> { "Chocolate", "Chocolet", "Chocolat" });
                        correctOptionIndices.AddRange(new[] { 2, 2, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Evening", "Ewening", "Evenning" });
                        roundOptions.Add(new List<string> { "Prais", "Praise", "Praize" });
                        roundOptions.Add(new List<string> { "Bouquet", "Boquet", "Bouqet" });
                        correctOptionIndices.AddRange(new[] { 0, 1, 0 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 10!");
                        break;
                }
                break;

            case 11:
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Cricket", "Crickit", "Crecket" });
                        roundOptions.Add(new List<string> { "Animel", "Anemal", "Animal" });
                        roundOptions.Add(new List<string> { "Commuincation", "Comunication", "Communication" });
                        correctOptionIndices.AddRange(new[] { 0, 2, 2 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Enviroment", "Environment", "Envirenment" });
                        roundOptions.Add(new List<string> { "Education", "Eduacation", "Eductation" });
                        roundOptions.Add(new List<string> { "Scisors", "Scissors", "Sissers" });
                        correctOptionIndices.AddRange(new[] { 1, 0, 1 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Tollerance", "Tolerence", "Tolerance" });
                        roundOptions.Add(new List<string> { "Nutritional", "Nutritonal", "Nutritional" });
                        roundOptions.Add(new List<string> { "Umbrela", "Umbralla", "Umbrella" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 2 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Tsunami", "Sunami", "Tsunamie" });
                        roundOptions.Add(new List<string> { "Goverment", "Governement", "Government" });
                        roundOptions.Add(new List<string> { "Principal", "Prencipal", "Principel" });
                        correctOptionIndices.AddRange(new[] { 0, 2, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Apologie", "Apollogy", "Apology" });
                        roundOptions.Add(new List<string> { "Dentist", "Dentest", "Dentast" });
                        roundOptions.Add(new List<string> { "Truthefulness", "Truthfulnes", "Truthfulness" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 2 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 11!");
                        break;
                }
                break;

            default:
                Debug.LogError("Invalid age group! Defaulting to age 6 level 1 setup.");
                switch (level)
                {
                    case 1:
                        roundOptions.Add(new List<string> { "Cricket", "Crickit", "Crecket" });
                        roundOptions.Add(new List<string> { "Animel", "Anemal", "Animal" });
                        roundOptions.Add(new List<string> { "Commuincation", "Comunication", "Communication" });
                        correctOptionIndices.AddRange(new[] { 0, 2, 2 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Enviroment", "Environment", "Envirenment" });
                        roundOptions.Add(new List<string> { "Education", "Eduacation", "Eductation" });
                        roundOptions.Add(new List<string> { "Scisors", "Scissors", "Sissers" });
                        correctOptionIndices.AddRange(new[] { 1, 0, 1 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Tollerance", "Tolerence", "Tolerance" });
                        roundOptions.Add(new List<string> { "Nutritional", "Nutritonal", "Nutritional" });
                        roundOptions.Add(new List<string> { "Umbrela", "Umbralla", "Umbrella" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 2 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Tsunami", "Sunami", "Tsunamie" });
                        roundOptions.Add(new List<string> { "Goverment", "Governement", "Government" });
                        roundOptions.Add(new List<string> { "Principal", "Prencipal", "Principel" });
                        correctOptionIndices.AddRange(new[] { 0, 2, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Apologie", "Apollogy", "Apology" });
                        roundOptions.Add(new List<string> { "Dentist", "Dentest", "Dentast" });
                        roundOptions.Add(new List<string> { "Truthefulness", "Truthfulnes", "Truthfulness" });
                        correctOptionIndices.AddRange(new[] { 2, 0, 2 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 11!");
                        break;
                }
                break;
        }

    }

    private void DisplayQuestionForCurrentRound()
    {
        if (sbqGameManager.round > roundOptions.Count)
        {
            Debug.LogWarning("Invalid round setup. Ensure all rounds are configured.");
            sbqGameManager.EndGame();
            return;
        }

        Debug.Log($"Displaying question for round: {sbqGameManager.round}");
        CloseAllImages();

        // Calculate the image index based on age group and round
        int ageIndex = sbqGameManager.age - 6; // Age groups start at 6
        int imageIndex = ageIndex * 3 + (sbqGameManager.round - 1); // Each age group has 3 images
        if (imageIndex >= 0 && imageIndex < images.Count)
        {
            images[imageIndex].SetActive(true);
        }

        // Get options and correct index for the current round
        List<string> options = roundOptions[sbqGameManager.round - 1];
        correctOptionIndex = correctOptionIndices[sbqGameManager.round - 1];

        // Set up option buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(i < options.Count);
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
            if(sbqGameManager.level == 1)
            {
                score += 10;
                PlayerPrefs.SetInt($"Score {sbqGameManager.level}", score);
            }
            else if(sbqGameManager.level == 2)
            {
                score += 20;
                PlayerPrefs.SetInt($"Score {sbqGameManager.level}", score);
            }
            else if(sbqGameManager.level == 3)
            {
                score += 30;
                PlayerPrefs.SetInt($"Score {sbqGameManager.level}", score);
            }
            else if(sbqGameManager.level == 4)
            {
                score += 40;
                PlayerPrefs.SetInt($"Score {sbqGameManager.level}", score);
            }
            else if(sbqGameManager.level == 5)
            {
                score += 50;
                PlayerPrefs.SetInt($"Score {sbqGameManager.level}", score);
            }
        }
        else
        {
            Debug.Log($"Incorrect choice! Enemy attacks.");
            enemyController.Attack();
            if (sbqGameManager.level == 1)
            {
                errors++;
                PlayerPrefs.SetInt($"ErrorSBQ {sbqGameManager.level}", errors);
            }
            else if (sbqGameManager.level == 2)
            {
                errors++;
                PlayerPrefs.SetInt($"ErrorSBQ {sbqGameManager.level}", errors);
            }
            else if (sbqGameManager.level == 3)
            {
                errors++;
                PlayerPrefs.SetInt($"ErrorSBQ {sbqGameManager.level}", errors);
            }
            else if (sbqGameManager.level == 4)
            {
                errors++;
                PlayerPrefs.SetInt($"ErrorSBQ {sbqGameManager.level}", errors);
            }
            else if (sbqGameManager.level == 5)
            {
                errors++;
                PlayerPrefs.SetInt($"ErrorSBQ {sbqGameManager.level}", errors);
            }
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
        if (sbqGameManager.round <= roundOptions.Count)
        {
            DisplayQuestionForCurrentRound();
        }
        else
        {
            Debug.Log("All rounds completed. Ending the game.");
            sbqGameManager.EndGame();
            audioController.PlayAudio("Level Complete");
        }
    }
}
