using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEspejo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Espejo.countPiezas != transform.parent.GetComponent<Espejo>().maxPiezas)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<UI_PanelDissolve>().StartSolidify();
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
            transform.GetChild(0).GetComponent<UI_PanelDissolve>().StartDissolve();
        }
    }
}
