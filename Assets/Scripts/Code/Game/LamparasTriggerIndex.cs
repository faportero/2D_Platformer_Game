using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamparasTriggerIndex : MonoBehaviour
{
    [SerializeField] private LamparasActividad _lampara;
    [SerializeField] private int _index;
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.tag == "Player")
        {
            _audio.Play();
            if (_lampara._currentIndex == _index) _lampara._currentIndex = 0;
            else _lampara._currentIndex = _index;
        }
    }
}
