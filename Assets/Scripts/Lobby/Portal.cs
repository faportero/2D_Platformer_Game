using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Portal : MonoBehaviour
{
    public string[] limbos;

   [SerializeField] private enum Dimensions
    {
        DimensionA,
        DimensionB,
        DimensionC,
        DimensionD,
    }
  [SerializeField]  private Dimensions dimensions;

    //private void Awake()
    //{
    //    SelectDimension();
    //}

    private void SelectDimension()
    {
        switch (dimensions)
        {
            case (Dimensions.DimensionA):
                SceneManager.LoadScene(limbos[0]);
                break;
            case (Dimensions.DimensionB):
                SceneManager.LoadScene(limbos[1]);
                break;
            case (Dimensions.DimensionC):
                SceneManager.LoadScene(limbos[2]);
                break;
            case (Dimensions.DimensionD):
                SceneManager.LoadScene(limbos[3]);
                break;

            default:
                SceneManager.LoadScene(limbos[0]);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SelectDimension();
        }
    }
}
