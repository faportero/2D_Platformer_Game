using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeObject : MonoBehaviour
{
    public Vector2 _resizeTam;
    private RectTransform _rectTransform;
    bool landscape = false;
    float relativeSize;
    public RectTransform canvasTransform;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        canvasTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();        
    }
    void Update()
    {
        OrientationChangedHandler(Screen.orientation);
        _rectTransform.sizeDelta = new Vector2(relativeSize * _resizeTam.x, relativeSize * _resizeTam.y);
    }
    void OrientationChangedHandler(ScreenOrientation newOrientation)
    {
        if (canvasTransform.sizeDelta.x > canvasTransform.sizeDelta.y)
        {
            landscape = true;
            relativeSize = Mathf.Min(canvasTransform.sizeDelta.x, canvasTransform.sizeDelta.y);
        }
        else
        {
            landscape = false;
            relativeSize = Mathf.Min(canvasTransform.sizeDelta.y, canvasTransform.sizeDelta.x);
        }
        // También puedes manejar otros modos de orientación si es necesario
    }
}
