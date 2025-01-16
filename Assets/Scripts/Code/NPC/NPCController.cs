using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Vector2 _currentPosition, _targetPosition;
    private float _radius = 5f;
    private bool _canMove = true;
    private float _angle;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private AudioClip[] _audios;
    private AudioSource _audioSource;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isCollision, _isScreaming = false;
    private Vector3 _firstCollisionPosition;
    public List<NPCController> npcs = new List<NPCController>();
    GameObject child0, child1;
    public bool _changePosition { get; private set; }

    private void Awake()
    {
        _angle = Random.Range(0, Mathf.PI * 2);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        child0 = transform.GetChild(0).gameObject;
        child1 = transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        Move(GetDirection(_isCollision));
        if(!_isScreaming) StartCoroutine(PlayScreamSounds());
    }
    
    IEnumerator PlayScreamSounds()
    {
        _isScreaming = true;
        _audioSource.clip = _audios[Random.Range(0, _audios.Length)];
        yield return new WaitForSeconds(Random.Range(5, 20));
        _audioSource.Play();
        yield return new WaitWhile(()=> _audioSource.isPlaying);
        _audioSource.Stop();
        _isScreaming = false;
    }

    private void OnEnable()
    {
        if(npcs.Count == 0)
        {
            foreach (var item in transform.parent.GetComponentsInChildren<NPCController>())
            {
                npcs.Add(item);
            }
        } 
        _isScreaming = false;
        _audioSource.Stop();
    }
    public void Move(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speed;

        _animator.SetFloat("MovX", direction.x);
        _animator.SetFloat("MovY", direction.y);
    }

    public Vector2 GetDirection(bool isCollision)
    {
        _currentPosition = transform.position;
        if (isCollision)
        {
            GetRandomPosition();
            return Vector2.zero;
        }
        if (Vector2.Distance(_currentPosition, _targetPosition) > .0125f)
        {
            var direction = (_targetPosition - _currentPosition).normalized;
            transform.rotation = GetRotationFromVector(direction);
            return direction / 2;
        }
        else
            GetRandomPosition();
        return Vector2.zero;
    }
    Quaternion GetRotationFromVector(Vector2 direction)
    {
        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg - 90;
        if (angleDegrees < 0)
            angleDegrees += 360f;
        return Quaternion.Euler(0, 0, angleDegrees);
    }
    private Vector2 GetRandomPosition()
    {
        _angle = Random.Range(0, Mathf.PI * 2);
        _targetPosition = new Vector2(
            _currentPosition.x + (_radius * Mathf.Cos(_angle)),
            _currentPosition.y + (_radius * Mathf.Sin(_angle))
            );
        return _targetPosition;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Car")
        {
            transform.Translate((collision.transform.right) * .2f);
            if (!_changePosition) StartCoroutine(CambiarPosicion());
            _isCollision = true;
        }
        else
        {
            transform.Translate(-(collision.transform.position - transform.position).normalized * .025f);
            _isCollision = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        _isCollision = true;
        StartCoroutine(RestartCollision());
    }
    IEnumerator CambiarPosicion()
    {
        _changePosition = true;
        Vector3 newPosition = npcs[Random.Range(0, npcs.Count-1)].transform.position;
        child0.SetActive(false);
        child1.SetActive(false);
        yield return new WaitForSeconds(2f);
        transform.position = newPosition;
        _changePosition = false;
        child0.SetActive(true);
        child1.SetActive(true);
    }
    IEnumerator RestartCollision()
    {
        yield return new WaitForSeconds(Random.Range(2, 10));
        _isCollision = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {        
        _isCollision = false;
    }
}
