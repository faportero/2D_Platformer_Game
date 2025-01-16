using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotatation : MonoBehaviour
{
    private Transform _player;
    Vector3 _direction;
    float _angle, _startAngle, _distance;
    Vector3 _localScale;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindAnyObjectByType<CharacterMediator>().transform;
        _startAngle = transform.localRotation.z;
        _localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _startAngle));
            return;
        }
        _distance = Vector3.Distance(_player.position, transform.position);
        if (_distance < 1.25f)
        {
            if (transform.localScale.x != 0) transform.localScale = Vector3.zero;
        }
        else
        {
            if (transform.localScale.x != _localScale.x) transform.localScale = _localScale;
            _direction = (_player.position - transform.position).normalized;
            // Calcular el ángulo en el eje Z
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            // Aplicar la rotación
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _angle + 90));
        }
    }
}
