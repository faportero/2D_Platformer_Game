using Character;
using Character.ExtinguisherGas.Extinguisher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trigger
{
    public class TriggerExtinguisher : MonoBehaviour
    {
        [SerializeField] private CharacterMediator _characterMediator;
        [SerializeField] private int _valueExtinguisher;
        [SerializeField] private GameObject[] _gameObjectsToActive;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                _characterMediator.SetNoExtinguishers(_valueExtinguisher);
                for (int i = 0; i < _gameObjectsToActive.Length; i++)
                    _gameObjectsToActive[i].SetActive(true);
            }
        }
    }
}