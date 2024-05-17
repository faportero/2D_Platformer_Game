using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lifes : MonoBehaviour
{
    
    [HideInInspector] public List<GameObject> lifes = new List<GameObject>();
    private void Start()
    {
        lifes.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            lifes.Add(transform.GetChild(i).gameObject);
        }
    }
    public void UpdateLife()
    {
        if (lifes.Count >= 0)
        {            
            GameObject lastChild = lifes[lifes.Count - 1];            
            lifes.Remove(lastChild);
            lastChild.GetComponent<Image>().color = Color.black;

        }
        //else if (lifes.Count == 0)
        //{
        //    foreach (GameObject child in lifes)
        //    {
        //        child.GetComponent<Image>().color = Color.black;
        //    }
        //}
    }
}
