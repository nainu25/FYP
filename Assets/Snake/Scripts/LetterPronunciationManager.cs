using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPronunciationManager : MonoBehaviour
{
    public AudioClip[] letterAudioClips;
    public List<GameObject> letterTilePrefabs;
    public SnakeController snakeController;

    private int currentPronunciationIndex = 0;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        StartCoroutine(PlayLetterPronunciations());
    }

    private IEnumerator PlayLetterPronunciations()
    {
        while (currentPronunciationIndex < letterAudioClips.Length)
        {
            PlayAudio();
            SpawnLetterTiles();
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => snakeController.HasMadeSelection);
        }
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

        if (correctTilePrefab != null)
        {
            InstantiateLetterTile(correctTilePrefab, new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for correct letter: {correctLetter}");
        }

        List<string> incorrectLetters = new List<string> { "A", "B", "C", "D", "E", "F", "G" };
        string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
        GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);

        if (incorrectTilePrefab != null)
        {
            InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
        }
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
    }

    public string CurrentLetter => letterAudioClips[currentPronunciationIndex].name;

    public void CorrectSelection()
    {
        audioSource.Stop();
        currentPronunciationIndex++;
        Debug.Log($"Correct selection for: {CurrentLetter}");
    }
}
