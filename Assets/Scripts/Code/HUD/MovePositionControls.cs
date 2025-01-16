using InputFolder;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovePositionControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{
    private RectTransform _panelContent;
    [SerializeField] private RectTransform rectTransformDeslizador;
    [SerializeField] private RectTransform rectTransformDial;
    public RectTransform rectTransformDIndicador;
    private Slider _slider;
    [SerializeField] private FixedJoystick _joystickDial;
    private DialButtonOnUI _dial;
    private Vector2 originalPosition;
    private Camera _cam;
    private Vector2 _canvasval;
    bool isMobil;
    Vector2 delta, firstTouchPosition, localPoint;
    Vector2 _auxVal = new Vector2(-100, 375);
    private bool _corrutinaEspera;

    private void Awake()
    {
        _panelContent = GetComponent<RectTransform>();
        _cam = Camera.main;
        if (Application.isMobilePlatform) isMobil = true;
        else isMobil = false;
        _joystickDial = rectTransformDial.GetChild(0).GetComponent<FixedJoystick>();
        _dial = _joystickDial.GetComponent<DialButtonOnUI>();
        _slider = rectTransformDeslizador.GetComponentInChildren<Slider>();
        _corrutinaEspera = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Panel presionado");

        Vector2 _targetPosition;
        if (isMobil)
        {
            Touch touch = Input.GetTouch(0);
            _targetPosition = touch.position;
        }
        else
            _targetPosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _panelContent,
            eventData.position,
            eventData.pressEventCamera,
            out _targetPosition);
        rectTransformDeslizador.anchoredPosition = _targetPosition + _auxVal;
        rectTransformDial.anchoredPosition = _targetPosition + _auxVal;
        rectTransformDIndicador.anchoredPosition = _targetPosition + _auxVal;

        if (_joystickDial.gameObject.activeSelf)
        {
            if (isMobil)
            {
                _joystickDial.OnPointerDown(CreatePointerEventData(Input.GetTouch(0).position));
                _dial.OnPointerDown(CreatePointerEventData(Input.GetTouch(0).position));
            }
            else
            {
                _joystickDial.OnPointerDown(CreatePointerEventData(Input.mousePosition));
                _dial.OnPointerDown(CreatePointerEventData(Input.mousePosition));
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_joystickDial.gameObject.activeSelf)
        {
            if (isMobil) _dial.OnPointerUp(CreatePointerEventData(Input.GetTouch(0).position));
            else _dial.OnPointerUp(CreatePointerEventData(Input.mousePosition));
            _corrutinaEspera = false;
        }
        ResetPositions();
    }
    public void ResetPositions()
    {
        rectTransformDIndicador.anchoredPosition = Vector2.zero;
        rectTransformDial.anchoredPosition = Vector2.zero;
        rectTransformDeslizador.anchoredPosition = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out localPoint);
        delta = localPoint - firstTouchPosition + new Vector2(_auxVal.x, 0);
        if(!_slider)
            _slider = rectTransformDeslizador.GetComponentInChildren<Slider>();
        float normalizedValue = Mathf.InverseLerp(_slider.GetComponent<RectTransform>().rect.xMin, _slider.GetComponent<RectTransform>().rect.xMax, delta.x);
        if(_slider.gameObject.activeSelf) _slider.value = Mathf.Clamp01(normalizedValue);
        if (!_corrutinaEspera)
        {
            StartCoroutine(CorrutinaEspera());
            return;
        }        
        if (_joystickDial.gameObject.activeSelf)
        {
            if (_dial.IsDialPressed)
            {
                if (isMobil) _joystickDial.OnDrag(CreatePointerEventData(Input.GetTouch(0).position));
                else _joystickDial.OnDrag(CreatePointerEventData(Input.mousePosition));
            }
        }
    }
    IEnumerator CorrutinaEspera()
    {
        _corrutinaEspera = true;
        yield return new WaitWhile(() => _dial.IsDialPressed);
        yield return new WaitWhile(() => !_joystickDial.gameObject.activeSelf);
        ResetPositions();
        OnPointerUp(CreatePointerEventData(Vector2.zero));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out firstTouchPosition);
    }
    private PointerEventData CreatePointerEventData(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        return eventData;
    }
}