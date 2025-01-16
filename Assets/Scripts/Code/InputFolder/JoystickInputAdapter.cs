using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputFolder
{
    public class JoystickInputAdapter : InputInterface
    {
        private readonly Joystick _joystick;
        private readonly GasButtonOnUI _gasButton;
        private readonly DialButtonOnUI _dialButtonOnUI;
        private float _angleRot;
        public static float _angleRotAux;
        private GameObject _objectPress;
        private Vector2 _currentMousePosition, _targetPosition, direction;
        private CharacterMediator _character;
        private Camera _camera;
        private float _auxvalAngleDial;
        public JoystickInputAdapter(Joystick joystick, CharacterMediator character, GasButtonOnUI gasButton, DialButtonOnUI dialButtonOnUI)
        {
            _joystick = joystick;
            _character = character;
            _gasButton = gasButton;
            _camera = Camera.main;
            _dialButtonOnUI = dialButtonOnUI;
            _angleRot = _character.transform.rotation.eulerAngles.z;
        }

        public bool CanGasActionPress()
        {
            //Vector3 rotationRate = Input.gyro.rotationRateUnbiased;
            if (Mathf.Abs(_dialButtonOnUI.ValorAngulo().y) != _auxvalAngleDial)
            {
                _auxvalAngleDial = Mathf.Abs(_dialButtonOnUI.ValorAngulo().y);
                //_dialButtonOnUI._audio.Play();
                return true;
            }
            //_dialButtonOnUI._audio.Pause();
            return false;
        }
        IEnumerator StopAudio()
        {
            yield return new WaitForSeconds(.2f);
            //_dialButtonOnUI._audio.Pause();
        }

        public Vector2 GetDirection(bool isCollision)
        {
            if (Time.timeScale == 0) _joystick.ResetValues();
            if (isCollision)
            {
                _targetPosition = _character.transform.position;
                return Vector2.zero;
            }
            return new Vector2(_joystick.Horizontal, _joystick.Vertical) / 2;
        }
        public int GetExtinguisherActive(int active)
        {
            return active;
        }
        public bool IsGasActionPressed()
        {
            return _dialButtonOnUI.IsDialPressed;
        }
        public Vector3 GetRotation()
        {
            if (GetDirection(false) != Vector2.zero || (_character._isInFireTrigger && !_dialButtonOnUI.IsDialPressed))
            {
                _angleRot = _character.transform.rotation.eulerAngles.z;
                _angleRotAux = _angleRot;
            }
            else
            {
                var _angle = _dialButtonOnUI.ValorAngulo().y;
                if (_angle < 0)
                    _angle += 360;
                if (_angle > 360)
                    _angle -= 360;

                if (_angle < 180 && _angle > 0)
                {
                    //_angleRot += (250 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x)) * Time.deltaTime;
                    //_angleRot += (10 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x));
                    _angleRot = _angleRotAux + 5;
                }
                else if (_angle >= 180 && _angle <= 360)
                {
                    //_angleRot -= (250 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x)) * Time.deltaTime;
                    //_angleRot -= (10 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x));
                    _angleRot = _angleRotAux - 5;
                }
                _angleRot %= 360;
                var x = Mathf.Cos(_angleRot * Mathf.Deg2Rad);
                var y = Mathf.Sin(_angleRot * Mathf.Deg2Rad);
            }
            return new Vector3(0, 0, _angleRot);
        }
    }
}