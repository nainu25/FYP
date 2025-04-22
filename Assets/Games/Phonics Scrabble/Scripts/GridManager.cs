using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameManagerPS gameManager;
    public List<Transform> gridCells;
    public GameObject gridLetterTilePrefab;

    private List<GameObject> placedLetters = new List<GameObject>();

    public void SetLetterInGrid(int index, string letter)
    {
        if (index >= gridCells.Count) return;

        GameObject newTile = Instantiate(gridLetterTilePrefab, gridCells[index]);
        newTile.GetComponent<SpriteRenderer>().sprite = GetSpriteForLetter(letter[0]);

        placedLetters.Add(newTile);
    }

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

        int gridSize = 5; // Updated for 5x5 grid
        int totalCells = gridSize * gridSize;
        int attempts = 0;

        while (!valid && attempts < 100) // Prevent infinite loop
        {
            int startIndex = rand.Next(0, gridCells.Count);

            // Check if horizontal placement is possible
            bool canPlaceHorizontal = (startIndex % gridSize) + wordLength <= gridSize;
            // Check if vertical placement is possible
            bool canPlaceVertical = startIndex + (wordLength - 1) * gridSize < totalCells;

            indices.Clear();

            if (canPlaceHorizontal)
            {
                // Try horizontal placement
                for (int i = 0; i < wordLength; i++)
                {
                    indices.Add(startIndex + i);
                }
            }

            if (canPlaceVertical && indices.Count == 0) // Only try vertical if horizontal failed
            {
                // Try vertical placement
                for (int i = 0; i < wordLength; i++)
                {
                    indices.Add(startIndex + i * gridSize);
                }
            }

            // Ensure there is enough space and no overlap with occupied cells
            if (indices.All(i => !occupiedIndices.Contains(i)) && indices.Count == wordLength)
            {
                valid = true;
            }

            attempts++;
        }

        if (!valid || indices.Count != wordLength)
        {
            Debug.LogError($"Failed to find a valid path after {attempts} attempts for word length {wordLength}.");
            return new List<int>(); // Return empty list if no valid path found
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
