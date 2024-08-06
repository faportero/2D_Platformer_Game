using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Clase que maneja el disolverse y solidificarse de un panel
public class UI_PanelDissolve : UI_Animation, IMaterialModifier
{
    // Duración de la animación y curvas de aceleración para disolver y solidificar
    public float dissolveDuration = .5f; // Duración para disolución
    public AnimationCurve dissolveCurve; // Curva para la disolución
    public float solidifyDuration = .5f;  // Duración para solidificación
    public AnimationCurve solidifyCurve;  // Curva para la solidificación

    // Almacena el valor actual de disolución
    private float currentDissolveAmount = 0f;

    // Método que modifica el material base para aplicar el efecto de disolución
    public Material GetModifiedMaterial(Material baseMaterial)
    {
        // Asegúrate de que el material tenga la propiedad "_DissolveAmount"
        if (baseMaterial.HasProperty("_DissolveAmmount"))
        {
            // Establece el valor de disolución según el estado actual
            baseMaterial.SetFloat("_DissolveAmmount", currentDissolveAmount);
        }
        return baseMaterial;
    }
    private void OnEnable()
    {
        StartSolidify(); // Inicia la disolución al comenzar
        
    }
    private void Start()
    {
    }

    // Método para iniciar la disolución
    public void StartDissolve()
    {
        StartCoroutine(DissolveCoroutine());
        AudioManager.Instance.PlaySfx("Panel_hide");
    }

    // Corrutina que maneja la animación de disolución
    private IEnumerator DissolveCoroutine()
    {
        currentDissolveAmount = 0;

        float timer = 0f;
        float initialAmount = 0f; // Valor inicial de disolución

        // Animar de 0 a 1 usando la curva de disolución
        while (timer < dissolveDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / dissolveDuration; // Normaliza el tiempo
            float curveValue = dissolveCurve.Evaluate(t); // Evalúa la curva
            currentDissolveAmount = Mathf.Lerp(initialAmount, 1f, curveValue); // Interpola entre 0 y 1
            GetComponent<Image>().SetMaterialDirty(); // Marcar material como sucio para actualizar
            yield return null; // Espera un frame antes de continuar
        }

        // Asegúrate de que el panel esté completamente disuelto
        currentDissolveAmount = 1f;
        GetComponent<Image>().SetMaterialDirty(); // Actualiza el material
        gameObject.SetActive(false);
    }

    // Método para iniciar la solidificación
    public void StartSolidify()
    {
        StartCoroutine(SolidifyCoroutine());
        AudioManager.Instance.PlaySfx("Panel_show");

    }

    // Corrutina que maneja la animación de solidificación
    private IEnumerator SolidifyCoroutine()
    {
        currentDissolveAmount = 1;

        float timer = 0f;
        float initialAmount = 1f; // Valor inicial de disolución

        // Animar de 1 a 0 usando la curva de solidificación
        while (timer < solidifyDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / solidifyDuration; // Normaliza el tiempo
            float curveValue = solidifyCurve.Evaluate(t); // Evalúa la curva
            currentDissolveAmount = Mathf.Lerp(initialAmount, 0f, curveValue); // Interpola entre 1 y 0
            GetComponent<Image>().SetMaterialDirty(); // Marcar material como sucio para actualizar
            yield return null; // Espera un frame antes de continuar
        }

        // Asegúrate de que el panel esté completamente sólido
        currentDissolveAmount = 0f;
        GetComponent<Image>().SetMaterialDirty(); // Actualiza el material
    }
}
