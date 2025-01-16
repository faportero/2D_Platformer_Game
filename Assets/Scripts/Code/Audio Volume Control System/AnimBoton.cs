using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AnimBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image boton;
    [SerializeField] private Sprite normal, highlighted;
    [SerializeField] private AudioSource audio;
    [SerializeField] private float scaleValue = .15f;
    Vector3 LocalScaleConstante, localScale;
    RectTransform rectTransformBoton;
    bool isOnPointer = false;
    public void Start()
    {
        boton = GetComponent<Image>();
        rectTransformBoton = boton.gameObject.GetComponent<RectTransform>();
        localScale = rectTransformBoton.localScale;
        LocalScaleConstante = localScale + new Vector3(scaleValue,scaleValue,0);
        //localScale = new Vector3(LocalScaleConstante.x, LocalScaleConstante.y, LocalScaleConstante.z);
    }

    private void Update()
    {
        if (isOnPointer)
        {
            rectTransformBoton.localScale = new Vector3(Mathf.Lerp(rectTransformBoton.localScale.x, LocalScaleConstante.x,.05f), 
                Mathf.Lerp(rectTransformBoton.localScale.y, LocalScaleConstante.y, .05f), 
                1);
        }
        else
        {
            rectTransformBoton.localScale = new Vector3(Mathf.Lerp(rectTransformBoton.localScale.x, localScale.x, .1f),
                Mathf.Lerp(rectTransformBoton.localScale.y, localScale.y, .1f),
                1);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnPointer = true;
        if(audio) audio.Play();
        if (highlighted)
        {
            boton.sprite = highlighted;
            //rectTransformBoton.localScale = new Vector3(LocalScaleConstante.x, LocalScaleConstante.y, 1);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnPointer = false;
        boton.sprite = normal;
        //rectTransformBoton.localScale = new Vector3(1, 1, 1);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(RestartScale());
    }

    private IEnumerator RestartScale()
    {
        yield return new WaitForSecondsRealtime(.02f);
        isOnPointer = false;
        boton.sprite = normal;
        rectTransformBoton.localScale = localScale;
    }
}
