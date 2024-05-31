using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerMovementNew;
using Cinemachine;



public class SwitchModeCollider : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private List<GameObject> fallingTargets;
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineCameraOffset camOffset;
    private Vector3 initialCamOffset;

    private Vector3 startPosition;
    private Vector3 endPosition;
    public float duration = .25f;
    private float elapsedTime = 0f;
    private bool hasFinishedLerping = false;
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
        // vcam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        camOffset = vcam.GetComponent<CinemachineCameraOffset>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
        initialCamOffset = camOffset.m_Offset;
        // playerTransform = vcam.Follow;

    }

    private void Update()
    {
        //// Obtener la posición actual de la cámara
        //Vector3 cameraPosition = transform.position;

        //// Actualizar solo el componente Y de la posición de la cámara
        //cameraPosition.y = playerTransform.position.y;

        //// Asignar la nueva posición a la cámara
        //transform.position = cameraPosition;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            switch (movementMode)
            {
                case PlayerMovementMode.TapMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.TapMode;                   
                    break;
                case PlayerMovementMode.RunnerMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.RunnerMode;                                      
                    break;
                case PlayerMovementMode.FallingMode:
                    camOffset.m_Offset = new Vector3(initialCamOffset.x, -5, initialCamOffset.z);
                   
                    //startPosition = rbPlayer.position;
                    startPosition = player.transform.position;
                    endPosition = new Vector3(fallingTargets[0].transform.position.x, player.transform.position.y, player.transform.position.z);
                    player.transform.DOMove(endPosition, duration/15);
                    //StartCoroutine(LerpPosition());
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FallingMode;     
                    //player.transform.position = new Vector3(fallingTargets[0].transform.position.x, player.transform.position.y, player.transform.position.z);
                    collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                    collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //collision.GetComponent<Rigidbody2D>().gravityScale = collision.GetComponent<PlayerMovementNew>().fallingGravity;
                    collision.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -collision.GetComponent<PlayerMovementNew>().fallingVelocity);
                    //collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    //Destroy(collision.GetComponent<Rigidbody2D>());
                    break;
                case PlayerMovementMode.FlappyMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FlappyMode;
                    break;
            }

        }

    }

    private IEnumerator LerpPosition()
    {
        //yield return new WaitForSeconds(.02f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calcular el valor interpolado t
            float t = elapsedTime / duration;
            
            print (t);

            // Interpolar entre startPosition y endPosition
            Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);

            // Asignar la nueva posición al objeto
            player.transform.position = interpolatedPosition;

            // Incrementar el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Esperar al siguiente frame
            yield return null;
        }

        // Asegurar que la posición final se asigna correctamente
        player.transform.position = endPosition;

        // Marcar que el Lerp ha terminado
        hasFinishedLerping = true;
        Debug.Log("Lerp terminado.");
    }


}

