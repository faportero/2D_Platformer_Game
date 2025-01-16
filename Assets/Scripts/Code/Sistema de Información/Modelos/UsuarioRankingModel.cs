using System;
using UnityEngine;

//using Newtonsoft.Json;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class UsuarioRankingModel
    {
        public UsuarioRankingModelUsuario usuario;
        public UsuarioRankingModelObjeto objeto;

        public UsuarioRankingModel()
        {
        }
        public UsuarioRankingModel(UsuarioRankingModelUsuario usuario, UsuarioRankingModelObjeto objeto)
        {
            this.usuario = usuario;
            this.objeto = objeto;
        }
        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
    [Serializable]
    public class UsuarioRankingModelUsuario
    {
        public string nombre;
        public string correo;

        public UsuarioRankingModelUsuario(string nombre, string correo)
        {
            this.nombre = nombre;
            this.correo = correo;
        }
    }
    [Serializable]
    public class UsuarioRankingModelObjeto
    {
        public int identificador;
        public int cantidad;

        public UsuarioRankingModelObjeto(int identificador, int cantidad)
        {
            this.identificador = identificador;
            this.cantidad = cantidad;
        }
    }
}
