using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDesactiveObjects : MonoBehaviour
{
    [Header("Finish Game\n")]
    public bool _isFinishgGame;
    private int _lvl;
    private int _intentos;
    [SerializeField] GameObject _panelEspera;
    [SerializeField] string _nivelString;
    [Header("Pausa Game\n")]
    public bool _pausaGame;
    public bool _onlyFirstGame;
    [Header("TriggerFire tag object\n")]
    public bool _once;
    [Header("Trigger tag object\n")]
    public bool _destroy = true;
    [Header("Objects\n")]
    public GameObject[] _objectsToInstantiate;
    [SerializeField] private GameObject[] _objectsToShow;
    [SerializeField] private GameObject[] _objectsToHide;
    // Start is called before the first frame update
    private void Awake()
    {
        _lvl = FindAnyObjectByType<CharacterInstaller>()._lvl;
        if (_lvl == 1) _intentos = StarsView._intentos1;
        if (_lvl == 2) _intentos = StarsView._intentos2;
        if (_lvl == 3) _intentos = StarsView._intentos3;
        if (_lvl == 4) _intentos = StarsView._intentos4;
        if (_lvl == 5) _intentos = StarsView._intentosBoss;
    }
    public void DoStart()
    {
        //print("DoStart ActiveDesactive Level: " + _lvl + ". Intentos: " + _intentos);
        if (_isFinishgGame)
        {
            _panelEspera.GetComponent<CargarEscena>().CargarJuegoAsincrono(_nivelString);
        }
        if (_pausaGame)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        if (!_onlyFirstGame || (_onlyFirstGame && _intentos == 1))
        {
            foreach (var _object in _objectsToShow)
                if (_object) _object.SetActive(true);
            foreach (var _object in _objectsToHide)
                if(_object) _object.SetActive(false);
        }
        foreach (var _object in _objectsToInstantiate)
        {
            if (_object) Instantiate(_object, transform.position, transform.rotation, transform.parent);
            //print("Object: " + _object.name + " ha sido instanciado.");
        }
        if (_once)
        {
            Destroy(this, .1f);
        }
    }
}
