using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerControlPosition : MonoBehaviour
{
    [Serializable]
    public struct RectTransformObject
    {
        public RectTransform _object;
        [Header("Left Transform")]
        public Vector2 _leftControlMin;
        public Vector2 _leftControlMax;
        [Header("Right Transform")]
        public Vector2 _rightControlMin;
        public Vector2 _rightControlMax;
    }
    [SerializeField] private TextMeshProUGUI _textoControl;
    [SerializeField] private FloatingJoystick _movementJoystick;
    [SerializeField] private GameObject _panelToPress;

    [Header("\nDial Transform\n")]
    public RectTransformObject _dialTransform;

    [Header("\nCharge Transform\n")]
    public RectTransformObject _chargeTransform;

    [Header("\nSlider Transform\n")]
    public RectTransformObject _sliderTransform;

    [Header("\nAny Objects Transform\n")]
    public RectTransformObject[] _otherObjectTransform;

    [Header("\nAny Joysticks Transform\n")]
    public FloatingJoystick[] _otherJoysticksTransform;
    bool isStart = true;
    private bool isDragging = false;
    // Start is called before the first frame update
    void Start()
    {
        ChangeControl();
        isStart = false;
    }

    // Update is called once per frame
    public void ChangeControl()
    {
        if(!isStart) ControlDatos._isDiestro = !ControlDatos._isDiestro;
        if (ControlDatos._isDiestro)
        {
            if(_textoControl) _textoControl.SetText("Zurdo");
            if (_dialTransform._object)
            {
                _dialTransform._object.anchorMin = _dialTransform._rightControlMin;
                _dialTransform._object.anchorMax = _dialTransform._rightControlMax;
            }
            foreach (var item in _otherObjectTransform)
            {
                item._object.anchorMin = item._rightControlMin;
                item._object.anchorMax = item._rightControlMax;
            }
            if (_chargeTransform._object)
            {
                _chargeTransform._object.anchorMin = _chargeTransform._rightControlMin;
                _chargeTransform._object.anchorMax = _chargeTransform._rightControlMax;
            }
            if (_sliderTransform._object)
            {
                _sliderTransform._object.anchorMin = _sliderTransform._rightControlMin;
                _sliderTransform._object.anchorMax = _sliderTransform._rightControlMax;
            }
        }
        else
        {
            if (_textoControl) _textoControl.SetText("Diestro");
            if (_dialTransform._object)
            {
                _dialTransform._object.anchorMin = _dialTransform._leftControlMin;
                _dialTransform._object.anchorMax = _dialTransform._leftControlMax;
            }
            foreach (var item in _otherObjectTransform)
            {
                item._object.anchorMin = item._leftControlMin;
                item._object.anchorMax = item._leftControlMax;
            }
            if (_chargeTransform._object)
            {
                _chargeTransform._object.anchorMin = _chargeTransform._leftControlMin;
                _chargeTransform._object.anchorMax = _chargeTransform._leftControlMax;
            }
            if (_sliderTransform._object)
            {
                _sliderTransform._object.anchorMin = _sliderTransform._leftControlMin;
                _sliderTransform._object.anchorMax = _sliderTransform._leftControlMax;
            }
        }

        if (_movementJoystick) _movementJoystick.ChangeJoystickPosition();
        foreach (var item in _otherJoysticksTransform)
            if(item) item.ChangeJoystickPosition();
    }
    public void RestartControlsFloating()
    {

        if (ControlDatos._isDiestro)
        {
            if (_textoControl) _textoControl.SetText("Zurdo");
            if (_dialTransform._object)
            {
                _dialTransform._object.anchorMin = _dialTransform._rightControlMin;
                _dialTransform._object.anchorMax = _dialTransform._rightControlMax;
            }
            foreach (var item in _otherObjectTransform)
            {
                item._object.anchorMin = item._rightControlMin;
                item._object.anchorMax = item._rightControlMax;
            }
            if (_chargeTransform._object)
            {
                _chargeTransform._object.anchorMin = _chargeTransform._rightControlMin;
                _chargeTransform._object.anchorMax = _chargeTransform._rightControlMax;
            }
            if (_sliderTransform._object)
            {
                _sliderTransform._object.anchorMin = _sliderTransform._rightControlMin;
                _sliderTransform._object.anchorMax = _sliderTransform._rightControlMax;
            }
        }
        else
        {
            if (_textoControl) _textoControl.SetText("Diestro");
            if (_dialTransform._object)
            {
                _dialTransform._object.anchorMin = _dialTransform._leftControlMin;
                _dialTransform._object.anchorMax = _dialTransform._leftControlMax;
            }
            foreach (var item in _otherObjectTransform)
            {
                item._object.anchorMin = item._leftControlMin;
                item._object.anchorMax = item._leftControlMax;
            }
            if (_chargeTransform._object)
            {
                _chargeTransform._object.anchorMin = _chargeTransform._leftControlMin;
                _chargeTransform._object.anchorMax = _chargeTransform._leftControlMax;
            }
            if (_sliderTransform._object)
            {
                _sliderTransform._object.anchorMin = _sliderTransform._leftControlMin;
                _sliderTransform._object.anchorMax = _sliderTransform._leftControlMax;
            }
        }
    }
}

