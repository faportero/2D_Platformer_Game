using System;
using UnityEngine;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class LoginUsuarioModel : MonoBehaviour
    {
        public string correo;
        public string contrasena;

        public LoginUsuarioModel()
        {
        }

        //public UsuarioModel(int error, string msg, long info, long id, string nombre, string correo, string contrasena, int telefono, int demo, int scorm)
        //{
        //    this.error = error;
        //    this.msg = msg;
        //    this.id = id;
        //    this.info = info;
        //    this.correo = correo;
        //    this.contrasena = contrasena;
        //    this.telefono = telefono;
        //    this.demo = demo;
        //    this.scorm = scorm;
        //}

        public LoginUsuarioModel(string correo, string contrasena)
        {
            this.correo = correo;
            this.contrasena = contrasena;
        }

        public string ToJson()
        {

            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
