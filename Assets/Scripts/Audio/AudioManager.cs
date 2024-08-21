using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, dialogueSounds;
    public AudioSource musicSource1, musicSource2, sfxSource, dialogueSource;
    public AudioMixer audioMixer;
    public string musicVolume1Parameter = "musicVol1";
    public string musicVolume2Parameter = "musicVol2";
    private AudioSource currentMusicSource;
    private AudioSource nextMusicSource;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string name, int musicSourceIndex)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
            return;
        }

        // Set the new music source
        nextMusicSource = musicSourceIndex == 0 ? musicSource1 : musicSource2;
        nextMusicSource.clip = s.clip;
        nextMusicSource.Play();

        // If no transition is currently happening, set this as the current source
        if (!isTransitioning)
        {
            currentMusicSource = nextMusicSource;
        }
    }

    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
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
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            dialogueSource.clip = s.clip;
            dialogueSource.Play();
        }
    }

    public void ToggleMusic(bool mute)
    {
        musicSource1.mute = !mute;
       // musicSource2.mute = !musicSource2.mute;
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
        // Set the volume for both music sources
        audioMixer.SetFloat(musicVolume1Parameter, Mathf.Log10(volume) * 20);
        audioMixer.SetFloat(musicVolume2Parameter, Mathf.Log10(volume) * 20);
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void DialogueVolume(float volume)
    {
        dialogueSource.volume = volume;
    }

    public void TransitionMusic(float duration)
    {
        if (isTransitioning) return;

        isTransitioning = true;

        // Determine which source is active and which one is the new one
        AudioSource activeSource = currentMusicSource;
        AudioSource newSource = nextMusicSource;

        // Set initial volumes
        audioMixer.SetFloat(musicVolume1Parameter, activeSource == musicSource1 ? 0 : -80);
        audioMixer.SetFloat(musicVolume2Parameter, activeSource == musicSource2 ? 0 : -80);

        StartCoroutine(TransitionCoroutine(duration, activeSource, newSource));
    }

    private IEnumerator TransitionCoroutine(float duration, AudioSource activeSource, AudioSource newSource)
    {
        float elapsedTime = 0;

        // Get initial volumes
        audioMixer.GetFloat(musicVolume1Parameter, out float startVolumeActive1);
        audioMixer.GetFloat(musicVolume2Parameter, out float startVolumeActive2);

        float startVolumeActive = Mathf.Exp(startVolumeActive1 / 20);
        float startVolumeNew = Mathf.Exp(startVolumeActive2 / 20);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate between 0 and 1
            float volumeActive = Mathf.Lerp(startVolumeActive, 0, t);
            float volumeNew = Mathf.Lerp(startVolumeNew, 1, t);

            audioMixer.SetFloat(musicVolume1Parameter, Mathf.Log10(volumeActive) * 20);
            audioMixer.SetFloat(musicVolume2Parameter, Mathf.Log10(volumeNew) * 20);

            yield return null;
        }

        // Finalize the transition
        audioMixer.SetFloat(musicVolume1Parameter, Mathf.Log10(0) * 20);
        audioMixer.SetFloat(musicVolume2Parameter, Mathf.Log10(1) * 20);

        // Update the current music source
        currentMusicSource = newSource;
        isTransitioning = false;
    }
}
