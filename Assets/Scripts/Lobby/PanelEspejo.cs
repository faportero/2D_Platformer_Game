using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEspejo : MonoBehaviour
{
    private PlayerMovementNew playerMovementNew;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Espejo.countPiezas != transform.parent.GetComponent<Espejo>().maxPiezas)
            {
                playerMovementNew.swipeDetector.gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<UI_PanelDissolve>().StartSolidify();
                transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<UI_PanelDissolve>().StartSolidify();
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Espejo.countPiezas != transform.parent.GetComponent<Espejo>().maxPiezas)
        {
            if(playerMovementNew != null)playerMovementNew.swipeDetector.gameObject.SetActive(true);
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(0).GetComponent<UI_PanelDissolve>().StartDissolve();
            }
               if(transform.GetChild(0).GetChild(1).GetChild(0).gameObject.activeSelf) transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<UI_PanelDissolve>().StartDissolve();
        }
    }
}
