using Extinguisher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCameraMap : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private Transform _camMap;
    Transform activeMap, player;
    float auxTransformX, auxTransformY;
    [SerializeField] private Vector2 _bounds;
    [SerializeField] private float _speedSwipe = 1;
    [SerializeField] private float _multiplierZoom = 3;
    Vector2 delta, firstTouchPosition, localPoint;
    Vector3 newPosition, newPositionAux;
    float _firstOrtographicSize;
    Camera _cam;
    [HideInInspector] public bool _isCollision;
    RenderTexture _renderTexture;
    RawImage _rawImage;
    List<StayInsideMap> _objectsStayInsideMap;
    public List<GameObject> _visibleObjectsMap;
    List<Vector2> _minMapObjectsInMap;
    [System.Serializable]
    public struct Maps
    {
        public Transform _map;
        public Vector2 _bounds;
        public float _multiplierZoom;
    }
    [SerializeField] private Maps[] _maps;


    Touch touchZero;
    Touch touchOne;
    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;
    Vector2 _mapsBoundsInit;
    private bool zoonIn;

    private void Awake()
    {
        if (Application.isMobilePlatform) _speedSwipe *=2;
    }
    private void Update()
    {
        HandleZoom();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out firstTouchPosition);
        firstTouchPosition = new Vector2(firstTouchPosition.x - _camMap.localPosition.x, firstTouchPosition.y - _camMap.localPosition.y);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            return;
        }
        if (!Application.isMobilePlatform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out localPoint);
            delta = localPoint - firstTouchPosition;
        }
        else
        {
            localPoint = Input.GetTouch(0).position;
            delta = Input.GetTouch(0).deltaPosition * 20;
        }
        newPosition = new Vector3(-delta.x, -delta.y, 0) * (_speedSwipe * (_cam.orthographicSize / _firstOrtographicSize)) * .0001f;
        if (!_isCollision && newPosition != newPositionAux)
            _camMap.localPosition = new Vector3(newPosition.x + _camMap.localPosition.x, newPosition.y + _camMap.localPosition.y, _camMap.localPosition.z);


        _camMap.localPosition = new Vector3(
            Mathf.Clamp(_camMap.localPosition.x, (activeMap.localPosition.x - auxTransformX) - (_bounds.x / (_cam.orthographicSize / _firstOrtographicSize)), (activeMap.localPosition.x - auxTransformX) + (_bounds.x / (_cam.orthographicSize / _firstOrtographicSize))),
            Mathf.Clamp(_camMap.localPosition.y, (activeMap.localPosition.y - auxTransformY) - (_bounds.y / (_cam.orthographicSize / _firstOrtographicSize)), (activeMap.localPosition.y - auxTransformY) + (_bounds.y / (_cam.orthographicSize / _firstOrtographicSize))),
            _camMap.localPosition.z);
        newPositionAux = newPosition;
    }
    private void OnDisable()
    {
        _camMap.localPosition = Vector3.zero;
        _cam.orthographicSize = _firstOrtographicSize;
        for (int i = 0; i < _objectsStayInsideMap.Count; i++)
        {
            _objectsStayInsideMap[i]._minMapSize = _minMapObjectsInMap[i];
        }
        zoonIn = false;
        for (int i = 0; i < _visibleObjectsMap.Count; i++)
        {
            _visibleObjectsMap[i].transform.localScale = Vector3.one;
            if (_visibleObjectsMap[i].GetComponent<Animator>())
            {
                _visibleObjectsMap[i].GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void OnEnable()
    {
        if (!_cam) _cam = _camMap.GetComponent<Camera>();
        if(_firstOrtographicSize != _cam.orthographicSize) _firstOrtographicSize = _cam.orthographicSize;
        _objectsStayInsideMap = new List<StayInsideMap>();
        _objectsStayInsideMap.Clear();
        _visibleObjectsMap = new List<GameObject>();
        _visibleObjectsMap.Clear();
        _minMapObjectsInMap = new List<Vector2>();
        _minMapObjectsInMap.Clear();
        var objects = FindObjectsOfType<StayInsideMap>();
        foreach (var item in objects)
        {
            _objectsStayInsideMap.Add(item);
            _minMapObjectsInMap.Add(item._minMapSize);
        }

        var mapObjects = FindObjectsOfType<GameObject>();
        foreach (var item in mapObjects)
        {
            if (item.layer == 7 && item.name == "ItemCamMap")
            {
                _visibleObjectsMap.Add(item);
                if (item.GetComponentInParent<ActiveExtinguisher>())
                {
                    item.transform.GetChild(1).gameObject.SetActive(true);
                }
                if(item.GetComponent<Animator>() && item.GetComponent<Animator>().runtimeAnimatorController.name != "Fire")
                    item.GetComponent<Animator>().enabled = false;
            }
        }

        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (var map in _maps)
        {
            if (map._map.GetComponent<Renderer>().isVisible)
            {
                activeMap = map._map;
                _bounds = map._bounds;
                _multiplierZoom = map._multiplierZoom;
            }
        }
        auxTransformX = player.position.x - activeMap.position.x;
        auxTransformY = player.position.y - activeMap.position.y;

        _camMap.localPosition = new Vector3(
            Mathf.Clamp(_camMap.localPosition.x, (activeMap.localPosition.x - auxTransformX) - _bounds.x / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.x - auxTransformX) + _bounds.x / (_cam.orthographicSize / _firstOrtographicSize)),
            Mathf.Clamp(_camMap.localPosition.y, (activeMap.localPosition.y - auxTransformY) - _bounds.y / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.y - auxTransformY) + _bounds.y / (_cam.orthographicSize / _firstOrtographicSize)),
            _camMap.localPosition.z);
        if(!zoonIn) StartCoroutine(ZoomOutInit());
    }
    IEnumerator ZoomOutInit()
    {
        zoonIn = true;
        //ZoomIn

        //_cam.orthographicSize = _firstOrtographicSize * _multiplierZoom;
        //while (Mathf.Abs(_cam.orthographicSize - _firstOrtographicSize) > .005f)
        //{
        //    float newSize = _cam.orthographicSize - (1) * .5f;
        //    _cam.orthographicSize = Mathf.Clamp(newSize, _firstOrtographicSize, _firstOrtographicSize * _multiplierZoom);
        //    for (int i = 0; i < _objectsInMap.Count; i++)
        //    {
        //        _objectsInMap[i]._minMapSize = _minMapObjectsInMap[i] * (_cam.orthographicSize / _firstOrtographicSize);
        //    }
        //    _camMap.localPosition = new Vector3(
        //        Mathf.Clamp(_camMap.localPosition.x, (auxTransformX) - _bounds.x / (_cam.orthographicSize / _firstOrtographicSize), (auxTransformX) + _bounds.x / (_cam.orthographicSize / _firstOrtographicSize)),
        //        Mathf.Clamp(_camMap.localPosition.y, (auxTransformY) - _bounds.y / (_cam.orthographicSize / _firstOrtographicSize), (auxTransformY) + _bounds.y / (_cam.orthographicSize / _firstOrtographicSize)),
        //        _camMap.localPosition.z);
        //    yield return new WaitForSecondsRealtime(.125f / _multiplierZoom);
        //}

        ////ZoomOut

        while (Mathf.Abs(_cam.orthographicSize - _firstOrtographicSize * _multiplierZoom) > .005f)
        {
            float newSize = _cam.orthographicSize - (-1) * .5f;
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, Mathf.Clamp(newSize, _firstOrtographicSize, _firstOrtographicSize * _multiplierZoom), .5f);
            for (int i = 0; i < _objectsStayInsideMap.Count; i++)
            {
                _objectsStayInsideMap[i]._minMapSize = _minMapObjectsInMap[i] * (_cam.orthographicSize / _firstOrtographicSize);
            }
            _camMap.localPosition = new Vector3(
                Mathf.Clamp(_camMap.localPosition.x, (activeMap.localPosition.x - auxTransformX * _firstOrtographicSize) - _bounds.x / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.x - auxTransformX * _firstOrtographicSize) + _bounds.x / (_cam.orthographicSize / _firstOrtographicSize)),
                Mathf.Clamp(_camMap.localPosition.y, (activeMap.localPosition.y - auxTransformY * _firstOrtographicSize) - _bounds.y / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.y - auxTransformY * _firstOrtographicSize) + _bounds.y / (_cam.orthographicSize / _firstOrtographicSize)),
                _camMap.localPosition.z);

            for (int i = 0; i < _visibleObjectsMap.Count; i++)
                _visibleObjectsMap[i].transform.localScale = Vector3.one * Mathf.Clamp(_cam.orthographicSize / _firstOrtographicSize, 1, 2.5f);
            //print("Scale es: " + _cam.orthographicSize / _firstOrtographicSize);
            yield return new WaitForSecondsRealtime(.125f / _multiplierZoom);
        }
    }
    void HandleZoom()
    {
        // Zoom for mobile devices
        if (Input.touchCount > 1)
        {
            zoonIn = false;
            //print("detecta los dos inputs");
            if (!_cam) _camMap.GetComponent<Camera>();
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newSize = _cam.orthographicSize + deltaMagnitudeDiff * .005f;
            //_cam.orthographicSize = Mathf.Clamp(Mathf.Abs(newSize), _firstOrtographicSize, _firstOrtographicSize * _multiplierZoom);
            _cam.orthographicSize = Mathf.Clamp(Mathf.Abs(newSize), _firstOrtographicSize, _firstOrtographicSize * _multiplierZoom);
            //_cam.orthographicSize = newSize;
        }

        // Zoom for PC (scroll wheel)
        else if (Input.mouseScrollDelta.y != 0)
        {
            zoonIn = false;
            float scroll = Input.mouseScrollDelta.y;
            float newSize = _cam.orthographicSize - scroll * .5f;
            _cam.orthographicSize = Mathf.Clamp(newSize, _firstOrtographicSize, _firstOrtographicSize * _multiplierZoom);
        }
        for (int i = 0; i < _objectsStayInsideMap.Count; i++)
        {
            _objectsStayInsideMap[i]._minMapSize = _minMapObjectsInMap[i] * (_cam.orthographicSize / _firstOrtographicSize);
        }

        _camMap.localPosition = new Vector3(
            Mathf.Clamp(_camMap.localPosition.x, (activeMap.localPosition.x - auxTransformX) - _bounds.x / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.x - auxTransformX) + _bounds.x / (_cam.orthographicSize / _firstOrtographicSize)),
            Mathf.Clamp(_camMap.localPosition.y, (activeMap.localPosition.y - auxTransformY) - _bounds.y / (_cam.orthographicSize / _firstOrtographicSize), (activeMap.localPosition.y - auxTransformY) + _bounds.y / (_cam.orthographicSize / _firstOrtographicSize)),
            _camMap.localPosition.z);

        for (int i = 0; i < _visibleObjectsMap.Count; i++)
            _visibleObjectsMap[i].transform.localScale = Vector3.one * Mathf.Clamp(_cam.orthographicSize / _firstOrtographicSize, 1, 2.5f);
    }
}
