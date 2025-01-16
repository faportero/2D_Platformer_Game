using Character.ExtinguisherGas.Extinguisher;
using InputFolder;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas
{
    public class GasSprintController: MonoBehaviour
    {
        [SerializeField] private GasesSprintConfiguration _gasesSprintConfiguration;
        [SerializeField] private GasSprintId _defaultGasSprintId;
        [SerializeField] private Transform _gasSpawnPosition;
        [SerializeField] private float _sprintRateInSeconds;
        [SerializeField] private GameObject _humo;
        private ExtinguisherController _extinguisher;
        private float _remainingSecondsToAbleToSprint;
        private CharacterInterface _character;
        private GasSprintFactory _gasSprintFactory;
        public bool _canSprint;
        private string _activeGasId;
        public bool _isSprinting;
        CharacterMediator _characterMediator;
        private void Awake()
        {
            var instance = Instantiate(_gasesSprintConfiguration);
            _gasSprintFactory = new GasSprintFactory(instance);
            _extinguisher = GetComponent<ExtinguisherController>();
            _characterMediator = GetComponent<CharacterMediator>();
        }
        public void Configure(CharacterInterface character)
        {
            _character = character;
            _activeGasId = _defaultGasSprintId.Value;
        }

        internal void TryGasSprint()
        {
            _remainingSecondsToAbleToSprint -= Time.deltaTime;
            //print("Seconds: " + _remainingSecondsToAbleToSprint);
            _canSprint = false;
            if (_remainingSecondsToAbleToSprint > 0 || _extinguisher.GetValueId() == 0)
                return;
            Sprint();
        }
        //if (_input.CanGasActionPress(_isCollision))
        //    _gasSprintPrefab.gameObject.SetActive(true);
        //else
        //    _gasSprintPrefab.gameObject.SetActive(false);
        private void Sprint()
        {
            //var prefGas = _gasSprintPrefabs.First(gas => gas.Id.Equals(_activeGasId));

            _characterMediator.valorCarga -= (_characterMediator.cargaMultiplier + (_characterMediator._sprintMultiplier * .00125f));
            _characterMediator.valorCarga = Mathf.Clamp(_characterMediator.valorCarga, 0, 1.1f);
            if (_characterMediator._lvl == 1 && _characterMediator.valorCarga < .6f) _characterMediator.valorCarga = .6f;

            _characterMediator._dialButton.PlayAudioExtinguisher();

            StartCoroutine(RestartAudioExtinguisher());
            StartCoroutine(RestartSprintigBool());
            _canSprint = true;
            _activeGasId = "Gas" + (_extinguisher.GetValueId());
            var gasSprint = _gasSprintFactory
                .CreateGasSprintFactory(
                _activeGasId,
                _gasSpawnPosition.position,
                _gasSpawnPosition.rotation
                );
            _remainingSecondsToAbleToSprint = _sprintRateInSeconds;
            var humo = Instantiate(_humo,
                _gasSpawnPosition.position,
                _gasSpawnPosition.rotation);
            Destroy(humo, 1);
        }
        IEnumerator RestartSprintigBool()
        {
            _isSprinting = true;
            yield return new WaitForSeconds(1);
            _isSprinting = false;
        }
        IEnumerator RestartAudioExtinguisher()
        {
            yield return new WaitForSeconds(2);
            _characterMediator._dialButton.StopAudioExtinguisher();
        }
    }
}