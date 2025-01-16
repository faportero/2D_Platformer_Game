using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings audioSettings;

    [Header("Information - Read Only from inspector")]
    public float musicVolume;
    public float sfxVolume;
    public float cinematicVolume;

    List<AudioSource> musicAudioSources;
    List<AudioSource> sfxAudioSources;
    AudioControl audioControl;
    [SerializeField]
    private int musicAudioSourcesCount=0;
    [SerializeField]
    private int sfxAudioSourcesCount = 0;
    [SerializeField] private GameObject _musicButton, _sfxButton;
    [SerializeField] private GameObject _blockMusicButton, _blockSfxButton;
    private int auxMusicVolume;
    private int auxSFXVolume;
    public static float _audioSFXVolumen;
    private void Awake()
    {
        audioControl = GetComponent<AudioControl>();
        audioSettings = this;
        musicAudioSources = new List<AudioSource>();
        sfxAudioSources = new List<AudioSource>();
        LoadSavedSettings();
    }

    public void LoadSavedSettings()
    {
        //musicVolume = PlayerPrefs.GetFloat(musicVolumeDataName,musicDefaultVolume);
        //sfxVolume = PlayerPrefs.GetFloat(sfxVolumeDataName, sfxDefaultVolume);
        //cinematicVolume = PlayerPrefs.GetFloat(cinematicVolumeDataName, cinematicDefaultVolume);
        musicVolume = ControlDatos._audioAmbiental / 100;
        sfxVolume = ControlDatos._audioEfectos / 100;
        //SetVolumeToAudioSources(musicAudioSources, musicVolume);
        //SetVolumeToAudioSources(sfxAudioSources, sfxVolume);
        //SetVolumeToAudioSources(cinematicAudioSources, cinematicVolume);
    }

    public void ChangeMusicVolume()
    {
        float newVolume = audioControl.SliderAudioAmbiental.value;
        ControlDatos._audioAmbiental = (int)newVolume;
        musicVolume = newVolume / 100;
        if(musicVolume > 0)
        {
            _musicButton.SetActive(true);
            _blockMusicButton.SetActive(false);
        }
        else
        {
            _musicButton.SetActive(false);
            _blockMusicButton.SetActive(true);
        }
        //PlayerPrefs.SetFloat(musicVolumeDataName, musicVolume);
        SetVolumeToAudioSources(musicAudioSources, musicVolume);
    }


    public void ChangSFXVolume()
    {
        float newVolume = audioControl.SliderAudioEfectos.value;
        ControlDatos._audioEfectos = (int)newVolume;
        sfxVolume = newVolume / 100;
        if (sfxVolume > 0)
        {
            _sfxButton.SetActive(true);
            _blockSfxButton.SetActive(false);
        }
        else
        {
            _sfxButton.SetActive(false);
            _blockSfxButton.SetActive(true);
        }
        _audioSFXVolumen = sfxVolume;
        //PlayerPrefs.SetFloat(sfxVolumeDataName, sfxVolume);
        SetVolumeToAudioSources(sfxAudioSources, sfxVolume);
    }
    public void MuteMusicVolume(bool mute)
    {
        if (mute)
        {
            auxMusicVolume = ControlDatos._audioAmbiental;
            audioControl.SliderAudioAmbiental.value = ControlDatos._audioAmbiental = 0;
            musicVolume = 0;
        }
        else
        {
            if(auxMusicVolume == 0) auxMusicVolume = ControlDatos._audioAmbiental = 1;
            else ControlDatos._audioAmbiental = auxMusicVolume;
            audioControl.SliderAudioAmbiental.value = ControlDatos._audioAmbiental;
            musicVolume = ControlDatos._audioAmbiental / 100;
        }
        ChangeMusicVolume();
        //PlayerPrefs.SetFloat(musicVolumeDataName, musicVolume);
        //SetVolumeToAudioSources(musicAudioSources, musicVolume);
    }

    public void MuteSFXVolume(bool mute)
    {
        if (mute)
        {
            auxSFXVolume = ControlDatos._audioEfectos;
            audioControl.SliderAudioEfectos.value = ControlDatos._audioEfectos = 0;
            sfxVolume = 0;
        }
        else
        {
            if(auxSFXVolume == 0) auxSFXVolume = ControlDatos._audioEfectos = 1;
            else ControlDatos._audioEfectos = auxSFXVolume;
            audioControl.SliderAudioEfectos.value = ControlDatos._audioEfectos;
            sfxVolume = ControlDatos._audioEfectos / 100;
        }
        ChangSFXVolume();
        //PlayerPrefs.SetFloat(musicVolumeDataName, musicVolume);
        //SetVolumeToAudioSources(sfxAudioSources, sfxVolume);
    }

    public void SetVolumeToAudioSources(List<AudioSource> audioSources, float volume)
    {
        foreach (AudioSource a in audioSources)
        {
            a.volume = volume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    public void AddMeToMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Add(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }

    public void RemoveMeFromMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Remove(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }
    public void AddMeToSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Add(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }

    public void RemoveMeFromSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Remove(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }
}
