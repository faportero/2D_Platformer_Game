using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.ExtinguisherGas.Extinguisher
{
    public class Extinguisher : MonoBehaviour
    {
        [SerializeField] private ExtinguisherId _id;

        public int Id => _id.Value;
    }
}