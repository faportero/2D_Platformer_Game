using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    public float elevationDistance = 2f; // Distancia de elevación
    public float elevationDuration = 2f; // Duración de la elevación y cambio de opacidad
    public float rotationDuration = 1f;  // Duración de la rotación
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimatePlayer());
    }

    IEnumerator AnimatePlayer()
    {
        
        // Configuración inicial
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 180, 90);
        spriteRenderer.color = new Color(1, 1, 1, 0); // Opacidad 0

        // Variables de tiempo
        float elapsedTime = 0;

        // Posición inicial y final
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, elevationDistance, 0);

        // Elevación y cambio de opacidad simultáneos
        while (elapsedTime < elevationDuration)
        {
            // Interpolación
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / elevationDuration);
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, elapsedTime / elevationDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la elevación y opacidad terminen en el estado final exacto
        transform.position = endPosition;
        spriteRenderer.color = new Color(1, 1, 1, 1); // Opacidad 1

        // Reiniciar el tiempo para la rotación
        elapsedTime = 0;

        // Rotación inicial y final
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 180, 0);

        // Rotación
        while (elapsedTime < rotationDuration)
        {
            // Interpolación
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la rotación termine en el estado final exacto
        transform.rotation = endRotation;
    }
}
