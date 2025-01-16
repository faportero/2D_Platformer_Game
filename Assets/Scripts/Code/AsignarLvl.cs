using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsignarLvl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = false;
            ControlDatos._isWinnerLvl3 = false;
            ControlDatos._isWinnerLvl4 = false;
            ControlDatos._isWinnerLvl5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = false;
            ControlDatos._isWinnerLvl4 = false;
            ControlDatos._isWinnerLvl5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
            ControlDatos._isWinnerLvl4 = false;
            ControlDatos._isWinnerLvl5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
            ControlDatos._isWinnerLvl4 = true;
            ControlDatos._isWinnerLvl5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
            ControlDatos._isWinnerLvl4 = true;
            ControlDatos._isWinnerLvl5 = true;
        }
    }
}
