using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    [SerializeField] private string url;
    // Método que se llama cuando el panel es presionado
    public void OnPanelClick()
    {
        Application.OpenURL(url);
    }
}
