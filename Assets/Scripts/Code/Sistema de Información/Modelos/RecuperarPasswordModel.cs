using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecuperarPasswordModel : MonoBehaviour
{
    public string key;
    public string nombre;
    public string correo;

    public RecuperarPasswordModel()
    {
    }
    public RecuperarPasswordModel(string key, string nombre, string correo)
    {
        this.key = key;
        this.nombre = nombre;
        this.correo = correo;
    }
    public string ToJson()
    {
        //return JsonConvert.SerializeObject(this);
        return JsonUtility.ToJson(this);
    }
}
