using Character;
using FireType;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _player;
    [Header("Colors")]
    [SerializeField] private Color[] _colors;
    [Header("Face sprite")]
    [SerializeField] private Sprite[] _facesSprite;
    [Header("Fires Prefabs")]
    [SerializeField] private Fire[] _fires;
    [SerializeField] private GameObject[] _gameObjectsToActive, _gameObjectsToActiveFinish;
    public AudioClip _audioFire, _audioLaugh;
    private Fire _currentFireObject;
    private int _currentFireId = 0;
    private SpriteRenderer _spriteFire, _spriteFace;
    private bool _isAttack = false, _isRecharging = false, _isInstantiateProjectile = false;
    private float _shootVelocity = .8f, _distanceToPlayer;
    private Vector3 _dirAux;
    private GameObject _sliderFire5;
    private MovementController _movementController;

    // Start is called before the first frame update
    void Start()
    {
        if(!_player) _player = FindAnyObjectByType<CharacterController>().transform;
        _movementController = _player.GetComponent<MovementController>();
        _movementController.SetLifeSlider2Value();
        _spriteFire = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _spriteFace = transform.GetChild(1).GetComponent<SpriteRenderer>();
        InstantiateCurrentFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttack)
            StartCoroutine(Attack());
        else
        {
            if (!_isRecharging)
            {
                _distanceToPlayer = Vector3.Distance(_player.position, transform.position);
                if (_distanceToPlayer > 1f && _distanceToPlayer < 5) transform.Translate((_player.position - transform.position).normalized * .005f);
                if (Vector3.Distance(_player.position, transform.position) < 5 && !_isInstantiateProjectile) StartCoroutine(InstantiateFires(2f));
            }
            else
            {
                if (!_movementController._isInBossTrigger)
                {
                    if (_dirAux == Vector3.zero) _dirAux = transform.position + new Vector3(UnityEngine.Random.Range(-.15f, .15f), UnityEngine.Random.Range(-.15f, .15f), 0);
                    transform.Translate((_dirAux - transform.position).normalized * .005f);
                }
            }
        }
        if(_currentFireObject.Life < 1 && _currentFireId < 4)
        {
            _currentFireId += 1;
            _currentFireObject.Damage();
            _shootVelocity += .1f;
            InstantiateCurrentFire();
        }
        if(_currentFireId == 4)
        {
            if(_currentFireObject.Life < 1)
            {
                _currentFireObject.Damage();
                foreach (var item in _gameObjectsToActiveFinish)
                    item.SetActive(true);
                Destroy(gameObject, 0);
                return;
            }
            if (_sliderFire5.activeSelf)
            {
                _spriteFire.gameObject.SetActive(true);
                _spriteFace.gameObject.SetActive(true);
            }
            else
            {
                _spriteFire.gameObject.SetActive(false);
                _spriteFace.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Attack()
    {
        _isAttack = true;
        yield return new WaitForSeconds(8f);
        _spriteFace.sprite = _facesSprite[0];
        _isRecharging = true;
        yield return new WaitForSeconds(8f);
        _spriteFace.sprite = _facesSprite[UnityEngine.Random.Range(1, 4)];
        _isAttack = false;
        _isRecharging = false;
        _dirAux = Vector3.zero;
    }
    IEnumerator InstantiateFires(float time)
    {
        var fire = Instantiate(_fires[_currentFireId], transform.position, Quaternion.identity);
        fire.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = false;
        fire.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().enabled = false;
        foreach (var item in fire.GetComponents<Collider2D>())
            if (item.isTrigger) item.enabled = false;
        foreach (var item in fire.transform.GetChild(1).GetComponentsInChildren<Image>())
            item.color = new Color(0, 0, 0, 0);
        fire.transform.GetChild(2).GetChild(0).transform.localScale = Vector3.zero;
        fire.transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        if (_currentFireId < 4)
            fire.GetComponent<Fire1>().ChangeLife(3);
        else
            fire.GetComponent<Fire2>().ChangeLife(3);
        fire.gameObject.AddComponent<Rigidbody2D>();
        var fireRb = fire.GetComponent<Rigidbody2D>();
        fireRb.gravityScale = 0;
        fireRb.velocity = (_player.position - transform.position).normalized * _shootVelocity;
        fire.transform.GetChild(0).localScale = Vector3.zero;
        fire.transform.GetChild(4).localScale = Vector3.zero;
        if (fire.Id != "Fire5")
        {
            fire.transform.GetChild(2).GetChild(2).localScale = Vector3.zero;
        }
        else
        {
            fire.transform.GetChild(2).GetChild(0).localScale = Vector3.zero;
            StartCoroutine(EncenderSprite(fire));
            //print("Encendiendo sprite");
        }
        _isInstantiateProjectile = true;
        yield return new WaitForSeconds(time);
        _isInstantiateProjectile = false;
        //Destroy(fire.gameObject, .5f);
    }
    IEnumerator EncenderSprite(Fire fire)
    {
        yield return new WaitForSeconds(.15f);
        fire.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }
    public void InstantiateCurrentFire()
    {
        _spriteFace.sprite = _facesSprite[UnityEngine.Random.Range(1, 4)];
        _spriteFire.color = _colors[_currentFireId];
        var auxTransform = _spriteFace.transform.position + new Vector3(UnityEngine.Random.Range(-.075f, .075f), UnityEngine.Random.Range(-.075f, .075f), 0);
        _currentFireObject = Instantiate(_fires[_currentFireId], auxTransform, Quaternion.identity, transform);
        _currentFireObject.transform.localScale = Vector3.one * 4;
        //print(_currentFireId);
        if (_currentFireId < 4)
            _currentFireObject.GetComponent<Fire1>().ChangeConstantValueMultiplier(_currentFireObject.GetComponent<Fire1>().ConstantValueMultiplier / 1.75f);
        else
        {
            _currentFireObject.GetComponent<Fire2>().ChangeConstantValueMultiplier((_currentFireObject.GetComponent<Fire2>().ConstantValueMultiplier / 1.75f));
            _sliderFire5 = _currentFireObject.transform.GetChild(1).gameObject;
            foreach (var item in _currentFireObject.GetComponents<CircleCollider2D>())
                if (!item.isTrigger) item.radius = .15f;
                else item.radius = .7f;
            foreach (var item in _gameObjectsToActive)
            {
                item.SetActive(true);
            }
        }
        //_currentFireObject.transform.GetChild(2).gameObject.SetActive(false);
        _currentFireObject.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        _currentFireObject.transform.GetChild(2).GetChild(0).localScale /= 1.5f;
        _currentFireObject.transform.GetChild(2).GetChild(0).transform.position += new Vector3(0, .15f, 0);
        if (_currentFireObject.transform.GetChild(2).childCount > 1)
        {
            foreach (var item in _currentFireObject.transform.GetChild(2).GetComponentsInChildren<SpriteRenderer>())
            {
                item.enabled = false;
            }
        }
        _currentFireObject.transform.GetChild(3).gameObject.SetActive(false);
    }
    public void SetId(int value)
    {
        _currentFireId = value;
    }
}
