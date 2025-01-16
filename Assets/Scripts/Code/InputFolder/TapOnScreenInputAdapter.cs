using UnityEngine;
using Character;
using InputFolder;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Character
{
    public class TapOnScreenInputAdapter : InputInterface
    {
        private Vector2 _currentMousePosition, _targetPosition, direction;
        private CharacterMediator _character;
        private Camera _camera;
        private float _angleRot;
        private readonly GasButtonOnUI _gasButton;
        private readonly DialButtonOnUI _dialButtonOnUI;
        GameObject _objectPress;

        public TapOnScreenInputAdapter(CharacterMediator character, GasButtonOnUI gasButton,  DialButtonOnUI dialButtonOnUI)
        {
            _character = character;
            _gasButton = gasButton;
            _camera = Camera.main;
            _dialButtonOnUI = dialButtonOnUI;
            _angleRot = _character.transform.rotation.eulerAngles.z;
        }

        public bool CanGasActionPress()
        {
            //Vector3 rotationRate = Input.gyro.rotationRateUnbiased;
            //if (rotationRate.magnitude > 0.25f)
            //{
            //    return true;
            //}
            return false;
        }

        public Vector2 GetDirection(bool isCollision)
        {
            if (Time.timeScale == 0) return Vector2.zero;
            if (isCollision)
            {
                _targetPosition = _character.transform.position;
                return Vector2.zero;
            }

            if (Input.touchCount > 0)
            {
                // Iterar sobre todos los toques activos
                Touch firstTouch = Input.GetTouch(0);

                // Manejar el inicio del toque
                if (firstTouch.phase == TouchPhase.Began)
                {
                    // Verificar si el toque está sobre un objeto de la interfaz de usuario
                    if (EventSystem.current.IsPointerOverGameObject(firstTouch.fingerId))
                    {
                        // Almacenar el objeto presionado
                        _objectPress = GetObjectUnderTouch(firstTouch.position);
                    }
                }
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase != TouchPhase.Ended)
                    {
                        GameObject currentObject = GetObjectUnderTouch(touch.position);
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && _objectPress == currentObject)
                        {
                            _targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                            var viewportPoint = _camera.WorldToViewportPoint(_targetPosition);
                            viewportPoint.x = Mathf.Clamp(viewportPoint.x, .05f, .95f);
                            viewportPoint.y = Mathf.Clamp(viewportPoint.y, .05f, .95f);
                            _targetPosition = _camera.ViewportToWorldPoint(viewportPoint);
                        }
                        //Debug.Log("ObjectPress: " + _objectPress + ". CurrentObject: " + currentObject);
                    }
                }
            }
            else
                _objectPress = null;

            //if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            //{
            //    _targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            //    var viewportPoint = _camera.WorldToViewportPoint(_targetPosition);
            //    viewportPoint.x = Mathf.Clamp(viewportPoint.x, .05f, .95f);
            //    viewportPoint.y = Mathf.Clamp(viewportPoint.y, .05f, .95f);
            //    _targetPosition = _camera.ViewportToWorldPoint(viewportPoint);
            //}
            if (Vector2.Distance(_currentMousePosition, _targetPosition) > .05f)
            {
                _currentMousePosition = _character.transform.position;
                direction = (_targetPosition - _currentMousePosition).normalized;
                return direction / 2;
            }
            return Vector2.zero;
        }
        public Vector3 GetRotation()
        {
            if (Time.timeScale == 0) return Vector3.zero;
            if (GetDirection(false) != Vector2.zero || (_character._isInFireTrigger && !_dialButtonOnUI.IsDialPressed))
                _angleRot = _character.transform.rotation.eulerAngles.z;
            else
            {
                var _angle = _dialButtonOnUI.ValorAngulo().y;
                if (_angle < 0)
                    _angle += 360;
                if (_angle > 360)
                    _angle -= 360;

                if (_angle < 180 && _angle > 0)
                {
                    _angleRot += (250 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x)) * Time.deltaTime;
                }
                else if (_angle >= 180 && _angle <= 360)
                {
                    _angleRot -= (250 * Mathf.Abs(_dialButtonOnUI.ValorAngulo().x)) * Time.deltaTime;
                }
                _angleRot %= 360;
                var x = Mathf.Cos(_angleRot * Mathf.Deg2Rad);
                var y = Mathf.Sin(_angleRot * Mathf.Deg2Rad);
            }
            return new Vector3(0, 0, _angleRot);
        }
        GameObject GetObjectUnderTouch(Vector2 touchPosition)
        {
            // Realizar un raycast desde la posición del toque
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touchPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // Devolver el primer objeto golpeado por el raycast (si lo hay)
            if (results.Count > 0)
            {
                return results[0].gameObject;
            }
            else
            {
                return null;
            }
        }
        public bool IsGasActionPressed()
        {
            return _dialButtonOnUI.IsDialPressed;
        }
    }
}