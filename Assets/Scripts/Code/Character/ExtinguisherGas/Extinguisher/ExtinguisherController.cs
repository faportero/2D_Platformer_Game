using InputFolder;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.ExtinguisherGas.Extinguisher
{
    public class ExtinguisherController : MonoBehaviour
    {
        [SerializeField] private ExtinguishersConfiguration _extinguisherConfiguration;
        [SerializeField] private ExtinguisherId _defaultExtinguisherId;
        [SerializeField] private Transform _extinguisherSpawnPosition;
        [SerializeField] private float _sprintRateInSeconds;
        //[SerializeField] private ExtinguisherButtonOnUI _extinguisherButtonOn;
        [SerializeField] private GameObject _dialTransform;
        [SerializeField] private GameObject _indicators;
        TextMeshProUGUI _textoTipoFuegoIndicators;
        Image _imageContenedorTextoTipoFuegoIndicators;
        public bool _activeIndicators;
        public static float[] _valueExtinguishersCarga = { 1, 1, 1, 1, 1, 1 };

        [Serializable]
        public struct ExtinguishersValues
        {
            public float _valueExtinguishersCarga;
            public bool _active;
        }
        public ExtinguishersValues[] extinguishersValues;


        private GameObject _carga;
        private Image _imageCarga;
        private CharacterInterface _character;
        private ExtinguisherFactory _extinguisherFactory;
        private CharacterMediator _characterMediator;
        private MovementController _movementController;
        private DialButtonOnUI _dialButtonOnUI;

        public static int _activeExtinguisherId;
        private int _numOfInstances;
        public Slider _sliderBloqueo;
        private float _sliderBloqueoValue;
        private float _valueActiveCarga;
        [SerializeField] private Color[] _colors;
        [SerializeField] private AudioClip[] _audiosCarga;
        private Image _circleGasButton, _boquillaGasButton;

        private AudioSource _audioSourceCarga;

        public int NoExtinguishers { get; private set; }

        public float Vida => _imageCarga.fillAmount;


        private void Awake()
        {
            _dialButtonOnUI = _dialTransform.GetComponent<DialButtonOnUI>();
            var instance = Instantiate(_extinguisherConfiguration);
            _extinguisherFactory = new ExtinguisherFactory(instance);
            _carga = _indicators.transform.GetChild(0).GetChild(0).gameObject;
            _imageContenedorTextoTipoFuegoIndicators = _indicators.transform.GetChild(1).GetComponent<Image>();
            _textoTipoFuegoIndicators = _indicators.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            _imageCarga = _carga.GetComponent<Image>();
            _characterMediator = GetComponent<CharacterMediator>();
            _movementController = GetComponent<MovementController>();
            _audioSourceCarga = _carga.GetComponent<AudioSource>();
            //print("_activeExtinguisherId Antes: " + _activeExtinguisherId);
            for (int i = 0; i < extinguishersValues.Length; i++)
            {
                extinguishersValues[i]._valueExtinguishersCarga = _valueExtinguishersCarga[i];
            }
            if(FindAnyObjectByType<CharacterInstaller>()._lvl > 1 && _activeExtinguisherId == 0)
            {
                _activeExtinguisherId = _defaultExtinguisherId.Value;
                CharacterMediator.NoExtinshers = NoExtinguishers = _characterMediator._noExtinguishers;
                //print(CharacterMediator.NoExtinshers);
            }
            if (_activeExtinguisherId != 0)
            {
                _dialTransform.GetComponent<DialButtonOnUI>().angleNewHandle = _characterMediator.transform.rotation.eulerAngles.z;
                TryCreate();
                _imageCarga.fillAmount = _valueActiveCarga;
            }
            else
            {
                _sliderBloqueo.gameObject.SetActive(false);
            }
            //print("Lvl: " + FindAnyObjectByType<CharacterInstaller>()._lvl + "_activeExtinguisherId Despues: " + _activeExtinguisherId);
            SetColorCarga();
        }
        public void Configure(CharacterInterface character)
        {
            _character = character;
            if (_activeExtinguisherId == 0)
            {
                _dialTransform.gameObject.SetActive(false);
                _sliderBloqueo.gameObject.SetActive(false);
                _indicators.SetActive(false);
            }
            else if (_activeExtinguisherId > 0)
            {
                _sliderBloqueo.gameObject.SetActive(true);
                _indicators.SetActive(true);
            }
        }
        public void TryCreate()
        {
            if (NoExtinguishers == 0)
                _indicators.SetActive(false);
            else
            {
                _dialTransform.GetComponent<DialButtonOnUI>().angleNewHandle = _characterMediator.transform.rotation.eulerAngles.z;
                _indicators.SetActive(true);
                _sliderBloqueo.gameObject.SetActive(true);
            }
            foreach (var instance in GetComponentsInChildren<Extinguisher>())
            {
                Destroy(instance.gameObject);
            }
            Create();
        }
        public void EnabledIndicators(bool isActive)
        {
            if (isActive)
            {
                if (_activeExtinguisherId > 0)
                {
                    _dialTransform.SetActive(true);
                }
                else
                {
                    _dialTransform.SetActive(false);
                }
            }
            else
            {
                _sliderBloqueo.value = .01f;
                _dialTransform.SetActive(false);
                _sliderBloqueo.gameObject.SetActive(true);
            }
        }

        private void Create()
        {
            if (_activeExtinguisherId > 0)
            {
                if (_circleGasButton) _circleGasButton.color = _colors[_activeExtinguisherId - 1];
                if (_boquillaGasButton) _boquillaGasButton.color = _colors[_activeExtinguisherId - 1];
                _indicators.SetActive(true);
                _imageCarga.fillAmount = extinguishersValues[_activeExtinguisherId]._valueExtinguishersCarga = _valueExtinguishersCarga[_activeExtinguisherId];
                DisableIndicators();
                extinguishersValues[_activeExtinguisherId]._active = true;
                _sliderBloqueo.gameObject.SetActive(true);
                _sliderBloqueo.value = .01f;
                _extinguisherFactory
                    .CreateExtinguisherFactory(
                    gameObject,
                    _activeExtinguisherId,
                    _extinguisherSpawnPosition.position,
                    _extinguisherSpawnPosition.rotation
                    );

                if (_activeExtinguisherId == 1)
                    _textoTipoFuegoIndicators.SetText("EXTINTOR TIPO A");
                if (_activeExtinguisherId == 2)
                    _textoTipoFuegoIndicators.SetText("EXTINTOR TIPO B");
                if (_activeExtinguisherId == 3)
                    _textoTipoFuegoIndicators.SetText("EXTINTOR TIPO C");
                if (_activeExtinguisherId == 4)
                    _textoTipoFuegoIndicators.SetText("EXTINTOR TIPO K");
                if (_activeExtinguisherId == 5)
                    _textoTipoFuegoIndicators.SetText("EXTINTOR TIPO D");
                _imageContenedorTextoTipoFuegoIndicators.color = _colors[_activeExtinguisherId - 1];
            }
            else
            {
                if (_circleGasButton) _circleGasButton.color = Color.white;
                if (_boquillaGasButton) _boquillaGasButton.color = Color.white;
                _indicators.SetActive(false);
                _sliderBloqueo.gameObject.SetActive(false);
                _dialTransform.gameObject.SetActive(false);
            }
        }
        public void ChangeActiveExtinguisher()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (i == _activeExtinguisherId - 1)
                    extinguishersValues[_activeExtinguisherId - 1]._active = true;
                else
                    extinguishersValues[_activeExtinguisherId - 1]._active = false;
            }
        }
        public void ChangeValueSlider()
        {
            _sliderBloqueoValue = _sliderBloqueo.value;
            _dialTransform.GetComponent<DialButtonOnUI>().angleNewHandle = _characterMediator.transform.rotation.eulerAngles.z;
            if (_sliderBloqueoValue < .8f && _sliderBloqueoValue > 0f)
            {
                _dialButtonOnUI.ResetAngle();
                if (_dialTransform) _dialTransform.gameObject.SetActive(false);
                return;
            }
            else if (_sliderBloqueoValue > .8f)
            {
                _dialTransform.GetComponent<DialButtonOnUI>().angleNewHandle = _characterMediator.transform.rotation.eulerAngles.z;
                if (_dialTransform) _dialTransform.gameObject.SetActive(true);
                _sliderBloqueo.gameObject.SetActive(false);
            }
            if (_dialButtonOnUI.gameObject.activeSelf)
            {
                _sliderBloqueo.value = 0;
                return;
            }
        }
        IEnumerator SliderOff()
        {
            _dialButtonOnUI.ResetAngle();
            yield return new WaitWhile(()=>_sliderBloqueoValue != _sliderBloqueo.value);
            if (_dialTransform) _dialTransform.gameObject.SetActive(false);
        }

        public float ChangeValueIndicatorsCarga()
        {
            _valueActiveCarga = _valueExtinguishersCarga[_activeExtinguisherId] = extinguishersValues[_activeExtinguisherId]._valueExtinguishersCarga;
            return _valueActiveCarga;
        }
        public bool GetActiveIndicators()
        {
            return _activeIndicators;
        }
        public void SetActiveIndicators(bool value)
        {
            _activeIndicators = value;
        }
        public void SetColorCarga()
        {
            if (_valueActiveCarga > .5f)
            {
                _imageCarga.color = Color.green;
            }
            else if (_valueActiveCarga <= .5f && _valueActiveCarga > .3f)
            {
                _imageCarga.color = Color.Lerp(_imageCarga.color, Color.yellow, .005f);
            }
        }
        public void ChangeValueCarga(float value)
        {
            _valueActiveCarga = value;
            if (_valueActiveCarga >= 0) _imageCarga.fillAmount = _valueActiveCarga;
            if (_valueActiveCarga > .5f)
            {
                _imageCarga.color = Color.green;
                _audioSourceCarga.Stop();
            }
            else if (_valueActiveCarga <= .5f && _valueActiveCarga > .3f)
            {
                _imageCarga.color = Color.Lerp(_imageCarga.color, Color.yellow, .005f);
                if (_audioSourceCarga.clip != _audiosCarga[0])
                {
                    _audioSourceCarga.Stop();
                    _audioSourceCarga.clip = null;
                }
                if (!_audioSourceCarga.isPlaying && _audioSourceCarga.clip != _audiosCarga[0] && Time.timeScale == 1)
                {
                    _audioSourceCarga.loop = false;
                    _audioSourceCarga.clip = _audiosCarga[0];
                    if(_activeIndicators) _audioSourceCarga.Play();
                }
            }
            else if (_valueActiveCarga <= .3f && _valueActiveCarga > .005f)
            {
                _imageCarga.color = Color.Lerp(_imageCarga.color, Color.red, .005f);
                if(_audioSourceCarga.clip != _audiosCarga[1])
                    _audioSourceCarga.Stop();
                if (!_audioSourceCarga.isPlaying && Time.timeScale == 1)
                {
                    _audioSourceCarga.loop = true;
                    _audioSourceCarga.clip = _audiosCarga[1];
                    if (_activeIndicators) _audioSourceCarga.Play();
                }
            }
            else if(_valueActiveCarga < .005f)
            {
                if (_dialButtonOnUI.IsDialPressed || _dialButtonOnUI.gameObject.activeSelf)
                {
                    _audioSourceCarga.loop = false;
                    if (!_audioSourceCarga.isPlaying && Time.timeScale == 1)
                    {
                        _audioSourceCarga.clip = _audiosCarga[0];
                        if(_activeIndicators)_audioSourceCarga.Play();
                    }
                }
                else
                {
                    _audioSourceCarga.Stop();
                    _audioSourceCarga.clip = null;
                }
            }
        }
        //Se referencia desde el inspector de Unity para detener los indicadores
        public void StopIndicatorIndex(int i)
        {
            extinguishersValues[i]._active = false;
        }
        public void StopIndicators()
        {
            for(int i = 0; i < extinguishersValues.Length; i++)
                extinguishersValues[i]._active = false;
            _activeIndicators = false;
        }
        //Se referencia desde el inspector de Unity para detener los indicadores. Arribaa!!!
        //Se referencia desde el inspector de Unity para restablecer los indicadores
        public void RestartIndicatorIndex(int i)
        {
            _valueExtinguishersCarga[i] = 1;
        }
        public void RestartIndicators()
        {
            for (int i = 0; i < extinguishersValues.Length; i++)
            {
                _valueExtinguishersCarga[i] = extinguishersValues[i]._valueExtinguishersCarga = 1;
            }
        }
        public void DisableIndicators()
        {
            for (int i = 0; i < extinguishersValues.Length; i++)
            {
                extinguishersValues[i]._active = false;
            }
        }
        public int GetValueId()
        {
            return _activeExtinguisherId;
        }
        public void SetValueExtinguisherActive(bool Increment)
        {
            _characterMediator._noExtinguishers = NoExtinguishers = CharacterMediator.NoExtinshers;            
            if (Increment)
            {
                _activeExtinguisherId ++;
                if (_activeExtinguisherId > NoExtinguishers)
                    _activeExtinguisherId = 0;
            }
            else
            {
                _activeExtinguisherId--;
                if (_activeExtinguisherId < 0)
                    _activeExtinguisherId = NoExtinguishers;
            }
            TryCreate();
        }
        public void SetExtinguisher(Vector2 value)
        {
            _characterMediator._noExtinguishers = NoExtinguishers = CharacterMediator.NoExtinshers = (int)value.x;
            _activeExtinguisherId = (int)value.y;
            TryCreate();
        }
        public void SetExtinguisherNonCreate(int value)
        {
            _characterMediator._noExtinguishers = NoExtinguishers = CharacterMediator.NoExtinshers;
            _activeExtinguisherId = value;
            TryCreate();
        }
        public void StopAudioSourceCarga()
        {
            _audioSourceCarga.Stop();
        }
        public void EnableDisableAudioSourceCarga(bool active)
        {
            _audioSourceCarga.enabled = active;
        }
    }
}