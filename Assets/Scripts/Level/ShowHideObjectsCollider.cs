using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowHideObjectsCollider : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToActive;
    [SerializeField] private GameObject[] gameObjectsToHide;
    public bool p1,p2,p3,p4;
    private void Start()
    {
        UserData.piezaA_N3 = p1;
        UserData.piezaB_N3 = p2;
        UserData.piezaC_N3 = p3;
        UserData.piezaD_N3 = p4;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UserData.piezaA_N3 || UserData.piezaB_N3 || UserData.piezaC_N3 || UserData.piezaD_N3)
            {
                foreach (GameObject obj in gameObjectsToActive)
                {
                    obj.SetActive(true);
                }
            }
            foreach (GameObject obj in gameObjectsToHide)
            {
                obj.SetActive(false);
            }
        }
    }
}
