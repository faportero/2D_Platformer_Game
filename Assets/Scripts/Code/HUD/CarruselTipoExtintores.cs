using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarruselTipoExtintores : MonoBehaviour
{
    [SerializeField] private int distance = 2045;
    [SerializeField] private int val = 1;
    [SerializeField] private Image fire1, fire2;
    [SerializeField] private Color[] _colors;
    RectTransform rectTransform;
    Vector2 _valuePos;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _valuePos = new Vector2(0, 0);
        fire1.color = _colors[0]; 
        fire2.color = _colors[0];
    }
    private void Update()
    {
        if (rectTransform.anchoredPosition.x != _valuePos.x)
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, _valuePos, .25f);
    }
    public void MoverCarruselIzqDer(bool derecha)
    {
        if (val == 1 && !derecha)
            val = 5;
        else if (val == 5 && derecha)
            val = 1;
        else if (derecha)
            val++;
        else
            val--;
        if (val == 1)
            _valuePos = new Vector2(0, 0);
        if (val == 2)
            _valuePos = new Vector2(-distance, 0);
        if (val == 3)
            _valuePos = new Vector2(-distance * 2, 0);
        if (val == 4)
            _valuePos = new Vector2(-distance * 3, 0);
        if (val == 5)
            _valuePos = new Vector2(-distance * 4, 0);
        fire1.color = _colors[val - 1];
        fire2.color = _colors[val - 1];
    }
}
