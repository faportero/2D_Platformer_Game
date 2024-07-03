using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelTutorial;
    private AudioPause audioPause;

    private void Start()
    {
        audioPause = FindAnyObjectByType<AudioPause>();    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioPause.Pause(true);
            panelTutorial.SetActive(true);
        }
    }
}
