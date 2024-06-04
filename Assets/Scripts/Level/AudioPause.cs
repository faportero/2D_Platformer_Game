using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPause : MonoBehaviour
{

    [SerializeField] private GameObject panelPause;
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
           // panelPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            //panelPause.SetActive(false);
        }
    }
}
