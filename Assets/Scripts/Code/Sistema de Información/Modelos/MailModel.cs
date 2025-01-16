using System;
using UnityEngine;

namespace Assets.Scripts.Modelos
{
    [Serializable]
    public class MailModel
    {
        public MailModel()
        {
        }

        public MailModel(string key, string correo, string subject, string texto)
        {
            this.key = key;
            this.correo = correo;
            this.subject = subject;
            this.texto = texto;
        }

        public MailModel(string key, string correo, string subject, string texto, string textoPDF)
        {
            this.key = key;
            this.correo = correo;
            this.subject = subject;
            this.texto = texto;
            this.textoPDF = textoPDF;
        }

        public string key;
        public string correo;
        public string subject;
        public string texto;
        public string? textoPDF;
        public string ToJson()
        {
            //return JsonConvert.SerializeObject(this);
            return JsonUtility.ToJson(this);
        }
    }
}
