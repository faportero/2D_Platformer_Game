using FireType;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerHud : MonoBehaviour
{
    [SerializeField] private Image[] _images;
    [SerializeField] private TextMeshProUGUI _textoMonedas;
    [SerializeField] private TextMeshProUGUI[] _textosMonedas;
    private Image _imageBackground;
    private Image _image;
    public float _initTime, _twoStarsTime, _threeStarsTime;
    [SerializeField] private GameObject[] _objectToActive, _objectToHide;
    [HideInInspector] public float _time;
    private TextMeshProUGUI _textCurrentFires, _textTime;
    private string _textTotalFires;
    private int _totalFires ,_currentFires;
    private AudioSource _audioReloj;
    private Animator _animator;
    private float _animatorSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //_textCurrentFires = transform.parent.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _textCurrentFires = transform.parent.parent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _textTime = transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        _image = transform.GetChild(0).GetComponent<Image>();
        _imageBackground = transform.GetChild(1).GetComponent<Image>();
        _audioReloj = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animatorSpeed = _animator.GetFloat("Multiplier");
        if (_animator.enabled) _animator.enabled = false;
        _initTime *= 60;
        _twoStarsTime *= 60;
        _threeStarsTime *= 60;
        _initTime += 1;
        _time = _initTime;
         _totalFires = FireController._totalFires;
        _currentFires = 0;
        _textTotalFires = "/" + _totalFires;
    }

    // Update is called once per frame
    void Update()
    {
        //if (_textCurrentFires.text != FireController._totalFires.ToString()) _textCurrentFires.SetText(FireController._totalFires.ToString() + _textTotalFires);
        if (_currentFires != FireController._totalFires)
        {
            var currentFire = FireController._totalFires;
            _currentFires = _totalFires - currentFire;
            _textCurrentFires.SetText(_currentFires + _textTotalFires);
            _currentFires = currentFire;
        }
        if (_image.fillAmount > 0)
        {
            _time -= Time.deltaTime;
            _image.fillAmount -= Time.deltaTime / _initTime;
            _imageBackground.fillAmount += Time.deltaTime / _initTime;
            //_textoMonedas.SetText(ControlDatos._coins.ToString());
            for (int i = 0; i < _images.Length; i++)
                _images[i].fillAmount = _image.fillAmount;
            if(_image.fillAmount < .25f && _image.fillAmount > .1f)
            {
                if (!_animator.enabled) _animator.enabled = true;
                _animator.SetFloat("Multiplier", _animatorSpeed);
                if(!_audioReloj.isPlaying) _audioReloj.Play();
            }
            if(_image.fillAmount <= .1f && _image.fillAmount > 0f)
            {
                if (!_animator.enabled) _animator.enabled = true;
                _animator.SetFloat("Multiplier", _animatorSpeed * 2);
            }
            //for (int i = 0; i < _textosMonedas.Length; i++)
            //    _textosMonedas[i].SetText(ControlDatos._coins.ToString());
            _textTime.SetText(FormatTime(_time).ToString());
        }
        else
        {
            Time.timeScale = 0;
            for (int i = 0; i < _objectToActive.Length; i++)
                _objectToActive[i].SetActive(true);
            for (int i = 0; i < _objectToHide.Length; i++)
                _objectToHide[i].SetActive(true);
            enabled = false;
            transform.parent.gameObject.SetActive(false);
        }
    }
    // Método para convertir un valor float (en segundos) a un formato de tiempo "MM:SS"
    public string FormatTime(float timeInSeconds)
    {
        // Obtener los minutos y segundos
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Formatear el tiempo como "MM:SS"
        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        return timeFormatted;
    }
    public float AddTime(float time)
    {
        float _auxTime = time;
        if (_initTime - _time < 30) _auxTime = _initTime - _time;
        _image.fillAmount += _auxTime / _initTime;
        _imageBackground.fillAmount -= _auxTime / _initTime;
        _time += _auxTime;
        return _auxTime;
    }
    public void SetNoMonedas()
    {
        _textoMonedas.SetText(ControlDatos._coins.ToString());
        foreach (var item in _textosMonedas)
        {
            item.SetText(ControlDatos._coins.ToString());
        }
    }
}
