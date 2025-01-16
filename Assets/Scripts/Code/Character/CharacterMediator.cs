using Character.ExtinguisherGas.Extinguisher;
using Character.ExtinguisherGas.Gas;
using FireType;
using InputFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(GasSprintController))]
    //Clase Character, que ocupa el patrónm Mediator
    public class CharacterMediator : MonoBehaviour, CharacterInterface
    {
        [SerializeField] private MovementController _movementController;
        [SerializeField] private GasSprintController _gasSprintController;
        [SerializeField] private ExtinguisherController _extinguisherController;
        public FloatingJoystick _joystickMovement;
        public DialButtonOnUI _dialButton;
        [SerializeField] private float valueMultiplierToRestIndicators = 2;
        [SerializeField] private GameObject _feedBackPanel;
        public float _sprintMultiplier;
        [HideInInspector] public bool _isInFireTrigger;
        private Image _imageJoystickMovementBackground, _imageJoystickMovementHandle;
        [HideInInspector] public float cargaMultiplier;
        public InputInterface _input;
        [HideInInspector] public float valorCarga = 1;
        public int _noExtinguishers;
        public static int NoExtinshers;
        private bool _esperaCorrutinaOcultarDial;
        private bool isIA;
        [HideInInspector] public int _lvl;
        private GameObject _movementJoystick;
        [SerializeField] private MovePositionControls _fondoPress;
        public void Configure(InputInterface input)
        {
            _input = input;
            _gasSprintController.Configure(this);
            _extinguisherController.Configure(this);
            valorCarga = ExtinguisherController._valueExtinguishersCarga[_extinguisherController.GetValueId()] = Mathf.Clamp(ExtinguisherController._valueExtinguishersCarga[_extinguisherController.GetValueId()], 0, 1.1f);
            _extinguisherController.ChangeValueCarga(valorCarga);
            cargaMultiplier = valueMultiplierToRestIndicators / 250;
            isIA = FindFirstObjectByType<CharacterInstaller>().IsIAInput();
            if(!_joystickMovement) _joystickMovement = FindAnyObjectByType<FloatingJoystick>();
            _imageJoystickMovementBackground = _joystickMovement.transform.GetChild(0).GetComponent<Image>();
            _imageJoystickMovementHandle = _joystickMovement.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _movementJoystick = _joystickMovement.transform.GetChild(0).gameObject;
            _lvl = FindAnyObjectByType<CharacterInstaller>()._lvl;
            if(!_fondoPress) _fondoPress = FindAnyObjectByType<MovePositionControls>();
        }
        private void Update()
        {
            if (Time.timeScale == 0)
            {
                valorCarga = _extinguisherController.extinguishersValues[_extinguisherController.GetValueId()]._valueExtinguishersCarga = ExtinguisherController._valueExtinguishersCarga[_extinguisherController.GetValueId()];
                _joystickMovement.ChangeJoystickPosition();
                _extinguisherController.EnableDisableAudioSourceCarga(false);
                return;
            }
            else
            {
                _extinguisherController.EnableDisableAudioSourceCarga(true);
            }
            var direction = _input.GetDirection(_movementController.IsCollisioningWithObject());
            if (isIA)
            {
                _movementController.Move(direction, Vector2.zero);
                //print("Use IA");
                return;
            }
            if (_feedBackPanel)
            {
                if (!_feedBackPanel.activeSelf && Time.timeScale == 0) Time.timeScale = 1;
                if (_extinguisherController.GetValueId() != 0)
                {
                    _movementController.Move(direction, _input.GetRotation());
                }
                else
                {
                    _movementController.Move(direction, Vector2.zero);
                }
                if (_extinguisherController.Vida > 0f && valorCarga > 0f) TryGasSprint();

                if (_input.IsGasActionPressed())
                {
                    if (!_movementController._isInBossTrigger)
                    {
                        if (valorCarga > 0f && _gasSprintController._canSprint)
                        {
                            //valorCarga -= (cargaMultiplier + (_sprintMultiplier * .00125f));
                            //valorCarga = Mathf.Clamp(valorCarga, 0, 1.1f);
                            //if (_lvl == 1 && valorCarga < .6f) valorCarga = .6f;
                        }
                        if (_esperaCorrutinaOcultarDial)
                        {
                            StopCoroutine("EsperaParaOcultarDial");
                            _esperaCorrutinaOcultarDial = false;
                            _dialButton.ResetAngle();
                        }
                    }
                }
                else
                {
                    if (_dialButton.gameObject.activeSelf && !_esperaCorrutinaOcultarDial) StartCoroutine(EsperaParaOcultarDial(.1f));
                }
                if(valorCarga != _extinguisherController.extinguishersValues[ _extinguisherController.GetValueId()]._valueExtinguishersCarga)
                {
                        _extinguisherController.extinguishersValues[_extinguisherController.GetValueId()]._valueExtinguishersCarga =
                        ExtinguisherController._valueExtinguishersCarga[_extinguisherController.GetValueId()] = valorCarga;
                    _extinguisherController.ChangeValueCarga(_extinguisherController.extinguishersValues[_extinguisherController.GetValueId()]._valueExtinguishersCarga);
                }
            }
        }
        IEnumerator EsperaParaOcultarDial(float time)
        {
            _esperaCorrutinaOcultarDial = true;
            yield return new WaitForSeconds(.05f);
            yield return new WaitWhile(()=> !_dialButton.IsDialPressed);
            yield return new WaitWhile(()=> _dialButton.IsDialPressed);
            //print("Has dejado de presionar el dial");
            yield return new WaitForSeconds(time);
            _extinguisherController.TryCreate();
            //_fondoPress.ResetPositions();
            _extinguisherController.EnabledIndicators(false);
            _esperaCorrutinaOcultarDial = false;
            //print("indicadores cambiados");
        }
        private void ActiveSpriteJoystickMovement(bool active)
        {
            if (!_joystickMovement) _joystickMovement = FindAnyObjectByType<FloatingJoystick>();
            if (!_imageJoystickMovementBackground)
                _imageJoystickMovementBackground = _joystickMovement.transform.GetChild(0).GetComponent<Image>();
            if(!_imageJoystickMovementHandle)
                _imageJoystickMovementHandle = _joystickMovement.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _imageJoystickMovementBackground.enabled = active;
            _imageJoystickMovementHandle.enabled = active;
        }
        private void TryGasSprint()
        {
            if (!_dialButton.gameObject.activeSelf)
            {
                _dialButton.IsDialPressed = false;
                _dialButton.ResetAngle();
            }
            else if (_input.IsGasActionPressed() && _input.CanGasActionPress())
            {
                _gasSprintController.TryGasSprint();
            }
        }
        public void SetNoExtinguishers(int value)
        {
            NoExtinshers = value;
            _extinguisherController.TryCreate();
        }
        public void GamePause(bool pause)
        {
            if (pause)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }
    }
}