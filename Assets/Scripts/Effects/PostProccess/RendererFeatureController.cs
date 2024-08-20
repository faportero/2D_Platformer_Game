using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RendererFeatureController : MonoBehaviour
{
    public ScriptableRendererData rendererData; // Asigna el ScriptableRendererData desde el Inspector.
    public string featureName; // Nombre del renderer feature que deseas activar/desactivar.

    private ScriptableRendererFeature feature;
    private bool isFeatureEnabled;

    [SerializeField] private bool enableFeature; // Variable para activar o desactivar la característica desde el Inspector.

    private void Start()
    {
        // Busca la característica en el rendererData.
        foreach (var f in rendererData.rendererFeatures)
        {
            if (f.GetType().Name == featureName)
            {
                feature = f;
                isFeatureEnabled = feature.isActive;
                break;
            }
        }

        if (feature == null)
        {
            Debug.LogError("Feature not found: " + featureName);
        }

        // Inicializa el estado de la característica basado en la variable enableFeature.
        ToggleFeature(enableFeature);
    }

    public void ToggleFeature(bool enable)
    {
        if (feature == null) return;

        if (enable)
        {
            if (!isFeatureEnabled)
            {
                rendererData.rendererFeatures.Add(feature);
                isFeatureEnabled = true;
                print("postprocces trueee");
            }
        }
        else
        {
            if (isFeatureEnabled)
            {
                rendererData.rendererFeatures.Remove(feature);
                isFeatureEnabled = false;
            }
        }
    }

    private void Update()
    {
        // Actualiza la característica si el valor de enableFeature cambia en el Inspector.
        if (feature != null && (enableFeature != isFeatureEnabled))
        {
            ToggleFeature(enableFeature);
        }
    }
}
