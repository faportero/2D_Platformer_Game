
//using Newtonsoft.Json;
using System;
using UnityEngine;


namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class Respuesta_Modelo<T>
    {

        public Respuesta_Modelo(T response)
        {
            this.response = response;
        }

        public T response ;

        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
