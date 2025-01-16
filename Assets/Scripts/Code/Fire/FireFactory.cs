using Character.ExtinguisherGas.Gas.GasType;
using UnityEngine;

namespace FireType
{
    public class FireFactory
    {
        private readonly FireConfiguration _configuration;

        public FireFactory(FireConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Fire CreateFireFactory(string id, Vector3 position, Quaternion rotation)
        {
            var prefab = _configuration.GetFiretById(id);
            return Object.Instantiate(prefab, position, rotation);
        }
    }
}