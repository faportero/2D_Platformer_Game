using UnityEngine;
using System.Collections;

public class MovePlatforms : MonoBehaviour
{
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float offset = 0f; // Tiempo de inicio aleatorio
    private Rigidbody2D rb2D;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        startPosition = transform.position;
        endPosition = isHorizontal ? startPosition + Vector3.right * distance : startPosition + Vector3.up * distance;

        // Iniciar la corrutina con un retraso aleatorio basado en el offset
      //  StartCoroutine(StartDelayedMovement(Random.Range(0f, offset)));
        StartCoroutine(StartDelayedMovement(offset));

        rb2D = GetComponent<Rigidbody2D>();
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
    private void EnablePhysics()
    {
        if (rb2D != null)
        {
            rb2D.simulated = true; // Para Rigidbody2D, activa la simulación
            // rb2D.isKinematic = false; // Asegúrate de que no sea kinemático si necesitas que interactúe con la física
        }
    }

    private void DisablePhysics()
    {
        if (rb2D != null)
        {
            rb2D.simulated = false; // Para Rigidbody2D, desactiva la simulación
            // rb2D.isKinematic = true; // Opcional: marca como kinemático si quieres evitar que colisiones se resuelvan
        }
    }

    private void OnBecameVisible()
    {
        EnablePhysics(); // Activar físicas cuando el objeto sea visible
    }

    private void OnBecameInvisible()
    {
        DisablePhysics(); // Desactivar físicas cuando el objeto no sea visible
    }
}
