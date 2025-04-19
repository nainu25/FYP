using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
}
