using UnityEngine;
using System.Collections;

public class MovePlatforms : MonoBehaviour
{
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float offset = 0f; // Tiempo de inicio aleatorio

    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        startPosition = transform.position;
        endPosition = isHorizontal ? startPosition + Vector3.right * distance : startPosition + Vector3.up * distance;

        // Iniciar la corrutina con un retraso aleatorio basado en el offset
      //  StartCoroutine(StartDelayedMovement(Random.Range(0f, offset)));
        StartCoroutine(StartDelayedMovement(offset));
    }

    private IEnumerator StartDelayedMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float duration = journeyLength / speed;

        while (true)
        {
            for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += Time.deltaTime)
            {
                float fraction = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPosition, endPosition, fraction);
                yield return null;
            }
            transform.position = endPosition;

            for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += Time.deltaTime)
            {
                float fraction = elapsedTime / duration;
                transform.position = Vector3.Lerp(endPosition, startPosition, fraction);
                yield return null;
            }
            transform.position = startPosition;
        }
    }
}
