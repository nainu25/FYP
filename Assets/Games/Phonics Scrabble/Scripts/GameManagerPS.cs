using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManagerPS : MonoBehaviour
{
    public GridManager gridManager; // Reference to GridManager

    public Transform letterTrayParent; // Parent to hold letter tiles
    public GameObject tilePrefab; // Letter tile prefab
    public Sprite[] letterSprites; // Array of letter sprites (a-z)

    public string[] words; // Words to be formed (e.g. {"cat", "dog", "bat"})
    private int currentWordIndex = 0;
    private string currentWord;
    private int currentLetterIndex = 0;

    private List<GameObject> spawnedTiles = new List<GameObject>();

    void Start()
    {
        if (gridManager == null || letterSprites == null || letterSprites.Length == 0)
        {
            Debug.LogError("GameManagerPS has missing references!");
        }
        else
        {
            Debug.Log("All references are assigned properly.");
            LoadWord();
        }
    }



    // Load the current word and spawn corresponding tiles in the tray
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

        // Clear grid and tray
        gridManager.ClearGrid();
        foreach (Transform child in letterTrayParent)
            Destroy(child.gameObject);
        spawnedTiles.Clear();

        // Spawn letter tiles for each character in the word
        foreach (char c in currentWord)
        {
            GameObject tile = Instantiate(tilePrefab, letterTrayParent);
            if (tile == null)
            {
                Debug.LogError("Failed to instantiate tilePrefab.");
            }
            tile.GetComponent<SpriteRenderer>().sprite = GetSpriteForLetter(c);
            Button btn = tile.GetComponent<Button>();
            char captured = c;
            btn.onClick.AddListener(() => OnTileClicked(captured));

            spawnedTiles.Add(tile);
        }

    }



    // Function to get the correct sprite based on the letter
    private Sprite GetSpriteForLetter(char letter)
    {
        return letterSprites.FirstOrDefault(s => s.name.ToLower() == letter.ToString().ToLower());
    }

    // When a tile is clicked, check if it matches the current letter in the word
    void OnTileClicked(char letter)
    {
        if (currentLetterIndex >= currentWord.Length)
            return;

        char expected = currentWord[currentLetterIndex];
        if (letter == expected)
        {
            gridManager.SetLetterInGrid(currentLetterIndex, letter.ToString());
            currentLetterIndex++;

            // If word is complete, move to next word after a short delay
            if (currentLetterIndex >= currentWord.Length)
                Invoke(nameof(NextWord), 1f);
        }
        else
        {
            Debug.Log("Wrong letter selected.");
            // Optional: You can add feedback like shaking the wrong tile or playing a wrong sound
        }
    }

    // Go to the next word or end the game if there are no more words
    void NextWord()
    {
        currentWordIndex++;
        if (currentWordIndex < words.Length)
            LoadWord();
        else
            Debug.Log("Level complete!");
    }
}
