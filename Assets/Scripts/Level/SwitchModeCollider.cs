using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerMovementNew;
using Cinemachine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SwitchModeCollider : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> fallingTargets;
    private CinemachineVirtualCamera vcam;
    private CinemachineCameraOffset camOffset;
    private Transform playerTransform;

    public enum PlayerMovementMode
    {
        TapMode,
        RunnerMode,
        FallingMode,
        FlappyMode
    }
    public PlayerMovementMode movementMode;

    private void Start()
    {
        vcam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        camOffset = vcam.GetComponent<CinemachineCameraOffset>();
        playerTransform = vcam.Follow;

    }

    private void LateUpdate()
    {
        // Obtener la posición actual de la cámara
        Vector3 cameraPosition = transform.position;

        // Actualizar solo el componente Y de la posición de la cámara
        cameraPosition.y = playerTransform.position.y;

        // Asignar la nueva posición a la cámara
        transform.position = cameraPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            switch (movementMode)
            {
                case PlayerMovementMode.TapMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.TapMode;                   
                    break;
                case PlayerMovementMode.RunnerMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.RunnerMode;                                      
                    break;
                case PlayerMovementMode.FallingMode:
                    camOffset.m_Offset = new Vector3(0, 0, -15);



                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FallingMode;                   
                    player.transform.position = new Vector3(fallingTargets[0].transform.position.x, player.transform.position.y, player.transform.position.z);
                    collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                    collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //collision.GetComponent<Rigidbody2D>().gravityScale = collision.GetComponent<PlayerMovementNew>().fallingGravity;
                    collision.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -collision.GetComponent<PlayerMovementNew>().fallingVelocity);
                    //collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    //Destroy(collision.GetComponent<Rigidbody2D>());
                    break;
                case PlayerMovementMode.FlappyMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FlappyMode;
                    break;
            }

        }

    }
}
