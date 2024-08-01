using System.Collections;
using UnityEngine;

public class VortexAnimation : MonoBehaviour
{
    [Header("Sprites a animar")]
    public SpriteRenderer vortexPipeSprite; // SpriteRenderer del objeto VortexPipe
    public SpriteRenderer vortexRaysSprite; // SpriteRenderer del objeto VortexRays

    [Header("Configuración de animación")]
    public AnimationCurve vortexPipeAnimationCurve; // Curva de animación para VortexPipe
    public AnimationCurve vortexRaysAnimationCurve; // Curva de animación para VortexRays
    public float vortexPipeAnimationDuration = 1.0f; // Duración de la animación VortexPipe
    public float vortexRaysAnimationDuration = 1.0f; // Duración de la animación VortexRays
    public float vortexRaysOffset = 0.5f; // Tiempo de offset para VortexRays

    private Material vortexPipeMaterial; // Material del objeto VortexPipe
    private Material vortexRaysMaterial; // Material del objeto VortexRays

    private void Start()
    {
        // Obtener los materiales de los SpriteRenderers
        if (vortexPipeSprite != null)
        {
            vortexPipeMaterial = vortexPipeSprite.material;
        }
        if (vortexRaysSprite != null)
        {
            vortexRaysMaterial = vortexRaysSprite.material;
        }

        // Iniciar la animación
        StartCoroutine(AnimateDissolve());
    }

    private IEnumerator AnimateDissolve()
    {
        // Animar VortexPipe
        if (vortexPipeMaterial != null)
        {
            yield return AnimateMaterial(vortexPipeMaterial, vortexPipeAnimationCurve, vortexPipeAnimationDuration);
        }

        // Esperar el tiempo de offset antes de animar VortexRays
        yield return new WaitForSeconds(vortexRaysOffset);

        // Animar VortexRays
        if (vortexRaysMaterial != null)
        {
            yield return AnimateMaterial(vortexRaysMaterial, vortexRaysAnimationCurve, vortexRaysAnimationDuration);
        }
    }

    private IEnumerator AnimateMaterial(Material material, AnimationCurve curve, float duration)
    {
        float elapsedTime = 0f;

        // Animar desde 0 hasta 1 usando la curva
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = curve.Evaluate(t); // Obtener valor de la curva
            material.SetFloat("_DissolveAmount", curveValue); // Aplicar el valor al material

            elapsedTime += Time.deltaTime;
            yield return null; // Esperar un frame
        }

        // Asegurarse de que el valor final sea 1
        material.SetFloat("_DissolveAmount", curve.Evaluate(1f));
    }
}
