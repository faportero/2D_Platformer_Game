using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPause : MonoBehaviour
{
    public AudioMixerSnapshot paused; 
    public AudioMixerSnapshot unpaused; 
    [SerializeField] private GameObject panelPause;
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;

            LowPass();
           // panelPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;

            LowPass();
            //panelPause.SetActive(false);
        }
    }
    private void LowPass()
    {
        if(Time.timeScale == 0)
        {
            paused.TransitionTo(.01f);
        }
        else
        {
            unpaused.TransitionTo(.01f);
        }
    }
}
