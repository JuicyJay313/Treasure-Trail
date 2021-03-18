using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }

    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
