using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FireType
{
    public class FireController: MonoBehaviour
    {
        [SerializeField] private FireConfiguration _firesConfiguration;
        [SerializeField] private float _sprintRateInSeconds;
        [SerializeField] private GameObject[] _parentFires;
        public static ActiveDesactiveObjects _activeDesactive;
        public static int _totalFires;
        int _tipo;
        [System.Serializable]
        public struct FireCreation
        {
            public Transform _fireSpawnPosition;
            public FireId _fireId;
            public float _timeToCreate;
            [Header("isWrongFireFirst\n")]
            public bool _wrongFire;
            public int _tipo;
            public int _parentFiresIndex;
            public List<GameObject> _gameObjectsToActive;
        }
        [SerializeField] private FireCreation[] fireCreations;
        [SerializeField] private GameObject _gameCanvas;
        private CharacterInterface _character;
        private FireFactory _fireFactory;
        private void Awake()
        {
            _fireFactory = new FireFactory(Instantiate(_firesConfiguration));
            _totalFires = fireCreations.Length;
            _activeDesactive = GetComponent<ActiveDesactiveObjects>();
        }
        public void Configure(GameObject character)
        {            
            _character = character.GetComponent<CharacterInterface>(); ;
            TryFireCreate();
        }

        public static void ActiveDesactive()
        {
            if (_activeDesactive)
            {
                _activeDesactive.enabled = true;
                _activeDesactive.DoStart();
            }
        }

        internal void TryFireCreate()
        {
            Create();
        }
        private void Create()
        {
            foreach (var fire in fireCreations)
            {
                StartCoroutine(CreateAfterTime(fire));
            }
        }
        IEnumerator CreateAfterTime(FireCreation fire)
        {
            yield return new WaitForSeconds(fire._timeToCreate);
            var fireInstance = _fireFactory
                .CreateFireFactory(
                fire._fireId.Value,
                fire._fireSpawnPosition.position,
                fire._fireSpawnPosition.rotation
                );
            if (fire._wrongFire)
            {
                var wrongFire = fireInstance.gameObject.AddComponent<WrongFire>();
                foreach (var item in fire._gameObjectsToActive)
                    wrongFire._objectsToShow.Add(item);
                if(_gameCanvas) wrongFire._gameCanvas = _gameCanvas;
            }
            if (_parentFires.Length == 1 && _parentFires[0]) fireInstance.transform.parent = _parentFires[0].transform;
            else if((_parentFires.Length > 1)) fireInstance.transform.parent = _parentFires[fire._parentFiresIndex].transform;
            if (fireInstance.GetComponent<Fire1>()) fireInstance.GetComponent<Fire1>()._tipo = fire._tipo;
        }
    }
}