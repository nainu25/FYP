using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameManagerPS gameManager; // Reference to GameManagerPS
    public List<Transform> gridCells; // List of grid cell Transform references (16 cells for a 4x4 grid)
    public GameObject gridLetterTilePrefab; // Optional: prefab for tiles in grid (same as tile prefab)

    private List<GameObject> placedLetters = new List<GameObject>(); // Track placed tiles

    // Clear all placed letters in the grid
    public void ClearGrid()
    {
        foreach (GameObject obj in placedLetters)
            Destroy(obj);
        placedLetters.Clear();
    }

    // Place a letter in the grid at the specified index
    public void SetLetterInGrid(int index, string letter)
    {
        if (index >= gridCells.Count) return;

        // Instantiate a new letter tile in the specified grid cell
        GameObject newTile = Instantiate(gridLetterTilePrefab, gridCells[index]);
        newTile.GetComponent<SpriteRenderer>().sprite = GetSpriteForLetter(letter[0]);

        placedLetters.Add(newTile);
    }

    // Function to get the correct sprite for a letter (used in both GridManager and GameManager)
    private Sprite GetSpriteForLetter(char letter)
    {
        return gameManager.letterSprites.FirstOrDefault(s => s.name.ToLower() == letter.ToString().ToLower());
    }

    public List<int> GenerateValidPath(int wordLength)
    {
        List<int> indices = new List<int>();
        System.Random rand = new System.Random();
        bool valid = false;

        // Track occupied cells
        HashSet<int> occupiedIndices = new HashSet<int>();
        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i].childCount > 0)
            {
                occupiedIndices.Add(i);
            }
        }

        int attempts = 0;
        while (!valid && attempts < 100) // Prevent infinite loop
        {
            int startIndex = rand.Next(0, gridCells.Count);
            bool canPlaceHorizontal = (startIndex % 4) + wordLength <= 4;
            bool canPlaceVertical = startIndex + (wordLength - 1) * 4 < 16;

            indices.Clear();

            if (canPlaceHorizontal && canPlaceVertical)
            {
                bool horizontal = rand.Next(0, 2) == 0;
                for (int i = 0; i < wordLength; i++)
                {
                    int index = horizontal ? startIndex + i : startIndex + i * 4;
                    indices.Add(index);
                }
            }
            else if (canPlaceHorizontal)
            {
                for (int i = 0; i < wordLength; i++)
                    indices.Add(startIndex + i);
            }
            else if (canPlaceVertical)
            {
                for (int i = 0; i < wordLength; i++)
                    indices.Add(startIndex + i * 4);
            }

            // Check if any of the selected cells are already occupied
            if (indices.All(i => !occupiedIndices.Contains(i)))
            {
                valid = true;
            }

            attempts++;
        }

        if (!valid)
        {
            Debug.LogWarning("Could not find valid placement path without overlap after 100 attempts.");
            // Fallback: return empty list or allow overlap if necessary
        }

        return indices;
    }


    public void SetLetterAtIndex(int gridIndex, char letter)
    {
        if (gridIndex < 0 || gridIndex >= gridCells.Count)
            return;

        GameObject newTile = Instantiate(gridLetterTilePrefab, gridCells[gridIndex]);
        newTile.GetComponent<SpriteRenderer>().sprite = GetSpriteForLetter(letter);
        placedLetters.Add(newTile);
    }


}
