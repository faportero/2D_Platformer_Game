using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, dialogueSounds;
    public AudioSource musicSource, sfxSource, dialogueSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Background");
    }
    public void PlayMusic(string name)
    {
       Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s != null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySfx(string name)
    {
       Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {           
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayDialogue(string name)
    {
        Sound s = Array.Find(dialogueSounds, x => x.name == name);
        if (s != null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            dialogueSource.clip = s.clip;
            dialogueSource.Play();
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void ToggleDialogue()
    {
        dialogueSource.mute = !dialogueSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolueme(float volume)
    {
        sfxSource.volume = volume;
    }
    public void DiualogueVolume(float volume)
    {
        dialogueSource.volume = volume;
    }
}
