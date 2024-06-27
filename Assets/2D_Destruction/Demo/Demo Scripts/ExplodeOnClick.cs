using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Explodable))]
public class ExplodeOnClick : MonoBehaviour {

	private Explodable _explodable;
	private ExplosionForce _explosionForce;

	void Start()
	{
		_explodable = GetComponent<Explodable>();

    }
	public void Explode()
	{
		_explodable.explode();
        //ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        _explosionForce = GetComponent<ExplosionForce>();
        _explosionForce.doExplosion(transform.position);
	}
}
	