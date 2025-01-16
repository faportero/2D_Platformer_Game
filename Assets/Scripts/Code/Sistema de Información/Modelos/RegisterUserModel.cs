using System;
using UnityEngine;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class RegisterUserModel : MonoBehaviour
    {
        public string nombre;
        public string correo;
        public string contrasena;
        public string telefono;

        public RegisterUserModel()
        {
        }
        public RegisterUserModel(string nombre, string correo, string contrasena, string telefono)
        {
            this.nombre = nombre;
            this.correo = correo;
            this.contrasena = contrasena;
            this.telefono = telefono;
        }

        public string ToJson()
        {

            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
