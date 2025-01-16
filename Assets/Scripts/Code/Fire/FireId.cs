using UnityEngine;

namespace FireType
{
    [CreateAssetMenu(menuName = "Create FireId", fileName = "FireId", order = 0)]
    public class FireId : ScriptableObject
    {
        [SerializeField] private string _value;
        public string Value => _value;
    }
}