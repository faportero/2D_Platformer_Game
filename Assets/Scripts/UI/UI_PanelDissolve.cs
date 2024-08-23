using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelDissolve : UI_Animation, IMaterialModifier
{
    public float dissolveDuration = .5f; // Duración para disolución
    public AnimationCurve dissolveCurve; // Curva para la disolución
    public float solidifyDuration = .5f;  // Duración para solidificación
    public AnimationCurve solidifyCurve;  // Curva para la solidificación
    private LevelManager levelManager;
    private float currentDissolveAmount = 0f;
    public bool isWorldPanel;

    private List<Material> materialsToAnimate = new List<Material>();
    private List<TextMeshPro> worldPanelTexts = new List<TextMeshPro>();
    private List<TextMeshProUGUI> uiTexts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        if (isWorldPanel)
        {
            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in spriteRenderers)
            {
                if (renderer.material.HasProperty("_DissolveAmmount"))
                {
                    materialsToAnimate.Add(renderer.material);
                }
            }

            worldPanelTexts.AddRange(GetComponentsInChildren<TextMeshPro>());
        }
        else
        {
            var images = GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                if (image.material.HasProperty("_DissolveAmmount"))
                {
                    materialsToAnimate.Add(image.material);
                }
            }

            uiTexts.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
        }
    }

    private void OnEnable()
    {
        StartSolidify(); // Inicia la solidificación al activar el panel
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (baseMaterial.HasProperty("_DissolveAmmount"))
        {
            baseMaterial.SetFloat("_DissolveAmmount", currentDissolveAmount);
        }
        return baseMaterial;
    }

    public void StartDissolve()
    {
        StartCoroutine(DissolveCoroutine());
        AudioManager.Instance.PlaySfx("Panel_hide");
    }

    private IEnumerator DissolveCoroutine()
    {
        float timer = 0f;
        float initialAmount = 0f; // Valor inicial de disolución

        currentDissolveAmount = 0f;

        while (timer < dissolveDuration)
        {
            timer += isWorldPanel ? Time.deltaTime : Time.unscaledDeltaTime;

            float t = timer / dissolveDuration;
            float curveValue = dissolveCurve.Evaluate(t);
            currentDissolveAmount = Mathf.Lerp(initialAmount, 1f, curveValue);

            foreach (var material in materialsToAnimate)
            {
                material.SetFloat("_DissolveAmmount", currentDissolveAmount);
            }

            if (isWorldPanel)
            {
                foreach (var text in worldPanelTexts)
                {
                    var color = text.color;
                    color.a = Mathf.Lerp(1f, 0f, curveValue);
                    text.color = color;
                }
            }
            else
            {
                foreach (var text in uiTexts)
                {
                    var color = text.color;
                    color.a = Mathf.Lerp(1f, 0f, curveValue);
                    text.color = color;
                }
            }

            yield return null;
        }

        yield return new WaitForSecondsRealtime(.2f);

        currentDissolveAmount = 1f;

        foreach (var material in materialsToAnimate)
        {
            material.SetFloat("_DissolveAmmount", currentDissolveAmount);
        }

        if (isWorldPanel)
        {
            foreach (var text in worldPanelTexts)
            {
                var color = text.color;
                color.a = 0f;
                text.color = color;
            }
        }
        else
        {
            foreach (var text in uiTexts)
            {
                var color = text.color;
                color.a = 0f;
                text.color = color;
            }
        }

        if (levelManager.currentScene == LevelManager.CurrentScene.Nivel1 ||
            levelManager.currentScene == LevelManager.CurrentScene.Nivel2 ||
            levelManager.currentScene == LevelManager.CurrentScene.Nivel3)
        {
            if (!isWorldPanel) gameObject.SetActive(false);
        }
    }

    public void StartSolidify()
    {
        StartCoroutine(SolidifyCoroutine());
        AudioManager.Instance.PlaySfx("Panel_show");
    }

    private IEnumerator SolidifyCoroutine()
    {
        float timer = 0f;
        float initialAmount = 1f;

        currentDissolveAmount = 1f;

        while (timer < solidifyDuration)
        {
            timer += isWorldPanel ? Time.deltaTime : Time.unscaledDeltaTime;

            float t = timer / solidifyDuration;
            float curveValue = solidifyCurve.Evaluate(t);
            currentDissolveAmount = Mathf.Lerp(initialAmount, 0f, curveValue);

            foreach (var material in materialsToAnimate)
            {
                material.SetFloat("_DissolveAmmount", currentDissolveAmount);
            }

            if (isWorldPanel)
            {
                foreach (var text in worldPanelTexts)
                {
                    var color = text.color;
                    color.a = Mathf.Lerp(0f, 1f, curveValue);
                    text.color = color;
                }
            }
            else
            {
                foreach (var text in uiTexts)
                {
                    var color = text.color;
                    color.a = Mathf.Lerp(0f, 1f, curveValue);
                    text.color = color;
                }
            }

            yield return null;
        }

        currentDissolveAmount = 0f;

        foreach (var material in materialsToAnimate)
        {
            material.SetFloat("_DissolveAmmount", currentDissolveAmount);
        }

        if (isWorldPanel)
        {
            foreach (var text in worldPanelTexts)
            {
                var color = text.color;
                color.a = 1f;
                text.color = color;
            }
        }
        else
        {
            foreach (var text in uiTexts)
            {
                var color = text.color;
                color.a = 1f;
                text.color = color;
            }
        }
    }
}
