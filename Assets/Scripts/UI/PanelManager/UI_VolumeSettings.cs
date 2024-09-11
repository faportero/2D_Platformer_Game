using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Scrollbar musicScrollBar;
    [SerializeField] private Scrollbar sfxScrollBar;
    [SerializeField] private Scrollbar dialogueScrollBar;
    [SerializeField] private Scrollbar videoScrollBar;

    // Rango en dB
    private const float MIN_DB = -80f;  // Volumen mínimo
    private const float MAX_DB = 10f;   // Volumen máximo

    // Valores predeterminados en dB
    private const float DEFAULT_MUSIC_DB = -6f;
    private const float DEFAULT_SFX_DB = -3f;
    private const float DEFAULT_DIALOGUE_DB = 6f;
    private const float DEFAULT_VIDEO_DB = 0f;

    private void Start()
    {
        LoadVolume(); // Cargar y aplicar los volúmenes guardados
    }

    public void SetMusicVolume()
    {
        float volume = musicScrollBar.value;
        float dB = ConvertToDecibels(volume, MIN_DB, MAX_DB);
        myMixer.SetFloat("musicVol", dB);
    }

    public void SetSfxVolume()
    {
        float volume = sfxScrollBar.value;
        float dB = ConvertToDecibels(volume, MIN_DB, MAX_DB);
        myMixer.SetFloat("sfxVol", dB);
    }

    public void SetDialogueVolume()
    {
        float volume = dialogueScrollBar.value;
        float dB = ConvertToDecibels(volume, MIN_DB, MAX_DB);
        myMixer.SetFloat("dialogueVol", dB);
    }

    public void SetVideoVolume()
    {
        float volume = videoScrollBar.value;
        float dB = ConvertToDecibels(volume, MIN_DB, MAX_DB);
        myMixer.SetFloat("videoVol", dB);
    }

    private float ConvertToDecibels(float value, float minDB, float maxDB)
    {
        // Interpolación lineal entre minDB y maxDB
        return Mathf.Lerp(minDB, maxDB, value);
    }

    private float ConvertToScrollbarValue(float dB, float minDB, float maxDB)
    {
        // Convertir dB a un valor de Scrollbar (0 a 1)
        return Mathf.InverseLerp(minDB, maxDB, dB);
    }

    private void LoadVolume()
    {
        // Cargar los valores guardados o establecer los predeterminados
        musicScrollBar.value = PlayerPrefs.HasKey("musicVol") ? PlayerPrefs.GetFloat("musicVol") : ConvertToScrollbarValue(DEFAULT_MUSIC_DB, MIN_DB, MAX_DB);
        sfxScrollBar.value = PlayerPrefs.HasKey("sfxVol") ? PlayerPrefs.GetFloat("sfxVol") : ConvertToScrollbarValue(DEFAULT_SFX_DB, MIN_DB, MAX_DB);
        dialogueScrollBar.value = PlayerPrefs.HasKey("dialogueVol") ? PlayerPrefs.GetFloat("dialogueVol") : ConvertToScrollbarValue(DEFAULT_DIALOGUE_DB, MIN_DB, MAX_DB);
        videoScrollBar.value = PlayerPrefs.HasKey("videoVol") ? PlayerPrefs.GetFloat("videoVol") : ConvertToScrollbarValue(DEFAULT_VIDEO_DB, MIN_DB, MAX_DB);

        // Aplicar los valores
        SetMusicVolume();
        SetSfxVolume();
        SetDialogueVolume();
        SetVideoVolume();
    }

    public void ResetToDefault()
    {
        // Establecer los valores predeterminados de los Scrollbars
        musicScrollBar.value = ConvertToScrollbarValue(DEFAULT_MUSIC_DB, MIN_DB, MAX_DB);
        sfxScrollBar.value = ConvertToScrollbarValue(DEFAULT_SFX_DB, MIN_DB, MAX_DB);
        dialogueScrollBar.value = ConvertToScrollbarValue(DEFAULT_DIALOGUE_DB, MIN_DB, MAX_DB);
        videoScrollBar.value = ConvertToScrollbarValue(DEFAULT_VIDEO_DB, MIN_DB, MAX_DB);

        // Aplicar los valores predeterminados a los mixers
        SetMusicVolume();
        SetSfxVolume();
        SetDialogueVolume();
        SetVideoVolume();

        // Borrar preferencias guardadas
        PlayerPrefs.DeleteKey("musicVol");
        PlayerPrefs.DeleteKey("sfxVol");
        PlayerPrefs.DeleteKey("dialogueVol");
        PlayerPrefs.DeleteKey("videoVol");
    }

    public void SaveSettings()
    {
        // Guardar los valores actuales en PlayerPrefs
        PlayerPrefs.SetFloat("musicVol", musicScrollBar.value);
        PlayerPrefs.SetFloat("sfxVol", sfxScrollBar.value);
        PlayerPrefs.SetFloat("dialogueVol", dialogueScrollBar.value);
        PlayerPrefs.SetFloat("videoVol", videoScrollBar.value);

        // Aplicar los valores a los mixers
        SetMusicVolume();
        SetSfxVolume();
        SetDialogueVolume();
        SetVideoVolume();
    }
}
