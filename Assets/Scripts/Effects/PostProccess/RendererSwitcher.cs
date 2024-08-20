using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RendererSwitcher : MonoBehaviour
{
    public Camera YourCamera; // Asigna la cámara desde el Inspector o en el código
    public int rendererIndex; // Índice del renderer al que deseas cambiar

    void Start()
    {
        SwitchRenderer(YourCamera, rendererIndex);
    }

    public void SwitchRenderer(Camera camera, int index)
    {
        // Verificar si la cámara y el índice son válidos
        if (camera == null)
        {
            Debug.LogError("La cámara no está asignada.");
            return;
        }

        var _camData = camera.GetUniversalAdditionalCameraData();

        if (_camData == null)
        {
            Debug.LogError("La cámara no tiene datos adicionales de URP.");
            return;
        }

        // Obtener el UniversalRenderPipelineAsset activo
        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        if (urpAsset == null)
        {
            Debug.LogError("No se encuentra el UniversalRenderPipelineAsset activo.");
            return;
        }

        var renderers = urpAsset.renderers;

        // Verificar si el índice está dentro del rango válido
        if (index < 0 || index >= renderers.Length)
        {
            Debug.LogError("Índice de renderer fuera de rango.");
            return;
        }

        // Cambiar el renderer
        _camData.SetRenderer(index);

        // Opcional: forzar una actualización de la cámara si es necesario
        camera.targetTexture = null; // Asegúrate de que no esté asignada una textura objetivo.
    }
}
