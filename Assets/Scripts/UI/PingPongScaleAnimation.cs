using System.Collections;
using UnityEngine;

public class PingPongScaleAnimation : MonoBehaviour
{
    private float minScale = 0.75f; // Escala mínima
    private float maxScale = 1f;   // Escala máxima
    private float duration = .5f;   // Duración de la animación

    void Start()
    {
        // Iniciar la corutina para la animación de escala
        StartCoroutine(ScalePingPong());
    }

    // Corutina para la animación de ping pong en la escala
    private IEnumerator ScalePingPong()
    {
        while (true)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                float t = Mathf.PingPong(timeElapsed / duration, 1f); // Interpolar entre 0 y 1
                float scale = Mathf.Lerp(minScale, maxScale, t);     // Interpolar entre minScale y maxScale

                transform.localScale = new Vector3(scale, scale, 1f); // Aplicar la escala

                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // Invertir la animación invirtiendo minScale y maxScale
            float temp = minScale;
            minScale = maxScale;
            maxScale = temp;
        }
    }
}
