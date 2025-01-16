using System;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class InventarioModel
    {
        public InventarioModel()
        {
        }

        public InventarioModel(int error, string msg, int rowCount, List<ObjetoModel> lista)
        {
            this.error = error;
            this.msg = msg;
            this.rowCount = rowCount;
            this.lista = lista;
        }

        public int error;
        public string msg;
        public int rowCount;
        public List<ObjetoModel> lista;
        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
    
}
