using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animation : MonoBehaviour, IMaterialModifier
{
    public float animationDuration = .5f;
    private float timer = 0f;
    private bool _isAnimating = false;

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        baseMaterial.SetFloat("_isAnimate", _isAnimating ? 1 : 0);
        return baseMaterial;
    }

    void OnValidate()
    {
        GetComponent<Image>().SetMaterialDirty();
    }

    void Update()
    {
        if (_isAnimating)
        {
            timer += Time.deltaTime;

            if (timer >= animationDuration)
            {
                _isAnimating = false; // Detener la animación automáticamente
                timer = 0f;
                GetComponent<Image>().SetMaterialDirty();
            }

            GetComponent<Image>().SetMaterialDirty();
        }
    }

    public void StartAnimation()
    {
        _isAnimating = true;
        timer = 0f;
        GetComponent<Image>().SetMaterialDirty();
    }
}
