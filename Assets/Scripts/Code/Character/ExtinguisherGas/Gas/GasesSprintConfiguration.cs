using Character.ExtinguisherGas.Gas.GasType;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas
{
    [CreateAssetMenu(menuName = "Create GasesConfiguration", fileName = "GasesConfiguration", order = 0)]
    public class GasesSprintConfiguration : ScriptableObject
    {
        [SerializeField] private GasSprint[] _GasPrefabs;
        private Dictionary<string, GasSprint> _idToGasesPrefab;
        private void Awake()
        {
            _idToGasesPrefab = new Dictionary<string, GasSprint>();
            foreach (var gasSprint in _GasPrefabs)
            {
                _idToGasesPrefab.Add(gasSprint.Id, gasSprint);
            }
        }
        public GasSprint GetGasSprintById(string id)
        {
            if(!_idToGasesPrefab.TryGetValue(id, out var gasSprint))
            {
                throw new Exception($"GasSprint {id} not found");  
            }
            return gasSprint;
        }
    }
}