using System;
using UnityEngine;
//using Newtonsoft.Json;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class ObjetoModel
    {
        public ObjetoModel()
        {
        }

        public ObjetoModel(int identificador, int cantidad)
        {
            this.identificador = identificador;
            this.cantidad = cantidad;
        }

        public int identificador;
        public int cantidad ;
        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }

}
