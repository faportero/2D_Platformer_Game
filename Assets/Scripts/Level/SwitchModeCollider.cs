using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerMovementNew;




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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            switch (movementMode)
            {
                case PlayerMovementMode.TapMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.TapMode;
                    //collision.GetComponent<PlayerMovementNew>().anim.SetBool("Flappy", false);
                    //collision.GetComponent<PlayerMovementNew>().anim.SetBool("Jump", true);

                    break;
                case PlayerMovementMode.RunnerMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.RunnerMode;                                      
                    break;
                case PlayerMovementMode.FallingMode:
                    //camOffset.m_Offset = new Vector3(initialCamOffset.x, -5, initialCamOffset.z);

                    //startPosition = rbPlayer.position;
                    //startPosition = player.transform.position;
                    //endPosition = new Vector3(fallingTargets[0].transform.position.x, player.transform.position.y, player.transform.position.z);

                    Vector3 closestTargetPosition = GetClosestTargetPosition();
                    startPosition = transform.position;
                    endPosition = new Vector3(closestTargetPosition.x, transform.position.y, transform.position.z);
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

                    collision.GetComponent<Transform>().transform.rotation = Quaternion.identity;

                    break;
                case PlayerMovementMode.FlappyMode:
                    initialCamOffset = camOffset.m_Offset;
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FlappyMode;



                    collision.GetComponent<PlayerMovementNew>().rb.gravityScale = 0;
                    collision.GetComponent<PlayerMovementNew>().rb.velocity = Vector3.zero;
                    collision.GetComponent<PlayerMovementNew>().rb.AddForce(Vector3.up * 20, ForceMode2D.Impulse);
                    collision.GetComponent<PlayerMovementNew>().rb.gravityScale = 4;
                    break;
            }

        }

    }

    // Método para obtener la posición del objetivo más cercano
    public Vector3 GetClosestTargetPosition()
    {
        Vector3 closestTargetPosition = Vector3.zero;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in fallingTargets)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, target.transform.position);

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                closestTargetPosition = target.transform.position;
            }
        }

        return closestTargetPosition;
    }
}

