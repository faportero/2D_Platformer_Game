using FireType;
using System.Collections;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas.GasType
{
    public class GasSprint2 : GasSprint
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


//using System.Collections;
//using UnityEngine;

//namespace Character.ExtinguisherGas.Gas.GasType
//{
//    [RequireComponent(typeof(BoxCollider2D))]
//    [RequireComponent(typeof(Rigidbody2D))]
//    public class GasSprint2 : GasSprint
//    {
//        [SerializeField] private Rigidbody2D _rigidbody2D;
//        [SerializeField] private float _speed;

//        // Start is called before the first frame update
//        void Start()
//        {
//            _rigidbody2D.velocity = transform.up * _speed;
//            StartCoroutine(Destroy(.25f));
//        }
//        private IEnumerator Destroy(float sec)
//        {
//            yield return new WaitForSeconds(sec);
//            Destroy(gameObject);
//        }
//    }
//}