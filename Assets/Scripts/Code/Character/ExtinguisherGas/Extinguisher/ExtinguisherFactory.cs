using UnityEngine;

namespace Character.ExtinguisherGas.Extinguisher
{
    public class ExtinguisherFactory
    {
        private readonly ExtinguishersConfiguration _configuration;

        public ExtinguisherFactory(ExtinguishersConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Extinguisher CreateExtinguisherFactory(GameObject characterObject, int id, Vector3 position, Quaternion rotation)
        {
            var prefab = _configuration.GetExtinguisherById(id);
            Extinguisher extinguisher = Object.Instantiate(prefab, position, rotation);
            extinguisher.transform.SetParent(characterObject.transform);
            return extinguisher;
        }
    }
}