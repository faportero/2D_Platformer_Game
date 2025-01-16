using Character;
using Character.ExtinguisherGas.Extinguisher;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputFolder
{
    public class DialButtonOnUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private GameObject _handleView;
        private float angleJoystick;
        private float angleJoystickAux, angleJoystickAuxiliarControlador;
        private bool isNotMove = false;
        [HideInInspector] public float angleNewHandle;
        RectTransform _rectTransform, _rectTransformHandle;
        Vector3 _initScale;
        Vector3 _auxScale = Vector3.zero;
        private int scaleFactor;
        public AudioSource _audio;
        private ExtinguisherController _extinguisherController;
        public Transform _player;
        public bool IsDialPressed { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDialPressed = true;
            //_audio.Play();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsDialPressed = false;
            _audio.Stop();
        }
        public void PlayAudioExtinguisher()
        {
            _audio.Play();
            _audio.volume = AudioSettings._audioSFXVolumen;
        }
        public void StopAudioExtinguisher()
        {
            _audio.volume = 0;
        }
        void Start()
        {
            if(!_player) _player = GameObject.FindAnyObjectByType<MovementController>().transform;
            _extinguisherController = _player.GetComponent<ExtinguisherController>();
            _rectTransform = GetComponent<RectTransform>();
            _rectTransformHandle = transform.GetChild(0).GetComponent<RectTransform>();
            _initScale = Vector3.one;
            scaleFactor = Screen.width / Screen.height;
            if (!_player) _audio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (ExtinguisherController._valueExtinguishersCarga[_extinguisherController.GetValueId()] == 0) _audio.Stop();
            //print("Valor vertical: " + _joystick.Vertical + " horizontal: " + _joystick.Horizontal + " angleJoystick: " + angleJoystick);
            if (_joystick.Vertical > 0 && _joystick.Horizontal != 0)
            {
                angleJoystick = Mathf.Atan2(_joystick.Vertical, _joystick.Horizontal) * Mathf.Rad2Deg;
                if (angleJoystick < 50)
                    angleJoystick = 50; 
                if (angleJoystick > 130)
                    angleJoystick = 130;
                angleNewHandle = angleJoystick - 90;
                if (angleNewHandle < 0) angleNewHandle = 360 + angleNewHandle;
                if (angleNewHandle > 360) angleNewHandle = angleNewHandle - 360;
            }
            else if (_joystick.Vertical == 0 && _joystick.Horizontal == 0)
                angleNewHandle = 0;
            angleJoystickAux = Mathf.LerpAngle(angleJoystickAux, angleNewHandle, .125f);
            if (Mathf.Abs(angleJoystickAux) < .05f) angleJoystickAux = 0;
            _handleView.transform.rotation = Quaternion.Euler(0, 0, angleJoystickAux);
            float relativeSize = Mathf.Min(Screen.width, Screen.height) * .75f; // Aquí puedes ajustar el factor de escala según sea necesario
            if (angleJoystickAuxiliarControlador == angleJoystick) isNotMove = true;
            else isNotMove = false;
            angleJoystickAuxiliarControlador = angleJoystick;
            _rectTransform.sizeDelta = new Vector2(relativeSize, relativeSize);
            _rectTransformHandle.sizeDelta = _rectTransform.sizeDelta / 2;
        }
        public Vector2 ValorAngulo()
        {
            if (isNotMove == true) return Vector2.zero;
            return new Vector2(_handleView.transform.rotation.z, angleNewHandle);
        }
        public void ResetAngle()
        {
            _joystick.transform.GetChild(0).position = _joystick.transform.position;
            _joystick.ResetValues();
            angleNewHandle = 0;
            angleJoystickAux = 0;
            angleJoystick = 0;
            _handleView.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
}
