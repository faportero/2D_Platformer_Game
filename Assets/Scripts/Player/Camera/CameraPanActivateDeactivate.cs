using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanActivateDeactivate : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Invoke("SwitchColliderCamera", 3);
            return;
        }
    }

private void SwitchColliderCamera()
    {
        gameObject.SetActive(false);
        objectToActivate.SetActive(true);
    }
}
