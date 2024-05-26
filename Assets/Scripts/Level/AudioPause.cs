using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPause : MonoBehaviour
{

    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0; 
        }
        else Time.timeScale = 1;
    }
}
