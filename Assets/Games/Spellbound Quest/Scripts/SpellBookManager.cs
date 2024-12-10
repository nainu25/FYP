using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellBookManager : MonoBehaviour
{
    private SBQGameManager sbqGameManager;
    private EnemyController enemyController;
    private PlayerController playerController;
    private AudioController audioController;

    [Header("UI Elements")]
    public List<GameObject> images; // Single list for all images
    public Button[] optionButtons;

    [Header("Round Data")]
    private List<List<string>> roundOptions; // Dynamic options based on age and level
    private List<int> correctOptionIndices;  // Dynamic correct answers based on age and level
    private int correctOptionIndex;

    private void Start()
    {
        sbqGameManager = GetComponent<SBQGameManager>();
        enemyController = FindObjectOfType<EnemyController>();
        playerController = FindObjectOfType<PlayerController>();
        audioController = FindObjectOfType<AudioController>();

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
                        roundOptions.Add(new List<string> { "Dog", "Dug", "Dag" });
                        roundOptions.Add(new List<string> { "Sun", "San", "Sin" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Bag", "Beg", "Bog" });
                        roundOptions.Add(new List<string> { "Red", "Rid", "Rad" });
                        roundOptions.Add(new List<string> { "Hat", "Het", "Hut" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Cup", "Cap", "Cip" });
                        roundOptions.Add(new List<string> { "Ant", "Ent", "Ont" });
                        roundOptions.Add(new List<string> { "Bed", "Bad", "Bud" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Ship", "Shap", "Shup" });
                        roundOptions.Add(new List<string> { "Fish", "Fash", "Fesh" });
                        roundOptions.Add(new List<string> { "Tree", "Trey", "Troe" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Pot", "Put", "Pat" });
                        roundOptions.Add(new List<string> { "Ball", "Boll", "Bule" });
                        roundOptions.Add(new List<string> { "Frog", "Frug", "Freg" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
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
                        roundOptions.Add(new List<string> { "Bug", "Bog", "Bag" });
                        roundOptions.Add(new List<string> { "Eye", "Eie", "Aye" });
                        roundOptions.Add(new List<string> { "Boy", "Bay", "Bey" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Hen", "Hon", "Han" });
                        roundOptions.Add(new List<string> { "Cow", "Caw", "Cew" });
                        roundOptions.Add(new List<string> { "Two", "Toe", "Too" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Milk", "Mulk", "Malk" });
                        roundOptions.Add(new List<string> { "Star", "Stir", "Ster" });
                        roundOptions.Add(new List<string> { "Moon", "Moun", "Maan" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Home", "Hume", "Heme" });
                        roundOptions.Add(new List<string> { "Horse", "Harse", "Hirse" });
                        roundOptions.Add(new List<string> { "Room", "Rume", "Reme" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Deer", "Dear", "Dare" });
                        roundOptions.Add(new List<string> { "White", "Wite", "Whet" });
                        roundOptions.Add(new List<string> { "Photo", "Poto", "Phato" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
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
                        roundOptions.Add(new List<string> { "Map", "Mop", "Mip" });
                        roundOptions.Add(new List<string> { "City", "Sity", "Cety" });
                        roundOptions.Add(new List<string> { "Gold", "Gald", "Guld" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Shirt", "Shart", "Shert" });
                        roundOptions.Add(new List<string> { "Hand", "Hend", "Hind" });
                        roundOptions.Add(new List<string> { "Clap", "Clup", "Clip" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Happy", "Hippy", "Huppy" });
                        roundOptions.Add(new List<string> { "Earth", "Erth", "Arth" });
                        roundOptions.Add(new List<string> { "Magic", "Magik", "Mugic" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Classroom", "Clasroom", "Clasrum" });
                        roundOptions.Add(new List<string> { "Flower", "Flouer", "Floer" });
                        roundOptions.Add(new List<string> { "Water", "Watar", "Witer" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Vacation", "Vacetion", "Vaketion" });
                        roundOptions.Add(new List<string> { "Important", "Importent", "Impertant" });
                        roundOptions.Add(new List<string> { "Stranger", "Strangor", "Strangar" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
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
                        roundOptions.Add(new List<string> { "Sister", "Sistor", "Sastar" });
                        roundOptions.Add(new List<string> { "Flowers", "Flouwers", "Flouers" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Park", "Perk", "Pork" });
                        roundOptions.Add(new List<string> { "Uncle", "Unkil", "Ancle" });
                        roundOptions.Add(new List<string> { "Triangle", "Triangel", "Triengle" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Country", "Contry", "Cuntry" });
                        roundOptions.Add(new List<string> { "Balloon", "Baloone", "Balune" });
                        roundOptions.Add(new List<string> { "Weather", "Weathar", "Wether" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Nature", "Nacher", "Natar" });
                        roundOptions.Add(new List<string> { "Butterfly", "Butterfley", "Buterfly" });
                        roundOptions.Add(new List<string> { "Autumn", "Autem", "Autimn" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Honeybee", "Honibee", "Hunybee" });
                        roundOptions.Add(new List<string> { "Garden", "Gardun", "Gardan" });
                        roundOptions.Add(new List<string> { "Sparrow", "Sperow", "Sperrow" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
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
                        roundOptions.Add(new List<string> { "Crowd", "Crad", "Croud" });
                        roundOptions.Add(new List<string> { "Candy", "Cendy", "Cundy" });
                        roundOptions.Add(new List<string> { "Knife", "Knive", "Knefe" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Cloud", "Claud", "Clowd" });
                        roundOptions.Add(new List<string> { "Sheep", "Ship", "Shep" });
                        roundOptions.Add(new List<string> { "Plant", "Plent", "Plaint" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Recipe", "Resipe", "Recipie" });
                        roundOptions.Add(new List<string> { "Caterpillar", "Caterpiller", "Caterpillar" });
                        roundOptions.Add(new List<string> { "Morning", "Mourning", "Marning" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Crawl", "Craule", "Cral" });
                        roundOptions.Add(new List<string> { "Wednesday", "Wensday", "Wednsday" });
                        roundOptions.Add(new List<string> { "Chocolate", "Chocolet", "Chocolat" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Evening", "Ewening", "Evenning" });
                        roundOptions.Add(new List<string> { "Praise", "Prais", "Praize" });
                        roundOptions.Add(new List<string> { "Bouquet", "Boquet", "Bouqet" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
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
                        roundOptions.Add(new List<string> { "Animal", "Anemal", "Animel" });
                        roundOptions.Add(new List<string> { "Communication", "Comunication", "Commuincation" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 2:
                        roundOptions.Add(new List<string> { "Environment", "Enviroment", "Envirenment" });
                        roundOptions.Add(new List<string> { "Education", "Eduacation", "Eductation" });
                        roundOptions.Add(new List<string> { "Scissors", "Scisors", "Sissers" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 3:
                        roundOptions.Add(new List<string> { "Tolerance", "Tolerence", "Tollerance" });
                        roundOptions.Add(new List<string> { "Nutritional", "Nutritonal", "Nutritional" });
                        roundOptions.Add(new List<string> { "Umbrella", "Umbralla", "Umbrela" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 4:
                        roundOptions.Add(new List<string> { "Tsunami", "Sunami", "Tsunamie" });
                        roundOptions.Add(new List<string> { "Government", "Governement", "Goverment" });
                        roundOptions.Add(new List<string> { "Principal", "Prencipal", "Principel" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    case 5:
                        roundOptions.Add(new List<string> { "Apology", "Apollogy", "Apologie" });
                        roundOptions.Add(new List<string> { "Dentist", "Dentest", "Dentast" });
                        roundOptions.Add(new List<string> { "Truthfulness", "Truthfulnes", "Truthefulness" });
                        correctOptionIndices.AddRange(new[] { 0, 0, 0 });
                        break;
                    default:
                        Debug.LogError("Invalid level for age 11!");
                        break;
                }
                break;

            default:
                Debug.LogError("Invalid age group! Defaulting to age 6 level 1 setup.");
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
