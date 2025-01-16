using Character;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputFolder
{
    public class UnityInputAdapter : InputInterface
    {
        private float _angleRot;
        private Vector3 mousePosition;
        private Vector3 currentMousePosition, previousMousePosition;
        private Vector2 _targetPosition;
        private readonly CharacterMediator _character;
        private readonly GasButtonOnUI _gasButton;
        private readonly DialButtonOnUI _dialButtonOnUI;
        private Camera _camera;
        private Vector2 _currentMousePosition;
        private Vector2 direction;
        bool isFirstRotate = false;
        public UnityInputAdapter(CharacterMediator character, GasButtonOnUI gasButton, DialButtonOnUI dialButtonOnUI)
        {
            _character = character;
            _gasButton = gasButton;
            _camera = Camera.main;
            _dialButtonOnUI = dialButtonOnUI;
            _angleRot = _character.transform.rotation.eulerAngles.z;
        }
        public bool CanGasActionPress()
        {
            currentMousePosition = Input.mousePosition;
            if (currentMousePosition != previousMousePosition)
            {
                previousMousePosition = currentMousePosition;
                return true;
            }
            return false;
        }

        public Vector2 GetDirection(bool isCollision)
        {
            //if (isCollision)
            //    return Vector2.zero;
            //var horizontal = Input.GetAxis("Horizontal");
            //var vertical = Input.GetAxis("Vertical");
            //return new Vector2(horizontal, vertical);

            if (isCollision)
            {
                _targetPosition = _character.transform.position;
                return Vector2.zero;
            }
            //if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var viewportPoint = _camera.WorldToViewportPoint(_targetPosition);
                viewportPoint.x = Mathf.Clamp(viewportPoint.x, .05f, .95f);
                viewportPoint.y = Mathf.Clamp(viewportPoint.y, .05f, .95f);
                _targetPosition = _camera.ViewportToWorldPoint(viewportPoint);
            }
            if (Vector2.Distance(_currentMousePosition, _targetPosition) > .05f)
            {
                _currentMousePosition = _character.transform.position;
                direction = (_targetPosition - _currentMousePosition).normalized;
                return direction / 2;
            }
            return Vector2.zero;
        }

        public int GetExtinguisherActive()
        {
            // Obtener el valor del scroll
            float scrollValue = Input.mouseScrollDelta.y*5;

            //Debug.Log("Valor del scroll: " + scrollValue);
            // Verificar si hay un cambio en el scroll
            if (scrollValue != 0f)
            {
                // Imprimir el valor del scroll
                if (scrollValue > 5)
                    scrollValue = 0;
                //Debug.Log("Valor del scroll: " + scrollValue);
                return (int)scrollValue;
            }
            return 0;
        }

        public bool IsGasActionPressed()
        {
            //if (_extinguisherButtonOnUI.IsPressed || _lockedSliderOnUI.IsLockedExtinguisherPressed)
            //    return false;
            //return Input.GetButton("Fire1");
            return _dialButtonOnUI.IsDialPressed;
            //return _gasButton.IsPressed;
        }

        public Vector3 GetRotation()
        {
            if (GetDirection(false) != Vector2.zero || (_character._isInFireTrigger && !_dialButtonOnUI.IsDialPressed))
                _angleRot = _character.transform.rotation.eulerAngles.z;
            else
            {
                    //if (_gasButton.IsPressed || !_dialButtonOnUI.gameObject.activeSelf)
                    //    return new Vector3(0, 0, _angleRot);
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
            isFirstRotate = false;
            //Debug.Log("Angle: " + (int)_angle + ". AngleRot: " + (int)_angleRot);
            //return new Vector3(x, y, _angleRot);
            return new Vector3(0, 0, (int)_angleRot);
        }
    }
}