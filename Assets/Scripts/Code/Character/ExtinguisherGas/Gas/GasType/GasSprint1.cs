using FireType;
using System.Collections;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas.GasType
{
    public class GasSprint1 : GasSprint
    {
        [SerializeField] private float _speed;
        protected override void DoDestroy()
        {
        }

        protected override void DoMove()
        {
        }

        protected override void DoStart()
        {
            _rigidbody2D.velocity = transform.up * _speed;
            transform.GetChild(0).transform.localScale = Vector3.one * (Random.Range(.7f,.9f));
            //_rigidbody2D.velocity = Fire._auxDirection * _speed;
        }
    }
}