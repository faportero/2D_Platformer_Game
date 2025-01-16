using Character.CheckLimits;
using Character.ExtinguisherGas.Extinguisher;
using Extinguisher;
using FireType;
using InputFolder;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Character
{
    public class MovementController : MonoBehaviour
    {
        public float _speed;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _dialTransform;
        [SerializeField] private Slider _lifeSlider, _lifeSlider2;
        [SerializeField] private GameObject _dieTrigger;
        [SerializeField] private GameObject _damagePanel;
        [SerializeField] private Material[] _materials;
        [SerializeField] private Animator _coinAnimator;
        [SerializeField] private GameObject _prefabEscudo;
        private Rigidbody2D _rigidbody;
        private ExtinguisherController _extinguisherController;
        private CharacterInterface _character;
        private Transform _myTransform;
        private Camera _camera;
        private Transform _cameraTransform;
        private CheckLimits.CheckLimits _checkLimits;
        private Transform _gasSpawnPosition;
        private bool _isCollisionwithObjects;
        private Transform _collisionTransform;
        public int _startVida = 20;
        [HideInInspector] public float _initialSpeed;
        private int _vida;
        private CharacterMediator _characterMediator;
        private PlayCharacterSounds _sounds;
        private bool isInNewExtinguisher;
        private float timeToCollision;
        private TimerHud _timerHud;
        private EscogerAudio _escogerAudio;
        [HideInInspector] public bool _isInBossTrigger;
        [HideInInspector] public bool _speedActive;
        [HideInInspector] public float _speedActiveMultiplier = .5f;
        Color _colorOutliner, _colorOutliner2;
        private bool _changeColor;
        private int _lvl;
        float _completeLife;
        Animator _animLifeSlider;

        private void Awake()
        {
            _initialSpeed = _speed;
            _vida = _startVida;
            _lifeSlider.value = _vida;
            _animLifeSlider = _lifeSlider.GetComponentInChildren<Animator>();
            _completeLife = _lifeSlider.maxValue;
            _myTransform = transform;
            _characterMediator = GetComponent<CharacterMediator>();
            _sounds = GetComponent<PlayCharacterSounds>();
            _gasSpawnPosition = _myTransform.GetChild(0);
            _camera = Camera.main;
            _cameraTransform = _camera.transform;
            if (!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
            if (!_animator) _animator = GetComponent<Animator>();
            _extinguisherController = GetComponent<ExtinguisherController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _timerHud = FindAnyObjectByType<TimerHud>();
            _escogerAudio = FindAnyObjectByType<EscogerAudio>();
            _colorOutliner = _materials[1].GetColor("_Color");
            _colorOutliner2 = _materials[1].GetColor("_Color2");
            _changeColor = false;
            _speedActive = false;
            _spriteRenderer.material = _materials[0];
            _lvl = FindAnyObjectByType<CharacterInstaller>()._lvl;
            //if (SystemInfo.supportsGyroscope)
            //{
            //    // Habilitar el giroscopio
            //    Input.gyro.enabled = true;
            //}
            //else
            //{
            //    Debug.Log("Giroscopio no disponible en este dispositivo.");
            //}
            ChangeColorLifeSlider();
            _extinguisherController.SetColorCarga();
        }
        public void Configure(CharacterInterface character, CheckLimits.CheckLimits checkLimits)
        {
            _character = character;
            _checkLimits = checkLimits;
        }

        //Lógica para desplazar el character
        public void Move(Vector2 direction, Vector3 rotation)
        {
            _cameraTransform.position = new Vector3(_myTransform.position.x, _myTransform.position.y + .25f, _cameraTransform.position.z);
            _rigidbody.velocity = direction * _speed;

            if (_extinguisherController.GetValueId() == 0)
                _animator.SetBool("ExtinguisherActive", false);
            else
                _animator.SetBool("ExtinguisherActive", true);

            ClampFinalPosition();
            _animator.SetFloat("MovX", direction.x);
            _animator.SetFloat("MovY", direction.y);



            if (direction != Vector2.zero)
            {
                _myTransform.rotation = GetRotationFromVector(direction);
                _animator.SetFloat("Speed", Vector2.Distance(direction * 4, Vector2.zero));
            }
            else
            {
                _animator.SetFloat("Speed", 1);
                if (_dialTransform.gameObject.activeSelf)
                {
                    if (Time.timeScale > 0) _myTransform.rotation = Quaternion.Euler(0, 0, GetRotationFromVector(direction).z + rotation.z);
                }
            }
            _gasSpawnPosition.rotation = _myTransform.rotation;

            if (_speedActive)
            {
                if (_spriteRenderer.material.HasProperty("_Color") && !_changeColor)
                {
                    StartCoroutine(ChangeColorOutline());
                }
            }
            else
            {
                if (_spriteRenderer.material.GetColor("_Color") != Color.white) _spriteRenderer.material.SetColor("_Color", Color.white);
            }
        }

        IEnumerator ChangeColorOutline()
        {
            _changeColor = true;
            _spriteRenderer.material.SetColor("_Color", _colorOutliner);
            yield return new WaitForSeconds(.15f);
            _spriteRenderer.material.SetColor("_Color", _colorOutliner2);
            yield return new WaitForSeconds(.15f);
            _changeColor = false;
        }

        Quaternion GetRotationFromVector(Vector2 direction)
        {
            // Obtener el ángulo en radianes usando Atan2
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            // Convertir el ángulo de radianes a grados
            float angleDegrees = angleRadians * Mathf.Rad2Deg - 90;
            // Ajustar el ángulo para que esté en el rango [0, 360]
            if (angleDegrees < 0)
                angleDegrees += 360f;
            return Quaternion.Euler(0, 0, angleDegrees);
        }
        private void ClampFinalPosition()
        {
            var viewportPoint = _camera.WorldToViewportPoint(_myTransform.position);
            viewportPoint.x = Mathf.Clamp(viewportPoint.x, 0.03f, 0.97f);
            viewportPoint.y = Mathf.Clamp(viewportPoint.y, 0.03f, 0.97f);
            _myTransform.position = _camera.ViewportToWorldPoint(viewportPoint);
        }
        IEnumerator RestartCollision()
        {
            yield return new WaitForSeconds(.25f);
            _isCollisionwithObjects = false;
        }
        public bool IsCollisioningWithObject()
        {
            return _isCollisionwithObjects;
        }
        IEnumerator DamagePanelView()
        {
            _damagePanel.SetActive(true);
            Animator anim = _damagePanel.GetComponent<Animator>();
            AnimationClip animacion = anim.runtimeAnimatorController.animationClips[0];
            yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
            _damagePanel.SetActive(false);
        }
        public void Daño(int lifeToReduce)
        {
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).name == "Escudo")
                    return;
            StartCoroutine(DamagePanelView());
            if (_lifeSlider2) _lifeSlider2.fillRect.GetComponent<Image>().color = _lifeSlider.fillRect.GetComponent<Image>().color;
            _vida -= lifeToReduce;
            _sounds.PlaySound();
            StartCoroutine(MoveVidaSlider());
            if (_vida <= 0)
            {
                //Aquí se aumentan los intentos!!! 
                if (_lvl == 1) StarsView._intentos1 += 1;
                if (_lvl == 2) StarsView._intentos2 += 1;
                if (_lvl == 3) StarsView._intentos3 += 1;
                if (_lvl == 4) StarsView._intentos4 += 1;
                if (_lvl == 5) StarsView._intentos5 += 1;
                if (_lvl == 5 && FindAnyObjectByType<BossController>()) StarsView._intentosBoss += 1;
                _dieTrigger.SetActive(true);
                _dieTrigger.GetComponent<ActiveDesactiveObjects>().DoStart();
            }
        } 
        public void ChangeColorLifeSlider()
        {
            if (_vida >= _completeLife / 2)
            {
                _lifeSlider.fillRect.GetComponent<Image>().color = Color.green;
                if(_animLifeSlider)
                    if (_animLifeSlider.enabled) 
                        _animLifeSlider.enabled = false;
            }
            else if (_vida < _completeLife / 2 && _vida > _completeLife / 4)
            {
                _lifeSlider.fillRect.GetComponent<Image>().color = Color.yellow;
                if (_animLifeSlider)
                    if (!_animLifeSlider.enabled)
                    {
                        _animLifeSlider.enabled = true;
                        _animLifeSlider.SetFloat("Multiplier", 1f);
                    }
            }
            else if (_vida <= _completeLife / 4)
            {
                _lifeSlider.fillRect.GetComponent<Image>().color = Color.red;
                if (_animLifeSlider)
                    if (!_animLifeSlider.enabled)
                    {
                        _animLifeSlider.enabled = true;
                        _animLifeSlider.SetFloat("Multiplier", 1.5f);
                    }
            }
            if (_lifeSlider2) _lifeSlider2.fillRect.GetComponent<Image>().color = _lifeSlider.fillRect.GetComponent<Image>().color;
        }
        IEnumerator MoveVidaSlider()
        {
            ChangeColorLifeSlider();
            while (_lifeSlider.value != _vida)
            {
                _lifeSlider.wholeNumbers = false;
                _lifeSlider.value = Mathf.Lerp(_lifeSlider.value, _vida, .25f);
                if (_lifeSlider2)
                {
                    _lifeSlider2.wholeNumbers = false;
                    _lifeSlider2.value = _lifeSlider.value;
                }
                yield return new WaitForSeconds(.025f);
            }
            _lifeSlider.value = _vida;
            _lifeSlider.wholeNumbers = true;
            if (_lifeSlider2)
            {
                _lifeSlider2.value = _vida;
                _lifeSlider2.wholeNumbers = true;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.transform.tag == "TriggerFire")
            {
                Daño(1);
            }
            if(collision.transform.tag == "Car")
            {
                timeToCollision = 1f;
                Daño(2);
                //print("Daño : 1. Vida : " + _vida);
            }
            //else
            //{
            //    _collisionTransform = transform;
            //    transform.Translate(-(collision.transform.position - transform.position).normalized * .025f);
            //    _isCollisionwithObjects = true;
            //    StartCoroutine(RestartCollision());
            //}
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.tag == "Car")
            {
                timeToCollision += Time.deltaTime;
                if ((int)timeToCollision % 3 == 0)
                {
                    Daño(1);
                    timeToCollision += .5f;
                    //print("Daño : 2. Vida : " + _vida);
                }
            }
            else
            {
                _collisionTransform = transform;
                transform.Translate(-(_collisionTransform.transform.position - transform.position).normalized * .05f);
                StartCoroutine(RestartCollision());
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Extinguisher" && !isInNewExtinguisher)
            {
                isInNewExtinguisher = true;
                if (collision.GetComponent<ActiveExtinguisher>()._idExtinguisher == _extinguisherController.GetValueId())
                {
                    _sounds.AddTextItems("Extintor", "Ya se encuentra equipado el extintor tipo " + tipoFuego());
                }
                else if (collision.GetComponent<ActiveExtinguisher>()._idExtinguisher != _extinguisherController.GetValueId())
                {
                    collision.GetComponent<ActiveExtinguisher>().CreateExtinguisher();
                    _sounds.AddTextItems("Extintor", "Extintor tipo " + tipoFuego() + " equipado");
                    ActiveDesactiveObjects _activeObject = collision.GetComponent<ActiveDesactiveObjects>();
                    if (_activeObject)
                    {
                        _activeObject.enabled = true;
                        _activeObject.DoStart();
                        if (_activeObject._pausaGame)
                        {
                            _characterMediator.GamePause(true);
                            _characterMediator._joystickMovement.ResetValues();
                        }
                    }
                    foreach (var item in collision.GetComponentsInChildren<SpriteRenderer>())
                    {
                        item.enabled = false;
                    }
                    collision.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                    StartCoroutine(PlaySoundItem(collision.gameObject));
                }
            }
            if (collision.tag == "Trigger")
            {
                if (collision.GetComponent<ActiveDesactiveObjects>())
                {
                    ActiveDesactiveObjects _activeObject = collision.GetComponent<ActiveDesactiveObjects>();
                    _activeObject.enabled = true;
                    _activeObject.DoStart();
                    if (_activeObject._pausaGame)
                        _characterMediator.GamePause(true);
                    _characterMediator._input.GetDirection(true);
                    if (collision.GetComponent<ActiveDesactiveObjects>()._destroy) Destroy(collision.gameObject, .25f);
                }
            }
            if (collision.tag == "TriggerChangePosition")
            {
                if (collision.GetComponent<ChangePosition>())
                {
                    ChangePosition _changePosition = collision.GetComponent<ChangePosition>();
                    _changePosition.MoveToAnotherPos();
                }
            }
            if(collision.tag == "Coin")
            {
                //_coinAnimator.gameObject.SetActive(false);
                //StartCoroutine(PlayCoinAnim());
                ControlDatos._coins += 20;
                //PlayerPrefs.SetInt("Coins", ControlDatos._coins);
                PlayerPrefs.SetInt("Coins", ControlDatos._coins);

                collision.GetComponent<SpriteRenderer>().enabled = false;
                _sounds.AddTextCoins();
                _sounds.AddTextItems("Monedas", "+20 monedas");
                StartCoroutine(PlaySoundItemCoins(collision.gameObject));
            }
            if(collision.tag == "Botiquin")
            {
                //print("_vida: " + _vida + ". maxValue: " + _lifeSlider.maxValue);
                if (_vida == _lifeSlider.maxValue)
                {
                    _sounds.AddTextItems("Botiquin", "Vida completa");
                    return;
                }
                _vida += 10;
                _vida = Mathf.Clamp(_vida, 0, (int)_lifeSlider.maxValue);
                //print("_vida: " + _vida + ". StartVida: " + _startVida);
                StartCoroutine(MoveVidaSlider());
                _sounds.AddTextItems("Botiquin", "Has obtenido un botiquín");
                if (_lvl == 3) StartCoroutine(InstantiateNewObject(collision.gameObject, 90));
                else if (_lvl == 4) StartCoroutine(InstantiateNewObject(collision.gameObject, 75));
                else if (_lvl == 5 && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 60));
                else if (_lvl == 5 && _isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 30));
                var sprites = collision.GetComponentsInChildren<SpriteRenderer>();
                foreach (var item in sprites)
                {
                    item.enabled = false;
                }
                collision.GetComponent<Collider2D>().enabled = false;
                foreach (var item in sprites)
                {
                    item.enabled = false;
                }
                StartCoroutine(PlaySoundItem(collision.gameObject));
            }
            if(collision.tag == "Speed")
            {
                if (_speedActive)
                {
                    StopCoroutine("SetSpeed");
                    _speedActive = false;
                }
                StartCoroutine(SetSpeed());
                if (_lvl == 3) StartCoroutine(InstantiateNewObject(collision.gameObject, 90));
                else if (_lvl == 4) StartCoroutine(InstantiateNewObject(collision.gameObject, 75));
                else if (_lvl == 5 && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 60));
                else if (_lvl == 5 && _isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 30));
                StartCoroutine(PlaySoundItem(collision.gameObject));
            }
            if (collision.tag == "Time")
            {
                if (!_timerHud) _timerHud = FindAnyObjectByType<TimerHud>();
                var _auxtime = _timerHud.AddTime(30);
                _sounds.AddTextItems("Timer", "+ "+ (int)_auxtime + " segundos");
                if ((_lvl == 4 || _lvl == 5) && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 120));
                else if (_lvl == 5 && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 90));
                StartCoroutine(PlaySoundItem(collision.gameObject));
            }
            if (collision.tag == "Burbuja")
            {
                InstantiateEscudo();
                _sounds.AddTextItems("Escudo", "Has obtenido un escudo protector");
                if ((_lvl == 4 || _lvl == 5) && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 120));
                else if (_lvl == 5 && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 90));
                StartCoroutine(PlaySoundItem(collision.gameObject));
            }
            if (collision.tag == "Recarga")
            {
                var valorRecarga = .2f;
                if (_characterMediator.valorCarga < .95f) _characterMediator.valorCarga += valorRecarga;
                else
                {
                    _sounds.AddTextItems("Recarga", "Carga completa del extintor tipo " + tipoFuego());
                    return;
                }
                _sounds.AddTextItems("Recarga", "Extintor tipo " + tipoFuego() + " recargado");
                if ((_lvl == 4 || _lvl == 5) && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 120));
                else if (_lvl == 5 && !_isInBossTrigger) StartCoroutine(InstantiateNewObject(collision.gameObject, 90));
                StartCoroutine(PlaySoundItem(collision.gameObject));
            }
            if (collision.tag == "BossTrigger")
            {
                _extinguisherController.SetActiveIndicators(false);
                _isInBossTrigger = true;
                _extinguisherController.StopAudioSourceCarga();
                _extinguisherController.EnableDisableAudioSourceCarga(false);
            }
        }
        private void OnTriggerStay(Collider collision)
        {
            if (collision.tag == "BossTrigger")
            {
                if (!_isInBossTrigger)
                {
                    _extinguisherController.SetActiveIndicators(false);
                    _isInBossTrigger = true;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Extinguisher")
            {
                isInNewExtinguisher = false;
            }
        }
        private string tipoFuego()
        {
            var _tipoFuego = "A";
            if (_extinguisherController.GetValueId() == 1) _tipoFuego = "A";
            if (_extinguisherController.GetValueId() == 2) _tipoFuego = "B";
            if (_extinguisherController.GetValueId() == 3) _tipoFuego = "C";
            if (_extinguisherController.GetValueId() == 4) _tipoFuego = "K";
            if (_extinguisherController.GetValueId() == 5) _tipoFuego = "D";
            return _tipoFuego;
        }
        private void InstantiateEscudo()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "Escudo")
                    Destroy(transform.GetChild(i).gameObject);
            }
            var _instanceEscuco = Instantiate(_prefabEscudo, transform);
            _instanceEscuco.name = "Escudo";
            Destroy(_instanceEscuco.gameObject, 10);
        }
        IEnumerator PlayCoinAnim()
        {
            _coinAnimator.gameObject.SetActive(true);
            AnimationClip animacion = _coinAnimator.runtimeAnimatorController.animationClips[1];
            //print("name " + animacion.name + " tiempo: " + animacion.averageDuration / Mathf.Abs(_coinAnimator.GetFloat("ExitSpeed")));
            yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(_coinAnimator.GetFloat("ExitSpeed")));
            yield return new WaitForSecondsRealtime(5);
            _coinAnimator.Play("AnimacionSalida");
            yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(_coinAnimator.GetFloat("ExitSpeed")));
            _coinAnimator.gameObject.SetActive(false);
        }
        IEnumerator PlaySoundItem(GameObject collider)
        {
            foreach (var item in collider.GetComponentsInChildren<SpriteRenderer>())
                item.enabled = false;
            AudioSource _audio = collider.GetComponent<AudioSource>();
            _audio.Play();
            float time = _audio.clip.length;
            yield return new WaitForSeconds(time);
            Destroy(collider);
        }
        IEnumerator PlaySoundItemCoins(GameObject collider)
        {
            foreach (var item in collider.GetComponentsInChildren<SpriteRenderer>())
                item.enabled = false;
            AudioSource _audio = collider.GetComponent<AudioSource>();
            _audio.enabled = true;
            float time = _audio.clip.length;
            yield return new WaitForSeconds(time);
            Destroy(collider);
        }
        IEnumerator InstantiateNewObject(GameObject collider, float relativeTime)
        {
            GameObject newObject = Instantiate(collider, collider.transform.parent);
            newObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(relativeTime, relativeTime + 10));
            newObject.SetActive(true);
        }
        IEnumerator SetSpeed()
        {
            _speedActive = true;
            _sounds.AddTextItems("Speed", "Velocidad aumentada");
            _speed = _initialSpeed * 1.25f;
            _escogerAudio.audioSource.pitch = 1.15f;
            _spriteRenderer.material = _materials[1];
            yield return new WaitForSeconds(15);
            _speed = _initialSpeed;
            _sounds.AddTextItems("Speed", "Velocidad normal");
           _escogerAudio.audioSource.pitch = 1f;
            _speedActive = false;
            _spriteRenderer.material = _materials[0];
        }
        public void RestartLife(int value)
        {
            _vida = value;
            ChangeColorLifeSlider();
        }
        public void SetLifeSlider2Value()
        {
            _lifeSlider2.value = _lifeSlider.value;
            ChangeColorLifeSlider();
        }
    }
}