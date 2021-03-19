using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerInGame : MonoBehaviour
{
    AudioSource audioSourceLevel;
    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Music") != null)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().StopMusic();
        }
        
    }

    void Start()
    {
        
        audioSourceLevel = GetComponent<AudioSource>();
    }

}
