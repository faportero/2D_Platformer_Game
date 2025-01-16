using UnityEngine;

namespace Character.ExtinguisherGas.Extinguisher
{
    [CreateAssetMenu(menuName = "Create ExtinguisherId", fileName = "ExtinguisherId", order = 0)]
    public class ExtinguisherId : ScriptableObject
    {
        [SerializeField] private int _value;
        public int Value => _value;
    }
}