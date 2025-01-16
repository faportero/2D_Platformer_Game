using FireType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas.GasType
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class GasSprint : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] private GasSprintId _id;
        public string Id => _id.Value;
        private void Start()
        {
            DoStart();
            DestroyGas();
        }
        protected abstract void DoStart();

        private void FixedUpdate()
        {
            DoMove();
        }
        protected abstract void DoMove();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Fire")
            {
                var GasId = Id[Id.Length - 1];
                var Fire = collision.GetComponentInParent<Fire>();
                var FireId = Fire.Id[Fire.Id.Length - 1];
                var Distance = Fire.Distance;
                var DistanceToAbleSprint = Fire.DistanceToAbleSprint;
                if ((int)Distance == DistanceToAbleSprint && collision.GetComponentInParent<WrongFire>() && GasId != FireId)
                        collision.GetComponentInParent<WrongFire>().DoStart();
                if (((int)Distance == DistanceToAbleSprint && GasId == FireId) || Fire.transform.GetChild(0).localScale.x == 0)
                    Fire.Damage();
            }
            else if (collision.tag != "TriggerFire")
            {
                Destroy(gameObject, .3f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
                Destroy(gameObject, .1f);
        }
        protected abstract void DoDestroy();

        private void DestroyGas()
        {
            DoDestroy();
            //Destroy(gameObject, .15f);
            Destroy(gameObject, .35f);
        }
    }
}