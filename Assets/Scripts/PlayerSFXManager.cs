using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFX
{
    public string name;
    public AudioClip[] audioClips;

    [Range(0f, 1f)] public float volume = 1.0f;
    [Range(0f, 1f)] public float pitch = 1.0f;
    public Vector2 randomVolumeRange = new Vector2(1.0f, 1.0f);
    public Vector2 randomPitchRange = new Vector2(1.0f, 1.0f);
    public bool isLooping = false;

    private AudioSource audioSource;

    public void SetSource(AudioSource source)
    {
        audioSource = source;
        int randomClip = Random.Range(0, audioClips.Length - 1);
        audioSource.clip = audioClips[randomClip];
        audioSource.loop = isLooping;
    }

    public void Play()
    {
        if(audioClips.Length > 1)
        {
            int randomClip = Random.Range(0, audioClips.Length - 1);
            audioSource.clip = audioClips[randomClip];
        }
        audioSource.volume = volume * Random.Range(randomVolumeRange.x, randomVolumeRange.y);
        audioSource.pitch = pitch * Random.Range(randomPitchRange.x, randomPitchRange.y);
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}

public class PlayerSFXManager : MonoBehaviour
{
    public static PlayerSFXManager instance;

    [SerializeField] SFX[] soundEffects;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one Player SFX Manager in scene");
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        for(int i = 0; i < soundEffects.Length; i++)
        {
            GameObject go = new GameObject("SFX_" + i + "_" + soundEffects[i].name);
            go.transform.SetParent(transform);
            soundEffects[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    public void PlaySFX(string name)
    {
        for(int i = 0; i < soundEffects.Length; i++)
        {
            if(soundEffects[i].name == name)
            {
                soundEffects[i].Play();
                return;
            } 
        }
        Debug.LogWarning("SFX Manager: Sound name not found in list: " + name);
    }

    public void StopSFX(string name)
    {
        for(int i = 0; i< soundEffects.Length; i++)
        {
            if(soundEffects[i].name == name)
            {
                soundEffects[i].Stop();
                return;
            }
        }
    }

    public bool IsPlaying(string name)
    {
        for(int i = 0; i < soundEffects.Length; i++)
        {
            if(soundEffects[i].name == name && soundEffects[i].IsPlaying())
            {
                return true;
            }
        }
        return false;
    }

}
