using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class ExplodePlayer : MonoBehaviour
{
    private Explodable _explodable;
    private ExplosionForce _explosionForce;
    private void OnEnable()
    {
        _explosionForce = GetComponent<ExplosionForce>();
        _explodable = GetComponent<Explodable>();
        _explodable.explode();
        // ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        _explosionForce.doExplosion(transform.position);
    }
    private void Start()
    {

    }
}
