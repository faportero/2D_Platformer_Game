using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingSlope : MonoBehaviour
{
   // [SerializeField] private bool isRight;
    [SerializeField] PlayerControllerNew player;
    [SerializeField] List<GameObject> slopeTargets;
    private Vector3 startPosition, endPosition;
    private float duration = .25f;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerControllerNew>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector3 closestTargetPosition = GetClosestTargetPosition();
            startPosition = transform.position;
            endPosition = new Vector3(closestTargetPosition.x, player.transform.position.y - 1, player.transform.position.z);
            player.transform.DOMove(endPosition, duration / 15);
            Invoke("ResetMovement", .26f);
        }
    }
    private void ResetMovement()
    {
        player.StartHitBadFloor();

    }
    public Vector3 GetClosestTargetPosition()
    {
        Vector3 closestTargetPosition = Vector3.zero;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in slopeTargets)
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
