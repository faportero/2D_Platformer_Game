using InputFolder;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Character
{
    internal class AIInputAdapter : InputInterface
    {
        public readonly CharacterMediator _character;
        private Vector2 _currentPosition, _targetPosition;
        private float _radius = 5f;
        private bool _canMove = true;
        private float _angle;
        public AIInputAdapter(CharacterMediator character)
        {
            _character = character;
            _angle = Random.Range(0, Mathf.PI * 2);
        }

        public bool CanGasActionPress()
        {
            return false;
        }

        public Vector2 GetDirection(bool isCollision)
        {
            //Debug.Log(isCollision + " _CanMove" + _canMove);
            _currentPosition = _character.transform.position;
            if (isCollision)
            {
                //_targetPosition = -_targetPosition;
                GetRandomPosition();
                return Vector2.zero;
            }
            if (Vector2.Distance(_currentPosition, _targetPosition) > .0125f)
            {
                var direction = (_targetPosition - _currentPosition).normalized;
                return direction / 2;
            }
            else
                GetRandomPosition();
            return Vector2.zero;
        }

        public bool IsGasActionPressed()
        {
            return Random.Range(0, 100) < 20;
        }
        public int GetExtinguisherActive(int active)
        {
            return active;
        }
        private Vector2 GetRandomPosition()
        {
            _angle = Random.Range(0, Mathf.PI * 2);
            //Debug.Log("Is Comming IA: " + _angle);
            _targetPosition = new Vector2(
                _currentPosition.x + (_radius * Mathf.Cos(_angle)), 
                _currentPosition.y + (_radius * Mathf.Sin(_angle))
                );
            //Debug.Log("NewPosition: " + _targetPosition.ToString());
            return _targetPosition;
        }

        public Vector3 GetRotation()
        {
            return Vector3.zero;
        }
    }
}