using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManagerPS : MonoBehaviour
{
    public GridManager gridManager;
    public Transform letterTrayParent;
    public GameObject tilePrefab;
    public Sprite[] letterSprites;
    public int level;

    public string[] words;
    private int currentWordIndex = 0;
    private string currentWord;
    private int currentLetterIndex = 0;

    public TMP_Text scoreText;
    private int score;

    private int errors;

    public GameObject pausePanel;
    public GameObject levelCompPanel;
    public TMP_Text endScore;

    private List<GameObject> spawnedTiles = new List<GameObject>();
    private List<int> wordGridPath = new List<int>();

    private const int ScreenWidth = 1920;
    private const int ScreenHeight = 1080;
    private const int ScoreIncrement = 10;
    private const int DistractorLettersCount = 2;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip levelCompleteSound;

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreenMode.FullScreenWindow);

        if (!ValidateReferences())
        {
            Debug.LogError("GameManagerPS has missing references!");
            return;
        }

        Debug.Log("All references are assigned properly.");
        score = 0;
        errors = 0;
        pausePanel.SetActive(false);
        levelCompPanel.SetActive(false);
        LoadWord();
    }

    private bool ValidateReferences()
    {
        return gridManager != null && letterSprites != null && letterSprites.Length > 0;
    }

    void LoadWord()
    {
        if (words == null || words.Length == 0)
        {
            Debug.LogError("No words available!");
            return;
        }

        currentWord = words[currentWordIndex];
        Debug.Log($"Loading word: {currentWord}");

        currentLetterIndex = 0;
        wordGridPath = gridManager.GenerateValidPath(currentWord.Length);

        if (!IsValidWordPath())
        {
            Debug.LogError($"Invalid grid path for word: {currentWord}. Expected length {currentWord.Length}, got {wordGridPath?.Count ?? 0}");
            return;
        }

        ClearLetterTray();
        SpawnLetterTiles();
    }

    private bool IsValidWordPath()
    {
        return wordGridPath != null && wordGridPath.Count == currentWord.Length;
    }

    private void ClearLetterTray()
    {
        foreach (Transform child in letterTrayParent)
        {
            Destroy(child.gameObject);
        }
        spawnedTiles.Clear();
    }

    private void SpawnLetterTiles()
    {
        List<char> letterList = GenerateLetterList();
        foreach (char c in letterList)
        {
            GameObject tile = Instantiate(tilePrefab, letterTrayParent);
            if (tile == null)
            {
                Debug.LogError("Failed to instantiate tilePrefab.");
                continue;
            }

            tile.GetComponent<Image>().sprite = GetSpriteForLetter(c);
            AddTileClickListener(tile, c);
            spawnedTiles.Add(tile);
        }
    }

    private List<char> GenerateLetterList()
    {
        List<char> letterList = currentWord.ToCharArray().ToList();
        AddDistractorLetters(letterList);
        return ShuffleList(letterList);
    }

    private void AddDistractorLetters(List<char> letterList)
    {
        System.Random rand = new System.Random();
        while (letterList.Count < currentWord.Length + DistractorLettersCount)
        {
            char extraChar = (char)rand.Next('a', 'z' + 1);
            if (!letterList.Contains(extraChar))
            {
                letterList.Add(extraChar);
            }
        }
    }

    private List<char> ShuffleList(List<char> list)
    {
        System.Random rand = new System.Random();
        return list.OrderBy(_ => rand.Next()).ToList();
    }

    private void AddTileClickListener(GameObject tile, char letter)
    {
        Button btn = tile.GetComponent<Button>();
        btn.onClick.AddListener(() => OnTileClicked(letter));
    }

    private Sprite GetSpriteForLetter(char letter)
    {
        string letterName = letter.ToString().ToLower();
        return letterSprites.FirstOrDefault(s => s.name.ToLower() == letterName);
    }

    void OnTileClicked(char letter)
    {
        if (IsClickOutOfBounds())
        {
            Debug.LogWarning("Click ignored: index out of bounds.");
            return;
        }

        if (letter == currentWord[currentLetterIndex])
        {
            ProcessCorrectLetter(letter);
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            Debug.Log("Wrong letter selected.");
            audioSource.PlayOneShot(wrongSound);
            errors++;
        }
    }

    private bool IsClickOutOfBounds()
    {
        return currentLetterIndex >= currentWord.Length || currentLetterIndex >= wordGridPath.Count;
    }

    private void ProcessCorrectLetter(char letter)
    {
        int gridIndex = wordGridPath[currentLetterIndex];
        gridManager.SetLetterAtIndex(gridIndex, letter);
        currentLetterIndex++;
        UpdateScore();

        if (currentLetterIndex >= currentWord.Length)
        {
            Invoke(nameof(NextWord), 1f);
        }
    }

    private void UpdateScore()
    {
        score += ScoreIncrement;
        scoreText.text = score.ToString();
    }

    void NextWord()
    {
        currentWordIndex++;
        if (currentWordIndex < words.Length)
        {
            LoadWord();
        }
        else
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        levelCompPanel.SetActive(true);
        audioSource.PlayOneShot(levelCompleteSound);
        endScore.text = score.ToString();
        SaveLevelProgress();
    }

    private void SaveLevelProgress()
    {
        string scoreKey = $"PS L{level}";
        string errorKey = $"PS Err L{level}";

        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetInt(errorKey, errors);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Game Selector");
    }

    public void NextLevel()
    {
        string nextScene = level switch
        {
            1 => "PS Level 2",
            2 => "PS Level 3",
            _ => "Main Menu"
        };

        SceneManager.LoadScene(nextScene);
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
