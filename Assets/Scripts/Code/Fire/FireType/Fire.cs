using Character;
using Character.ExtinguisherGas.Gas.GasType;
using InputFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FireType
{
    public abstract class Fire : MonoBehaviour
    {
        [SerializeField] protected FireId _id;
        [SerializeField] protected int _life = 10;
        [SerializeField] protected int _distanceToAbleSprint;
        [SerializeField] protected CharacterMediator _character;
        [SerializeField] private GameObject humo;
        [SerializeField] private AudioClip _audioFire, _audioLaugh;
        private AudioSource _audio;
        protected Canvas _canvas;
        protected Slider _slider;
        protected Transform _fireImage, _fireImage2, _fireTypeParent;
        protected MovementController _movementController;
        private DialButtonOnUI _dialButton;
        protected float _distance = 0;
        private bool _isOnTrigger = false;
        protected float _constantValueMultiplier = 6.2f;
        protected Transform _myTransform;
        protected LineRenderer _lineRenderer;
        protected GameObject _lineRendererParent;
        protected TextMeshPro _textDistance;
        protected Material _materialLine;
        protected float _angle;
        protected ActiveDesactiveObjects _activeDesactiveObjects;
        protected PlayCharacterSounds _sounds;
        protected FireController _fireController;
        private SpriteRenderer _fireImageSprite;
        protected SpriteRenderer _fireCamMap;
        private BossController _boss;
        public static Vector2 _auxDirection;
        private int _lvl;
        private bool _scaleOn = true;
        private bool _isAttack, _isWaitingToAttack;
        [SerializeField] private Sprite[] _facesSprite;
        protected SpriteRenderer _spriteFace;
        private Vector3 _dirAux, _currentPosition;
        private bool _isInstantiateProjectile;
        private float _radiusFire5;
        private bool _destroyInstantiateObject = false;
        public string Id => _id.Value;
        public int DistanceToAbleSprint => _distanceToAbleSprint;
        public float Distance => _distance;
        public float Life => _life;
        public float ConstantValueMultiplier => _constantValueMultiplier;
        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _fireController = FindAnyObjectByType<FireController>();
            DoStart();
            _lvl = FindAnyObjectByType<CharacterInstaller>()._lvl;
            _character = FindAnyObjectByType<CharacterInstaller>()._character;
            _movementController = _character.GetComponent<MovementController>();
            if (Id == "Fire5" && _lvl == 5)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                if (_movementController._isInBossTrigger && transform.parent) _boss = transform.parent.GetComponent<BossController>();
                if (!_boss)
                {
                    foreach (var item in GetComponents<CircleCollider2D>())
                    {
                        if (item.isTrigger)
                        {
                            _radiusFire5 = item.radius;
                            item.radius /= 10;
                        }
                    }
                }
                else
                {
                    _audioFire = _boss._audioFire;
                    _audioLaugh = _boss._audioLaugh;
                    foreach (var item in GetComponents<CircleCollider2D>())
                    {
                        if (item.isTrigger)
                        {
                            _radiusFire5 = item.radius;
                            item.radius = .7f;
                        }
                    }
                }

                foreach (var item in GetComponentsInChildren<SpriteRenderer>())
                    item.enabled = false;
                transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
            }
            _currentPosition = transform.position;
            _spriteFace.sprite = _facesSprite[0];
            //if (Id == "Fire1" && _lvl!=4)
            //    _spriteFace.gameObject.SetActive(false);
            if (Id == "Fire1"&& _lvl != 4)
                _spriteFace.gameObject.SetActive(false);
            else if (Id == "Fire1" && _lvl == 4)
            {
                _spriteFace.gameObject.SetActive(false);
            }
        }
        IEnumerator FireSizeIncrement()
        {
            _scaleOn = false;
            yield return new WaitForSeconds(4f);
            _fireImage.localScale += Vector3.one * .005f;
            _scaleOn = true;
        }
        protected abstract void DoStart();
        private void OnDisable()
        {
            _isInstantiateProjectile = false;
            _isWaitingToAttack = false;
            _isAttack = false;
        }
        void Update()
        {
            if (GetComponent<Rigidbody2D>() && !_destroyInstantiateObject)
            {
                _destroyInstantiateObject = true;
                Destroy(gameObject, 1.25f);
            }
            if (_spriteFace)
            {
                if (_movementController._isInBossTrigger || _spriteFace.transform.localScale == Vector3.zero)
                    return;
            }
            else
                return;
            if (_isOnTrigger)
            {
                _materialLine.mainTextureOffset += new Vector2(Time.deltaTime/20, 1);
                if (!_isWaitingToAttack && transform.parent)
                {
                    //if(_lvl == 4)
                    if (Id != "Fire1" && Id != "Fire2")
                    {
                        if (_lvl == 3)
                            if (_distance < 5 && !_isInstantiateProjectile)
                                StartCoroutine(InstantiateFires(UnityEngine.Random.Range(4f, 5f)));
                        if (_lvl == 4)
                            if (_distance < 5 && !_isInstantiateProjectile)
                                StartCoroutine(InstantiateFires(UnityEngine.Random.Range(3.75f, 4.25f)));
                        if (_lvl == 5 && !_movementController._isInBossTrigger)
                            if (_distance < 5 && !_isInstantiateProjectile)
                                StartCoroutine(InstantiateFires(UnityEngine.Random.Range(3.25f, 3.5f)));
                    }
                }
            }
            //if(!_movementController._isInBossTrigger && _fireImage.localScale.x < 2.1f && _scaleOn) StartCoroutine(FireSizeIncrement());
            if(_lvl >= 2)
            {
                //Aquí estamos en el nivel 4 y es para que el fuego 1 y 2 no tengan ataques
                if (Id == "Fire1") return;
                if (!_isAttack) StartCoroutine(Attack());
                else
                {
                    if (_movementController._isInBossTrigger) return;
                    if (_dirAux == Vector3.zero || Vector3.Distance(_dirAux, transform.position) < .05f)
                    {
                        _angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
                        _dirAux = new Vector3(
                            _currentPosition.x + (.15f * Mathf.Cos(_angle)),
                            _currentPosition.y + (.15f * Mathf.Sin(_angle)), 
                            0
                            );
                    }
                    if (!_isWaitingToAttack) transform.Translate((_dirAux - transform.position).normalized * (.001f * (_lvl / 3)));
                }
            }
        }
        //private void OnEnable()
        //{
        //    _scaleOn = true;
        //}
        protected abstract float DistanceCalc(Transform t);
        protected abstract void DoDestroy();

        IEnumerator Attack()
        {
            //print("Pasó el return;");
            _isAttack = true;
            if (Id != "Fire2") _spriteFace.sprite = _facesSprite[UnityEngine.Random.Range(1, 4)];
            //print("Pasó el return;" + _lvl + ". Time: " + 8f * 3f / _lvl);
            yield return new WaitForSeconds(8f * 3f / _lvl);
            _spriteFace.sprite = _facesSprite[0];
            _isWaitingToAttack = true;
            yield return new WaitForSeconds(8f * 3f / _lvl);
            _dirAux = Vector3.zero;
            _isAttack = false;
            _isWaitingToAttack = false;
            _angle = 0;
            //print("terminó el return;");
        }
        IEnumerator InstantiateFires(float time)
        {
            _isInstantiateProjectile = true;
            Instantiate(humo, transform);
            //var fire = Instantiate(this, transform.position, Quaternion.identity);
            //Instantiate(humo, fire.transform);
            _audio.clip = _audioLaugh;
            if (!_audio.isPlaying) _audio.Play();
            yield return new WaitForSeconds(1f);
            var fire = Instantiate(this, transform.position, Quaternion.identity);
            foreach (var item in fire.transform.GetChild(1).GetComponentsInChildren<Image>())
                item.color = new Color(0, 0, 0, 0);
            fire.transform.GetChild(0).localScale = Vector3.zero;
            fire.transform.GetChild(4).localScale = Vector3.zero;
            fire.transform.localScale /= 2;
            //fire.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = false;
            //fire.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().enabled = false;
            fire.transform.GetChild(2).GetChild(0).transform.localScale = Vector3.zero;
            fire.transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            if(fire.Id != "Fire5") fire.GetComponent<Fire1>().ChangeLife(1);
            if(fire.Id == "Fire5") fire.GetComponent<Fire2>().ChangeLife(1);
            fire.gameObject.AddComponent<Rigidbody2D>();
            var fireRb = fire.GetComponent<Rigidbody2D>();
            fireRb.gravityScale = 0;
            fireRb.velocity = (_character.transform.position - transform.position).normalized * .8f;
            _audio.clip = _audioFire;
            _audio.Play();
            //fire._spriteFace.transform.localScale = Vector3.zero;
            if (fire.Id != "Fire5") fire.transform.GetChild(2).GetChild(2).localScale = Vector3.zero;
            else fire.transform.GetChild(2).GetChild(0).localScale = Vector3.zero;
            foreach (var item in fire.GetComponents<Collider2D>())
                if (item.isTrigger) item.enabled = false;
            yield return new WaitForSeconds(.15f);
            if(Id == "Fire5")
            {
                if (_fireImage.GetComponent<SpriteRenderer>().enabled) fire._fireImage.GetComponent<SpriteRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(time - .65f);
            _isInstantiateProjectile = false;
            //Destroy(fire.gameObject, .5f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                _lineRenderer.gameObject.SetActive(true);
                _character._isInFireTrigger = true;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                if (!transform.GetChild(1).gameObject.activeSelf && _movementController._isInBossTrigger)
                {
                    _lineRenderer.gameObject.SetActive(false);
                    _fireTypeParent.gameObject.SetActive(false);
                    //print("Apagando el Boss LineRenderer");
                    return;
                }
                if (!_movementController) _movementController = _character.GetComponent<MovementController>();                
                if(!_movementController._isInBossTrigger) _movementController._speed = Mathf.Lerp(_movementController._speed, _movementController._initialSpeed / 1.5f, .1f);
                if (!_dialButton) _dialButton = _character._dialButton.GetComponent<DialButtonOnUI>();
                Vector3 direction = collision.transform.position - transform.position;
                float _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _character.transform.rotation = Quaternion.Euler(0, 0, _angle + 90);
                if(JoystickInputAdapter._angleRotAux != _character.transform.rotation.z) JoystickInputAdapter._angleRotAux = _character.transform.rotation.eulerAngles.z;
                _auxDirection = (_myTransform.position - collision.transform.position).normalized;
                _lineRenderer.SetPosition(0, _myTransform.position);
                _lineRenderer.SetPosition(1, collision.transform.position);
                _textDistance.transform.localPosition = (collision.transform.position - _myTransform.position) / 3;
                _textDistance.text = (int)_distance + " mts.";
                _isOnTrigger = true;
                _character._isInFireTrigger = true;
                DistanceCalc(collision.transform);
                if ((int)_distance == _distanceToAbleSprint)
                    _textDistance.color = Color.green;
                else if((int)_distance < _distanceToAbleSprint)
                    _textDistance.color = Color.red;
                else if((int)_distance > _distanceToAbleSprint)
                    _textDistance.color = Color.white;
                _fireTypeParent.gameObject.SetActive(true);
            }
            if (collision.tag == "Lampara" && _lvl == 5)
            {
                if (Id == "Fire5")
                {
                    foreach (var coll in GetComponents<CircleCollider2D>())
                        if (coll.isTrigger)
                            coll.radius = _radiusFire5;
                    if (!_fireImageSprite) _fireImageSprite = _fireImage.GetComponent<SpriteRenderer>();
                    if (!_boss)
                    {
                        //print("No tiene Boss");
                        _fireImageSprite.enabled = true;
                        _spriteFace.enabled = true;
                        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
                            item.enabled = true;
                    }
                    else
                    {
                        _fireImageSprite.enabled = false;
                        _spriteFace.enabled = false;
                        foreach (var item in GetComponents<CircleCollider2D>())
                            if (item.isTrigger)
                                item.radius = .7f;
                    }
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                _lineRenderer.gameObject.SetActive(false);
                _fireTypeParent.gameObject.SetActive(false);
                //transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
                _isOnTrigger = false;
                _character._isInFireTrigger = false;
                _angle = 0;
                if (!_movementController._isInBossTrigger)
                {
                    if(!_movementController._speedActive)
                        _movementController._speed = _movementController._initialSpeed;
                    else 
                        _movementController._speed = _movementController._initialSpeed + _movementController._speedActiveMultiplier;
                }
            }
            if (collision.tag == "Lampara" && _lvl == 5)
            {
                if (Id == "Fire5")
                {
                    foreach (var item in GetComponentsInChildren<SpriteRenderer>())
                        item.enabled = false;
                    if (!_boss)
                    {
                        _fireImageSprite.enabled = false;
                        _spriteFace.enabled = false;
                        foreach (var item in GetComponents<CircleCollider2D>())
                            if (item.isTrigger)
                                item.radius = _radiusFire5 / 10;
                    }
                    else
                    {

                        _lineRenderer.gameObject.SetActive(false);
                        _fireImageSprite.enabled = false;
                        _spriteFace.enabled = false;
                        foreach (var item in GetComponents<CircleCollider2D>())
                            if (item.isTrigger)
                                item.radius = .7f;
                    }
                    transform.GetChild(1).gameObject.SetActive(false);
                    _fireTypeParent.gameObject.SetActive(false);
                    if (!_fireCamMap.enabled) _fireCamMap.enabled = true;
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag!= "Player" && _lvl >= 3)
            {
                if (_dirAux == Vector3.zero)
                {
                    _angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
                }
            }
            if (Id != "Fire5")
            {
                if (transform.GetChild(2).GetChild(2).localScale == Vector3.zero)
                    Destroy(gameObject);
            }
            else
                if (transform.GetChild(2).GetChild(0).localScale == Vector3.zero)
                    Destroy(gameObject);
        }
        public void Damage()
        {
            DoDestroy();
        }
        public void ActiveLineRenderer(bool active)
        {
            if (active)
            {
                _lineRenderer.gameObject.SetActive(true);
            }
            else
            {
                _lineRenderer.gameObject.SetActive(false);
                _isOnTrigger = false;
            }
        }
        public void LineRendererUpdate(Collider2D collision)
        {
            if (!_movementController) _movementController = collision.GetComponent<MovementController>();
            _auxDirection = (_myTransform.position - collision.transform.position).normalized;
            _lineRenderer.SetPosition(0, _myTransform.position);
            _lineRenderer.SetPosition(1, collision.transform.position);
            _textDistance.transform.localPosition = (collision.transform.position - _myTransform.position) / 2;
            _textDistance.text = (int)_distance + " mts.";
            _isOnTrigger = true;
            DistanceCalc(collision.transform);
            if ((int)_distance == _distanceToAbleSprint)
                _textDistance.color = Color.green;
            else if ((int)_distance < _distanceToAbleSprint)
                _textDistance.color = Color.red;
            else if ((int)_distance > _distanceToAbleSprint)
                _textDistance.color = Color.white;
        }
    }
}