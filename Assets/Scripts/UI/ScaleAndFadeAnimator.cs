using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScaleAndFadeAnimator : MonoBehaviour
{
    private List<Transform> allTransforms = new List<Transform>();
    private List<Image> allImages = new List<Image>();
    private float duration = 1.0f; // Duración de la animación
    private float timeElapsed = 0.0f;
    private bool animating = false;

    // Agregar AnimationCurve para la escala
    public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    // Objeto a excluir de la animación
    public Transform excludedObject;

    private void Start()
    {
        // Obtener todos los transforms del objeto y sus hijos
        GetAllTransforms(transform);
        // Obtener todos los componentes Image del objeto y sus hijos
        GetAllImages(transform);
        StartAnimation();
    }

    private void Update()
    {
        if (animating)
        {
            timeElapsed += Time.unscaledDeltaTime; // Usar Time.unscaledDeltaTime para animar incluso cuando el juego está en pausa
            float t = timeElapsed / duration;
            t = Mathf.Clamp01(t); // Asegurarse de que t esté en el rango [0, 1]

            foreach (Transform trans in allTransforms)
            {
                // Excluir el objeto especificado y sus hijos si excludedObject no es null
                if (excludedObject != null && (trans == excludedObject || trans.IsChildOf(excludedObject)))
                    continue;

                // Usar la curva de animación para la escala
                float scaleValue = scaleCurve.Evaluate(t);
                trans.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scaleValue);
            }

            foreach (Image img in allImages)
            {
                // Excluir el objeto especificado y sus hijos si excludedObject no es null
                if (excludedObject != null && (img.transform == excludedObject || img.transform.IsChildOf(excludedObject)))
                    continue;

                Color color = img.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                img.color = color;
            }

            if (t >= 1.0f)
            {
                animating = false;
            }
        }
    }

    // Función recursiva para obtener todos los transforms en la jerarquía
    private void GetAllTransforms(Transform parent)
    {
        if (excludedObject != null && parent == excludedObject)
            return;

        allTransforms.Add(parent);
        foreach (Transform child in parent)
        {
            GetAllTransforms(child); // Llama recursivamente para todos los hijos
        }
    }

    // Función recursiva para obtener todos los componentes Image en la jerarquía
    private void GetAllImages(Transform parent)
    {
        if (excludedObject != null && parent == excludedObject)
            return;

        Image image = parent.GetComponent<Image>();
        if (image != null)
        {
            allImages.Add(image);
        }
        foreach (Transform child in parent)
        {
            GetAllImages(child); // Llama recursivamente para todos los hijos
        }
    }

    // Función para iniciar la animación
    public void StartAnimation()
    {
        timeElapsed = 0.0f;
        animating = true;
    }
}
