using Character.ExtinguisherGas.Gas.GasType;
using UnityEngine;

namespace Character.ExtinguisherGas.Gas
{
    public class GasSprintFactory
    {
        private readonly GasesSprintConfiguration _configuration;

        public GasSprintFactory(GasesSprintConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GasSprint CreateGasSprintFactory(string id, Vector3 position, Quaternion rotation)
        {
            var prefab = _configuration.GetGasSprintById(id);
            return Object.Instantiate(prefab, position, rotation);
        }
    }
}