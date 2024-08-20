using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "CustomRenderer2D", menuName = "Rendering/Custom Renderer2D")]
public class CustomRenderer2DData : Renderer2DData
{
    public Material postProcessMaterial;
    private FullScreenRenderPassFeature fullScreenPassFeature;

    protected override void OnEnable()
    {
        base.OnEnable();

        // Verifica si ya se ha agregado el FullScreenRenderPassFeature
        fullScreenPassFeature = rendererFeatures.Find(feature => feature is FullScreenRenderPassFeature) as FullScreenRenderPassFeature;

        if (fullScreenPassFeature == null)
        {
            // Crear y añadir el pase de renderizado completo si no existe
            fullScreenPassFeature = new FullScreenRenderPassFeature
            {
                postProcessMaterial = postProcessMaterial
            };
            rendererFeatures.Add(fullScreenPassFeature);
        }
    }
}
