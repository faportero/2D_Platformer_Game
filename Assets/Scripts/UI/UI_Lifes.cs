using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lifes : MonoBehaviour
{
    
    [HideInInspector] public List<GameObject> lifes = new List<GameObject>();
    [SerializeField] private UI_Habilidades uiHabilidades;
    private PlayerController playerController;
    public int lifesCount;
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();

        lifes.Clear();
        
        if (playerController != null && playerController.vidaExtra)
        {
           Instantiate(transform.GetChild(0).gameObject, transform);                     
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            lifes.Add(transform.GetChild(i).gameObject);
        }

        lifesCount = lifes.Count;
    }
    public void UpdateLife()
    {
        if (lifes.Count >= 0)
        {
            GameObject lastChild = lifes[lifes.Count - 1];  
            lastChild.GetComponent<Image>().color = Color.black;
            lifes.Remove(lastChild);
            lifesCount = lifes.Count;
        }   
    }
}
