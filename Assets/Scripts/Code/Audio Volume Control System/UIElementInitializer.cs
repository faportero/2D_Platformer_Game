using UnityEngine;
using UnityEngine.UI;

public class UIElementInitializer : MonoBehaviour
{
    public enum UIElementType { 
       SFX_Slider,
       MUSIC_Slider
    }

    public UIElementType type;

    Slider slider;

    private void Start()
    {
        switch (type)
        {
            case UIElementType.SFX_Slider:
                slider = GetComponent<Slider>();
                slider.value = ControlDatos._audioEfectos / 100;
                //print("SFX_Slider value " + slider.value);
                break;
            case UIElementType.MUSIC_Slider:
                slider = GetComponent<Slider>();
                slider.value = ControlDatos._audioAmbiental / 100;
                //print("MUSIC_Slider value " + slider.value);
                break;
        }

    }

}
