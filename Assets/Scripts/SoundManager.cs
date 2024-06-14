using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip addPieceClip;

    public bool soundOn = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSound()
    {
        if (soundOn)
        {
            musicAudioSource.Pause();
            sfxAudioSource.volume = 0f;
        }
        else
        {
            musicAudioSource.Play();
            sfxAudioSource.volume = 1f;
        }

        soundOn = !soundOn;
    }

    public void PlayAddPieceSound()
    {
        if (soundOn)
        {
            sfxAudioSource.PlayOneShot(addPieceClip);
        }
    }

}
