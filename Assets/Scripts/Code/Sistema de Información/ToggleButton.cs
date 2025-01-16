using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    Toggle toggle; 
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponentInParent<Toggle>();
    }

    public void ChangeToggle()
    {
        if (toggle.isOn) 
            toggle.isOn = false;
        else
            toggle.isOn = true;
    }
}
