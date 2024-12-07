using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource aSource;
    public AudioClip[] audioClips;

    public void PlayJumpAudio()
    {
        aSource.clip = audioClips[0];
        aSource.Play();
    }

    public void PlayCoinAudio()
    {
        aSource.clip = audioClips[1];
        aSource.Play();
    }

    public void PlayKillAudio()
    {
        aSource.clip = audioClips[2];
        aSource.Play();
    }
}
