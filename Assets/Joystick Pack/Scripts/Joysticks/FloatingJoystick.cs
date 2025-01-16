using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : Joystick
{
    private Vector3 _startPosition;
    private Color _colorPress, _colorUnPress;
    private Image _imageBackground, _imageHandle;
    private RectTransform _canvasRectTransform, _panelParentRectTransform;
    [HideInInspector] public bool _isDiestro;
    protected override void Start()
    {
        base.Start();
        //background.gameObject.SetActive(false);
        //_startPosition = background.anchoredPosition;
        _canvasRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if(!GetComponentInChildren<Animator>()) _panelParentRectTransform = transform.parent.GetComponent<RectTransform>();
        ChangeJoystickPosition();
        _imageBackground = background.GetComponent<Image>();
        _imageHandle = background.GetChild(0).GetComponent<Image>();
        _colorPress = _imageBackground.color;
        _colorUnPress = new Color(_colorPress.r, _colorPress.g, _colorPress.b, _colorPress.a / 2);
        _imageBackground.color = _colorUnPress;
        _imageHandle.color = _colorUnPress;
        background.anchoredPosition = _startPosition;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        _imageBackground.color = _colorPress;
        _imageHandle.color = _colorPress;
        //background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //background.gameObject.SetActive(false);
        background.anchoredPosition = _startPosition;
        _imageBackground.color = _colorUnPress;
        _imageHandle.color = _colorUnPress;
        base.OnPointerUp(eventData);
    }
    public void ChangeJoystickPosition()
    {
        if (!_canvasRectTransform) return;
        if(_isDiestro != ControlDatos._isDiestro) _isDiestro = ControlDatos._isDiestro;
        if (_isDiestro)
        {
            _startPosition = new Vector3(_canvasRectTransform.sizeDelta.x * 7 / 8, _canvasRectTransform.sizeDelta.y / 6f);
            if (_panelParentRectTransform)
            {
                _panelParentRectTransform.anchorMin = new Vector2(0.5f, 0);
                _panelParentRectTransform.anchorMax = new Vector2(1, 1);
            }
        }
        else
        {
            _startPosition = new Vector3(_canvasRectTransform.sizeDelta.x / 8, _canvasRectTransform.sizeDelta.y / 6f);
            if (_panelParentRectTransform)
            {
                _panelParentRectTransform.anchorMin = new Vector2(0, 0);
                _panelParentRectTransform.anchorMax = new Vector2(0.5f, 1);
            }
        }
        background.anchoredPosition = _startPosition;
    }
    private void OnDisable()
    {
        ChangeJoystickPosition();
    }
}