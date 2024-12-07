using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LetterPronunciationManager : MonoBehaviour
{
    public AudioClip[] letterAudioClips;
    public List<GameObject> letterTilePrefabs;
    private Food food;

    private int currentPronunciationIndex = 0;
    private AudioSource audioSource;
    private List<GameObject> activeTiles = new List<GameObject>();

    public bool HasMadeSelection { get; private set; } = false;

    public GameManager gm;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        if (food == null)
        {
            food = FindObjectOfType<Food>();
        }
        gm.ResetTimer();
        gm.StartTimer();
        ResetPlayerPrefs();
        StartCoroutine(PlayLetterPronunciations());
    }

    private IEnumerator PlayLetterPronunciations()
    {
        while (currentPronunciationIndex < letterAudioClips.Length)
        {
            ClearExistingTiles();
            yield return new WaitForSeconds(1f);
            SpawnLetterTiles();
            yield return new WaitUntil(() => HasMadeSelection);
        }

        Debug.Log("All pronunciations completed!");
        Time.timeScale = 0;
    }

    private void PlayAudio()
    {
        audioSource.clip = letterAudioClips[currentPronunciationIndex];
        audioSource.Play();
    }

    private IEnumerator PlayAudioAfterDelay()
    {
        while (!HasMadeSelection)
        {
            if (!audioSource.isPlaying)
            {
                PlayAudio();
                yield return new WaitForSeconds(audioSource.clip.length + 1f);
            }
            else
            {
                yield return null;
            }
        }
        yield break;
    }



    private void SpawnLetterTiles()
    {
        HasMadeSelection = false;
        StartCoroutine(PlayAudioAfterDelay());

        string correctLetter = letterAudioClips[currentPronunciationIndex].name;
        GameObject correctTilePrefab = GetTilePrefabByLetter(correctLetter);

        // Define incorrect letters for each level
        List<string> incorrectLetters = GetIncorrectLettersForLevel(gm.level);

        // Spawn a random number of incorrect tiles based on the level
        int numberOfIncorrectTiles = gm.level; // L1 -> 1, L2 -> 2, ..., L5 -> 5
        List<string> selectedIncorrectLetters = new List<string>();

        // Randomly select incorrect letters without repeating
        for (int i = 0; i < numberOfIncorrectTiles; i++)
        {
            if (incorrectLetters.Count > 0)
            {
                string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
                incorrectLetters.Remove(incorrectLetter); // Remove the selected letter to prevent repetition
                selectedIncorrectLetters.Add(incorrectLetter);
            }
        }

        // Spawn the selected incorrect tiles
        foreach (var incorrectLetter in selectedIncorrectLetters)
        {
            GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
        }

        // Spawn the correct tile
        if (correctTilePrefab != null)
        {
            InstantiateLetterTile(correctTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for correct letter: {correctLetter}");
        }
    }


    private List<string> GetIncorrectLettersForLevel(int level)
    {
        List<string> incorrectLetters = new List<string>();

        switch (level)
        {
            case 1:
                incorrectLetters.AddRange(new string[] { "A", "B", "C", "D", "E", "F", "G" });
                break;
            case 2:
                incorrectLetters.AddRange(new string[] { "H", "I", "J", "K", "L", "M", "N" });
                break;
            case 3:
                incorrectLetters.AddRange(new string[] { "O", "P", "Q", "R", "S", "T", "U" });
                break;
            case 4:
                incorrectLetters.AddRange(new string[] { "V", "W", "X", "Y", "Z", "A", "B" });
                break;
            case 5:
                incorrectLetters.AddRange(new string[] { "C", "D", "E", "F", "G", "H", "I" });
                break;
            default:
                Debug.LogWarning("Invalid level");
                break;
        }

        incorrectLetters.Remove(letterAudioClips[currentPronunciationIndex].name); // Remove correct letter
        return incorrectLetters;
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
        int maxAttempts = 10;
        bool validPosition = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            if (!Physics2D.OverlapCircle(position, 0.75f, LayerMask.GetMask("Obstacle")) && !Physics2D.OverlapCircle(position, 0.75f, LayerMask.GetMask("UI2")) && !Physics2D.OverlapCircle(position, 0.75f, LayerMask.GetMask("Tile")))
            {
                validPosition = true;
                break;
            }

            position = new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
        }

        if (validPosition)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.GetComponent<LetterTile>().Setup(tilePrefab.name, this);
            activeTiles.Add(tile);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid position for the tile.");
        }
    }


    private void ClearExistingTiles()
    {
        foreach (GameObject tile in activeTiles)
        {
            Destroy(tile);
        }
        activeTiles.Clear();
        if (HasMadeSelection)
        {
            StopCoroutine(PlayAudioAfterDelay());
        }
    }

    public string CurrentLetter => letterAudioClips[currentPronunciationIndex].name;

    public void CorrectSelection()
    {
        Debug.Log($"Correct selection for: {CurrentLetter}");
        currentPronunciationIndex++;
        HasMadeSelection = true;

        // Define the score increment for each level
        int scoreIncrement = GetScoreIncrementForLevel(gm.level);

        if (scoreIncrement > 0)
        {
            gm.score += scoreIncrement;
            gm.UpdateScoreText();
            SaveScoreForLevel(gm.level, gm.score);
        }

        // Check if all letters are completed
        if (currentPronunciationIndex >= letterAudioClips.Length)
        {
            Time.timeScale = 0;
            gm.ToggleEndPanel();
            gm.UpdateEndPanelScore();
        }
    }

    private int GetScoreIncrementForLevel(int level)
    {
        switch (level)
        {
            case 1: return 10;
            case 2: return 15;
            case 3: return 20;
            case 4: return 25;
            case 5: return 30;
            default: return 0;
        }
    }

    private void SaveScoreForLevel(int level, int score)
    {
        string scoreKey = $"PlayerScore Lv{level}";
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();
    }

    public void LoadNextScene()
    {
        if (gm.level == 5)
        {
            LoadScene("Game Selector");
        }
        else if (gm.level >= 0 && gm.level < 5)
        {
            LoadScene($"Snake Game Lv {gm.level + 1}");
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    void ResetPlayerPrefs()
    {
        int level = gm.level;
        if (level >= 1 && level <= 5)
        {
            PlayerPrefs.SetInt($"PlayerScore Lv{level}", 0);
            PlayerPrefs.Save();
        }
    }

}
