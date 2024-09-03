using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereraShakeCollider : MonoBehaviour
{
     [SerializeField]private float shakeValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shakeValue == 0)
            {
                CameraManager.instance.StopCameraShake();
                //CameraManager.instance.StartCameraShake(0);
                return;
            }
            else
            {
                CameraManager.instance.StartCameraShake(shakeValue);
                return;
            }
        }
    }
}
