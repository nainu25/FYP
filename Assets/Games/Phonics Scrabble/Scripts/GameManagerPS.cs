using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

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

    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        if (gridManager == null || letterSprites == null || letterSprites.Length == 0)
        {
            Debug.LogError("GameManagerPS has missing references!");
        }
        else
        {
            Debug.Log("All references are assigned properly.");
            LoadWord();
        }
        score = 0;
        errors = 0;
        pausePanel.SetActive(false);
        levelCompPanel.SetActive(false);

    }

    void LoadWord()
    {
        if (words == null || words.Length == 0)
        {
            Debug.LogError("No words available!");
            return;
        }

        currentWord = words[currentWordIndex];
        Debug.Log("Loading word: " + currentWord);

        currentLetterIndex = 0;

        wordGridPath = gridManager.GenerateValidPath(currentWord.Length);

        // Check if the word path is valid
        if (wordGridPath == null || wordGridPath.Count != currentWord.Length)
        {
            Debug.LogError($"Invalid grid path for word: {currentWord}. Expected length {currentWord.Length}, got {wordGridPath.Count}");
            return; // Stop further execution if path is invalid
        }

        foreach (Transform child in letterTrayParent)
            Destroy(child.gameObject);
        spawnedTiles.Clear();

        List<char> letterList = currentWord.ToCharArray().ToList();

        // Add 2 random distractor letters (not in currentWord)
        System.Random rand = new System.Random();
        while (letterList.Count < currentWord.Length + 2)
        {
            char extraChar = (char)rand.Next('a', 'z' + 1);
            if (!letterList.Contains(extraChar))
            {
                letterList.Add(extraChar);
            }
        }

        // Shuffle the list
        letterList = letterList.OrderBy(x => rand.Next()).ToList();

        // Spawn tiles for all letters in the randomized list
        foreach (char c in letterList)
        {
            GameObject tile = Instantiate(tilePrefab, letterTrayParent);
            if (tile == null)
            {
                Debug.LogError("Failed to instantiate tilePrefab.");
            }
            tile.GetComponent<Image>().sprite = GetSpriteForLetter(c);

            Button btn = tile.GetComponent<Button>();
            char captured = c;
            btn.onClick.AddListener(() => OnTileClicked(captured));

            spawnedTiles.Add(tile);
        }
    }


    private Sprite GetSpriteForLetter(char letter)
    {
        return letterSprites.FirstOrDefault(s => s.name.ToLower() == letter.ToString().ToLower());
    }

    void OnTileClicked(char letter)
    {
        if (currentLetterIndex >= currentWord.Length || currentLetterIndex >= wordGridPath.Count)
        {
            Debug.LogWarning("Click ignored: index out of bounds.");
            return;
        }

        char expected = currentWord[currentLetterIndex];
        if (letter == expected)
        {
            int gridIndex = wordGridPath[currentLetterIndex];
            gridManager.SetLetterAtIndex(gridIndex, letter);
            currentLetterIndex++;
            score += 10;
            scoreText.text = score.ToString();

            if (currentLetterIndex >= currentWord.Length)
                Invoke(nameof(NextWord), 1f);
        }
        else
        {
            Debug.Log("Wrong letter selected.");
            errors += 1;
        }
    }

    void NextWord()
    {
        currentWordIndex++;
        if (currentWordIndex < words.Length)
            LoadWord();
        else
        {
            levelCompPanel.SetActive(true);
            endScore.text = score.ToString();

            switch(level)
            {
                case 1:
                    PlayerPrefs.SetInt("PS L1", score);
                    PlayerPrefs.SetInt("PS Err L1", errors);
                    break;
                case 2:
                    PlayerPrefs.SetInt("PS L2", score);
                    PlayerPrefs.SetInt("PS Err L2", errors);
                    break;
                case 3:
                    PlayerPrefs.SetInt("PS L3", score);
                    PlayerPrefs.SetInt("PS Err L3", errors);
                    break;
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Stop all game time-related activities
        pausePanel.SetActive(true); // Show the pause menu
    }

    // Unpause the game
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game time
        pausePanel.SetActive(false); // Hide the pause menu
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Game Selector");
    }

    public void NextLevel()
    {
        if(level == 1)
        {
            SceneManager.LoadScene("PS Level 2");
        }
        else if(level == 2)
        {
            SceneManager.LoadScene("PS Level 3");
        }
        else
        {
            Home();
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
