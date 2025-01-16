using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [Header("Audio")]
    [Range(0f, 100f)] public int audioAmbiental;
    [Range(0f, 100f)] public int audioEfectos;
    public Slider SliderAudioAmbiental, SliderAudioEfectos;
    private AudioSource[] allAudioSources;
    // Start is called before the first frame update
    void Start()
    {
        if (SliderAudioAmbiental) SliderAudioAmbiental.value = ControlDatos._audioAmbiental;
        if(SliderAudioEfectos) SliderAudioEfectos.value = ControlDatos._audioEfectos;
        CargarValores();
    }

    // Update is called once per frame
    void Update()
    {
        //// Comprueba si el tiempo del juego está pausado
        //if (Time.timeScale == 0)
        //{
        //    allAudioSources = FindObjectsOfType<AudioSource>();
        //    // Pausa todos los audios si el juego está pausado
        //    foreach (var audioSource in allAudioSources)
        //    {
        //        if (audioSource.isPlaying)
        //        {
        //            if(audioSource.gameObject.name != "CanvasFeedBacks")
        //                audioSource.Pause();
        //        }
        //    }
        //}
        //else if (Time.timeScale == 1)
        //{
        //    // Reproduce todos los audios si el juego no está pausado
        //    foreach (var audioSource in allAudioSources)
        //    {
        //        if (!audioSource.isPlaying)
        //        {
        //            if(audioSource.gameObject.name != "CanvasFeedBacks")
        //                audioSource.UnPause();
        //        }
        //    }
        //}
    }
    public void GuardarValores()
    {
        if (SliderAudioAmbiental) audioAmbiental = (int)SliderAudioAmbiental.value;
        if (SliderAudioEfectos) audioEfectos = (int)SliderAudioEfectos.value;
        if (SliderAudioAmbiental) ControlDatos._audioAmbiental = audioAmbiental;
        if (SliderAudioEfectos) ControlDatos._audioEfectos = audioEfectos;
    }
    public void CargarValores()
    {
        if (SliderAudioAmbiental) audioAmbiental = ControlDatos._audioAmbiental;
        if (SliderAudioEfectos) audioEfectos = ControlDatos._audioEfectos;
        if (SliderAudioAmbiental) SliderAudioAmbiental.value = audioAmbiental;
        if (SliderAudioEfectos) SliderAudioEfectos.value = audioEfectos;
    }
    public void SetValueAmbiental(TextMeshProUGUI text)
    {
        if (SliderAudioAmbiental) text.text = SliderAudioAmbiental.value.ToString();
    }
    public void SetValueSFX(TextMeshProUGUI text)
    {
        if (SliderAudioEfectos) text.text = SliderAudioEfectos.value.ToString();
    }
}
