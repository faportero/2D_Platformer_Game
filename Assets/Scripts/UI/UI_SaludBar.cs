using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaludBar : MonoBehaviour
{
    public AnimationCurve curve;
    private float maxAdiccion = 100;
    public float currentAdiccion;
    private float fillSpeed = 1f;
   public Image healthFillBar;
    [SerializeField] private Gradient colorGradient;
    private float duration = 3;
    private float targetFillAmount;
    private float startFillAmount;


    private void OnValidate()
    {
        healthFillBar = transform.GetChild(1).GetComponent<Image>();

    }
    private void Start()
    {
        healthFillBar = transform.GetChild(1).GetComponent<Image>();
        //UpdateAdiccion(currentAdiccion);
    }

    public void UpdateHealth(float amount)
    {
        currentAdiccion += amount;
        currentAdiccion = Mathf.Clamp(currentAdiccion, 0f, maxAdiccion);
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        //targetFillAmount = currentAdiccion / maxAdiccion;
        //startFillAmount = healthFillBar.fillAmount;
       // healthFillBar.fillAmount = targetFillAmount;
        //StartCoroutine(LerpValue(0, 1));
        // healthFillBar.fillAmount = Mathf.Lerp(healthFillBar.fillAmount, targetFillAmount, curve.Evaluate(currentAdiccion * Time.deltaTime));
        //healthFillBar.DOFillAmount(targetFillAmount, fillSpeed);
        if(isActiveAndEnabled)StartCoroutine(SmoothUpdateHealth());
        healthFillBar.color = colorGradient.Evaluate(targetFillAmount);
    }

    IEnumerator LerpValue(float start, float end)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t= curve.Evaluate(t);

            healthFillBar.fillAmount = Mathf.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }



    private IEnumerator SmoothUpdateHealth()
    {
        //yield return new WaitForSeconds(.5f);
         startFillAmount = healthFillBar.fillAmount; 
         targetFillAmount = currentAdiccion / maxAdiccion;

        float elapsedTime = 0;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }

    }
}
