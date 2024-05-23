using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToActive; 
    [SerializeField] private GameObject[] gameObjectsToHide;
    public static bool isResetLevel;
    private void Start()
    {
        if (isResetLevel)
        {

            foreach (GameObject obj in gameObjectsToActive)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in gameObjectsToHide)
            {
                obj.SetActive(true);
            }
            isResetLevel = false;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isResetLevel = true;
            SceneManager.LoadScene("Test");
        }
    }
}
