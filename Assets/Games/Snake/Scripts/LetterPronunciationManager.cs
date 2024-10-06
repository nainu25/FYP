using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPronunciationManager : MonoBehaviour
{
    public AudioClip[] letterAudioClips;
    public List<GameObject> letterTilePrefabs;
    private Food food;

    private int currentPronunciationIndex = 0;
    private AudioSource audioSource;
    private List<GameObject> activeTiles = new List<GameObject>();

    public bool HasMadeSelection { get; private set; } = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        if(food == null)
        {
            food = FindObjectOfType<Food>();
        }
        StartCoroutine(PlayLetterPronunciations());
    }

    private IEnumerator PlayLetterPronunciations()
    {
        while (currentPronunciationIndex < letterAudioClips.Length)
        {
            PlayAudio();
            ClearExistingTiles();
            SpawnLetterTiles();
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => HasMadeSelection);
            
        }

        Debug.Log("All pronunciations completed!");
    }

    private void PlayAudio()
    {
        audioSource.clip = letterAudioClips[currentPronunciationIndex];
        audioSource.Play();
    }

    private void SpawnLetterTiles()
    {
        string correctLetter = letterAudioClips[currentPronunciationIndex].name;
        GameObject correctTilePrefab = GetTilePrefabByLetter(correctLetter);

        List<string> incorrectLetters = new List<string> { "A", "B", "C", "D", "E", "F", "G" };
        incorrectLetters.Remove(correctLetter);
        string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
        GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);

        if (correctTilePrefab != null)
        {
            InstantiateLetterTile(correctTilePrefab, new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for correct letter: {correctLetter}");
        }

        if (incorrectTilePrefab != null)
        {
            InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
        }
        HasMadeSelection = false;
    }

    private GameObject GetTilePrefabByLetter(string letter)
    {
        foreach (GameObject prefab in letterTilePrefabs)
        {
            if (prefab.name == letter)
            {
                return prefab;
            }
        }
        return null;
    }

    private void InstantiateLetterTile(GameObject tilePrefab, Vector2 position)
    {
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
        tile.GetComponent<LetterTile>().Setup(tilePrefab.name, this);
        activeTiles.Add(tile);
    }

    private void ClearExistingTiles()
    {
        foreach (GameObject tile in activeTiles)
        {
            Destroy(tile);
        }
        activeTiles.Clear();
    }

    public string CurrentLetter => letterAudioClips[currentPronunciationIndex].name;

    public void CorrectSelection()
    {
        audioSource.Stop();
        Debug.Log($"Correct selection for: {CurrentLetter}");
        currentPronunciationIndex++;
        HasMadeSelection = true;
       
        if (currentPronunciationIndex >= letterAudioClips.Length)
        {
            Debug.Log("All letters completed!");
        }
    }
}
