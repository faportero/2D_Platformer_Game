using Character.ExtinguisherGas.Gas.GasType;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireType
{
    [CreateAssetMenu(menuName = "Create FireConfiguration", fileName = "FireConfiguration", order = 0)]
    public class FireConfiguration : ScriptableObject
    {
        [SerializeField] private Fire[] _firesPrefab;
        private Dictionary<string, Fire> _idToFiresPrefab;
        private void Awake()
        {
            _idToFiresPrefab = new Dictionary<string, Fire>();
            foreach (var fire in _firesPrefab)
            {
                _idToFiresPrefab.Add(fire.Id, fire);
            }
        }
        public Fire GetFiretById(string id)
        {
            if(!_idToFiresPrefab.TryGetValue(id, out var fire))
            {
                throw new Exception($"FireId {id} not found");  
            }
            return fire;
        }
    }
}