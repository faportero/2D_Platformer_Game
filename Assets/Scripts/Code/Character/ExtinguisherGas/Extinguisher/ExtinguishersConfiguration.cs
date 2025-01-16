using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.ExtinguisherGas.Extinguisher
{
    [CreateAssetMenu(menuName = "Create ExtinguishersConfiguration", fileName = "ExtinguishersConfiguration", order = 0)]
    public class ExtinguishersConfiguration : ScriptableObject
    {
        [SerializeField] private Extinguisher[] _extinguisherPrefabs;
        private Dictionary<int, Extinguisher> _idToGasesPrefab;
        private void Awake()
        {
            _idToGasesPrefab = new Dictionary<int, Extinguisher>();
            foreach (var extinguisher in _extinguisherPrefabs)
            {
                _idToGasesPrefab.Add(extinguisher.Id, extinguisher);
            }
        }
        public Extinguisher GetExtinguisherById(int id)
        {
            if(!_idToGasesPrefab.TryGetValue(id, out var gasSprint))
            {
                throw new Exception($"GasSprint {id} not found");  
            }
            return gasSprint;
        }
    }
}