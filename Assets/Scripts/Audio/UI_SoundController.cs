using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider, _dialogueSlider;

    public void ToggleMusic(bool mute)
    {
        AudioManager.Instance.ToggleMusic(mute);
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    public void ToggleDialogue()
    {
        AudioManager.Instance.ToggleDialogue();
    }
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.MusicVolume(_sfxSlider.value);
    }
    public void DialogueVolume()
    {
        AudioManager.Instance.MusicVolume(_dialogueSlider.value);
    }
}
