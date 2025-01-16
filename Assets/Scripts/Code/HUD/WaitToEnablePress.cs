using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitToEnablePress : MonoBehaviour
{
    [SerializeField] private float _timeToWait;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(ActiveRaycastImage(_timeToWait));
    }

    private IEnumerator ActiveRaycastImage(float timeToWait)
    {
        _image.raycastTarget = false;
        yield return new WaitForSecondsRealtime(timeToWait);
        _image.raycastTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
