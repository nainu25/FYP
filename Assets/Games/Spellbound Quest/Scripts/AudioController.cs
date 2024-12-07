using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource bgSource;
    public AudioClip[] audioClips;

    private static AudioController instance;

    private void Awake()
    {
        // Singleton pattern to ensure a single instance of AudioController
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Snake Game Lv 1" || SceneManager.GetActiveScene().name == "Snake Game Lv 2" || SceneManager.GetActiveScene().name == "Snake Game Lv 3" || SceneManager.GetActiveScene().name == "Snake Game Lv 4" || SceneManager.GetActiveScene().name == "Snake Game Lv 5")
        {
            bgSource.volume = 0.1f;
        }
    }

    public void PlayAudio(string clipName)
    {
        if (audioSource == null || audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("AudioSource or AudioClips are not set.");
            return;
        }

        // Find the audio clip by name
        AudioClip clip = System.Array.Find(audioClips, c => c.name == clipName);
        if (clip != null)
        {
            Debug.Log("Audio Played: " + clip.name);
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip with name '{clipName}' not found.");
        }
    }
}
