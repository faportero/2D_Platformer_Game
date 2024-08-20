using UnityEngine;
using UnityEngine.UI; // Necesario para SetMaterialDirty, aunque no se usa directamente en postproceso

public class PostProcessIntensityModifier : MonoBehaviour, IMaterialModifier
{
    public Material postProcessMaterial; // El material de postproceso que quieres modificar
    public bool isIntensityActive = false; // Propiedad para activar/desactivar _Intensity

    // Implementación de IMaterialModifier
    public Material GetModifiedMaterial(Material baseMaterial)
    {
        // Modifica el parámetro _Intensity en el material base
        baseMaterial.SetFloat("_Intensity", isIntensityActive ? 1.0f : 0.0f);
        return baseMaterial;
    }

    void OnValidate()
    {
        // Llamar a SetMaterialDirty no es necesario aquí para postprocesos
        // Pero aquí está para mantener el esquema
        // Esto solo es relevante si se tiene un componente Image
        // GetComponent<Image>().SetMaterialDirty();
    }

    void Start()
    {
        // Llama a UpdateMaterial para asegurarse de que el material esté correcto al inicio
        UpdateMaterial();
    }

    public void ToggleIntensity(bool activate)
    {
        isIntensityActive = activate;
        UpdateMaterial(); // Actualiza el material inmediatamente
    }

    private void UpdateMaterial()
    {
        if (postProcessMaterial != null)
        {
            // Modifica el material usando GetModifiedMaterial
            GetModifiedMaterial(postProcessMaterial);

            // En el contexto de UI, SetMaterialDirty se usa para actualizar la vista del material
            // Para postprocesos, simplemente asegúrate de que el material esté actualizado.
            // Notarás que SetMaterialDirty no tiene un efecto directo en materiales de postprocesos.
        }
        else
        {
            Debug.LogWarning("Material de postproceso no asignado en " + gameObject.name);
        }
    }
}
