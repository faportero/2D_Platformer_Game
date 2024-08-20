using UnityEngine;

public class ToggleFullScreenPass : MonoBehaviour
{
    public CustomRenderer2DData customRenderer2DData; // Asigna el CustomRenderer2DData en el Inspector
    public bool enableFullScreenPass = true; // Estado deseado del pase de renderizado completo

    private FullScreenRenderPassFeature fullScreenPassFeature;
    private bool isFullScreenPassEnabled;

    void Start()
    {
        if (customRenderer2DData == null)
        {
            Debug.LogError("CustomRenderer2DData is not assigned.");
            return;
        }

        // Encuentra el pase de renderizado completo en el CustomRenderer2DData
        fullScreenPassFeature = customRenderer2DData.rendererFeatures
            .Find(feature => feature is FullScreenRenderPassFeature) as FullScreenRenderPassFeature;

        if (fullScreenPassFeature == null)
        {
            Debug.LogError("FullScreenRenderPassFeature not found in Renderer2DData.");
            return;
        }

        // Inicialmente, activa o desactiva el pase de renderizado completo
        isFullScreenPassEnabled = enableFullScreenPass;
        ToggleFullScreenPassProperty(isFullScreenPassEnabled);
    }

    void Update()
    {
        // Verifica si el valor del booleano ha cambiado
        if (fullScreenPassFeature != null)
        {
            if (enableFullScreenPass != isFullScreenPassEnabled)
            {
                ToggleFullScreenPassProperty(enableFullScreenPass);
                isFullScreenPassEnabled = enableFullScreenPass;
            }
        }
    }

    public void ToggleFullScreenPassProperty(bool enable)
    {
        if (fullScreenPassFeature != null)
        {
            // Activa o desactiva el pase de renderizado completo
            fullScreenPassFeature.SetEnabled(enable);
            isFullScreenPassEnabled = enable; // Actualiza el estado actual
        }
    }
}
