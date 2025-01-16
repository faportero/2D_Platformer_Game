using System;
using System.Collections.Generic;
using UnityEngine;

//using Newtonsoft.Json;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class ErrorModel
    {
        public int error;
        public List<string> message;

        public ErrorModel(List<string> message, int error)
        {
            this.error = error;
            this.message = message;
        }

        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
