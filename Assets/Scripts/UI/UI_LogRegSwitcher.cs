using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LogRegSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject panelLogin, panelRegistro;
    [SerializeField] private RectTransform bgImage;
    [SerializeField] private Slider sliderPanelSwitcher;
    [SerializeField] private TextMeshProUGUI loginText, registroText;
    [SerializeField] private float bgMoveDistance = 100f;
    [SerializeField] private float animationDuration = 0.5f;

    private Coroutine animationCoroutine;
    private bool isSliderChangedByUser = false; // Nuevo flag para detectar interacción del usuario.

    private void Start()
    {
        sliderPanelSwitcher.onValueChanged.AddListener(OnUserChangedSlider);
        OnUserChangedSlider(sliderPanelSwitcher.value); // Inicializar estado.
    }

    private void OnUserChangedSlider(float value)
    {
        if (!isSliderChangedByUser) // Ejecutar solo si es la primera interacción.
        {
            isSliderChangedByUser = true; // Marcamos que el usuario ya interactuó.
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimatePanelsAndBackground(value, animationDuration));
        AudioManager.Instance.PlaySfx("Item_info");
    }

    private IEnumerator AnimatePanelsAndBackground(float targetValue, float duration)
    {
        float elapsedTime = 0f;

        // Escala inicial y objetivo.
        Vector3 loginInitialScale = panelLogin.transform.localScale;
        Vector3 registroInitialScale = panelRegistro.transform.localScale;

        // Rotación inicial y objetivo (efecto 3D).
        Quaternion loginInitialRotation = panelLogin.transform.rotation;
        Quaternion registroInitialRotation = panelRegistro.transform.rotation;

        Quaternion loginTargetRotation = Quaternion.Euler(0, targetValue * 180f, 0);
        Quaternion registroTargetRotation = Quaternion.Euler(0, (1 - targetValue) * 180f, 0);

        // Colores iniciales y objetivo.
        Color loginInitialColor = loginText.color;
        Color registroInitialColor = registroText.color;
        Color activeColor = new Color(0.39f, 0.92f, 1f, 1f); // Celeste.
        Color inactiveColor = new Color(0.7f, 0.7f, 0.7f, 1f); // Gris desactivado.

        // Posición inicial y objetivo del fondo.
        Vector3 initialBgPosition = bgImage.anchoredPosition;
        Vector3 targetBgPosition = new Vector3(targetValue * bgMoveDistance, initialBgPosition.y, initialBgPosition.z);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Animar paneles.
            panelLogin.transform.localScale = Vector3.Lerp(loginInitialScale, new Vector3(1, 1, 1), t);
            panelRegistro.transform.localScale = Vector3.Lerp(registroInitialScale, new Vector3(1, 1, 1), t);

            panelLogin.transform.rotation = Quaternion.Lerp(loginInitialRotation, loginTargetRotation, t);
            panelRegistro.transform.rotation = Quaternion.Lerp(registroInitialRotation, registroTargetRotation, t);

            // Determinar color de los textos según el panel activo.
            loginText.color = Color.Lerp(loginInitialColor, targetValue < 0.5f ? activeColor : inactiveColor, t);
            registroText.color = Color.Lerp(registroInitialColor, targetValue >= 0.5f ? activeColor : inactiveColor, t);

            // Animar fondo.
            bgImage.anchoredPosition = Vector3.Lerp(initialBgPosition, targetBgPosition, t);

            // Cambiar visibilidad de paneles según rotación.
            bool isLoginVisible = Mathf.Abs(panelLogin.transform.rotation.y) < 0.7f;
            bool isRegistroVisible = Mathf.Abs(panelRegistro.transform.rotation.y) < 0.7f;

            panelLogin.SetActive(isLoginVisible);
            panelRegistro.SetActive(isRegistroVisible);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar valores finales.
        panelLogin.transform.rotation = loginTargetRotation;
        panelRegistro.transform.rotation = registroTargetRotation;
        bgImage.anchoredPosition = targetBgPosition;

        // Asignar colores finales según el estado del slider.
        loginText.color = targetValue < 0.5f ? activeColor : inactiveColor;
        registroText.color = targetValue >= 0.5f ? activeColor : inactiveColor;
    }
}
