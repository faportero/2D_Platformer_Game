using System;
using UnityEngine;

//using Newtonsoft.Json;

namespace Assets.Scripts.Modelos
{

    [Serializable]
    public class UsuarioModel
    {
        public int error;
        public string nombre ;
        public string msg ;
        public string info;
        public string correo ;
        public string contrasena ;
        public string id ;
        public int telefono ;
        public int demo ;
        public int scorm ;

        public UsuarioModel()
        {
        }

        public UsuarioModel(int error, string msg, string info, string id, string nombre, string correo, string contrasena, int telefono, int demo, int scorm)
        {
            this.error = error;
            this.msg = msg;
            this.info = info;
            this.id = id;
            this.nombre = nombre;
            this.correo = correo;
            this.contrasena = contrasena;
            this.telefono = telefono;
            this.demo = demo;
            this.scorm = scorm;
        }

        public string ToJson()
        {

            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
