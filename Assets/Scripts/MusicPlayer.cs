using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying) { return; }
        audioSource.Play();
    }

    public void StopMusic()
    {
        Destroy(gameObject);
    }
}
