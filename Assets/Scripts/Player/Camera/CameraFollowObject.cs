using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipyRotationTime = .5f;

    private Coroutine turnCoroutine;
    private PlayerMovementNew playerMovement;
    private bool isFacingRight;

    private void Awake()
    {
        playerMovement = playerTransform.gameObject.GetComponent<PlayerMovementNew>();
        isFacingRight = playerMovement.isFacingRight;
    }
    private void Update()
    {
        transform.position = playerTransform.position;
    }
    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = DetermihneEndRotation();
        float yRotation = 0;

        float elapsedTime = 0;
        while (elapsedTime < flipyRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / flipyRotationTime);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            
            yield return null;
        }
    }
    private float DetermihneEndRotation()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight)
        {
            return 180;
        }
        else
        {
            return 0;
        }
    }
}
