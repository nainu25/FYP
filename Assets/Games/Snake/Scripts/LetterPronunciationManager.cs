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
        if(food == null)
        {
            food = FindObjectOfType<Food>();
        }
        gm.ResetTimer();
        gm.StartTimer();
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

        if(gm.level==1)
        {
            List<string> incorrectLetters = new List<string> { "A", "B", "C", "D", "E", "F", "G" };
            incorrectLetters.Remove(correctLetter);
            string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
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
        else if(gm.level==2) 
        {
            List<string> incorrectLetters = new List<string> { "H", "I", "J", "K", "L", "M", "N" };
            incorrectLetters.Remove(correctLetter);
            string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
        }
        else if(gm.level==3)
        {
            List<string> incorrectLetters = new List<string> { "O", "P", "Q", "R", "S", "T", "U" };
            incorrectLetters.Remove(correctLetter);
            string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
        }
        else if (gm.level == 4)
        {
            List<string> incorrectLetters = new List<string> { "V", "W", "X", "Y", "Z", "A", "B" };
            incorrectLetters.Remove(correctLetter);
            string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }

        }
        else if (gm.level == 5)
        {
            List<string> incorrectLetters = new List<string> { "C", "D", "E", "F", "G", "H", "I" };
            incorrectLetters.Remove(correctLetter);
            string incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            GameObject incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }
            incorrectLetter = incorrectLetters[Random.Range(0, incorrectLetters.Count)];
            incorrectTilePrefab = GetTilePrefabByLetter(incorrectLetter);
            if (incorrectTilePrefab != null)
            {
                InstantiateLetterTile(incorrectTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
            }
            else
            {
                Debug.LogWarning($"No prefab found for incorrect letter: {incorrectLetter}");
            }

        }



        if (correctTilePrefab != null)
        {
            InstantiateLetterTile(correctTilePrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)));
        }
        else
        {
            Debug.LogWarning($"No prefab found for correct letter: {correctLetter}");
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
        if(gm.level==1)
        {
            gm.score += 10;
            gm.UpdateScoreText();
            PlayerPrefs.SetInt("PlayerScore", gm.score);
            PlayerPrefs.Save();
        }
        else if(gm.level==2) 
        {
            gm.score += 15;
            gm.UpdateScoreText();
            PlayerPrefs.SetInt("PlayerScore", gm.score);
            PlayerPrefs.Save(); 
        }
        else if(gm.level==3)
        {
            gm.score += 20;
            gm.UpdateScoreText();
            PlayerPrefs.SetInt("PlayerScore", gm.score);
            PlayerPrefs.Save();
        }
        else if(gm.level==4)
        {
            gm.score += 25;
            gm.UpdateScoreText();
            PlayerPrefs.SetInt("PlayerScore", gm.score);
            PlayerPrefs.Save();
        }
        else if(gm.level==5)
        {
            gm.score += 30;
            gm.UpdateScoreText();
            PlayerPrefs.SetInt("PlayerScore", gm.score);
            PlayerPrefs.Save();
        }
       
        if (currentPronunciationIndex >= letterAudioClips.Length)
        {
            Debug.Log("All letters completed!");
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load. Game complete!");
            }
        }
    }
}
