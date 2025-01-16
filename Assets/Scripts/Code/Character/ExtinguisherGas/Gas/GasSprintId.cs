using UnityEngine;

namespace Character.ExtinguisherGas.Gas
{
    [CreateAssetMenu(menuName = "Create GasId", fileName = "GasId", order = 0)]
    public class GasSprintId : ScriptableObject
    {
        [SerializeField] private string _value;
        public string Value => _value;
    }
}