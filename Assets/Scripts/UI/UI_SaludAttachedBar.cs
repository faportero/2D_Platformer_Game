using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaludAttachedBar : MonoBehaviour
{
    public AnimationCurve curve;
    public Image healthFillBar;
    [SerializeField] private Gradient colorGradient;
    private float maxTimeEffect = 100;
    public float currentTimeEffect;
    private float fillSpeed = 1f;
    private float duration = 3;
    private float targetFillAmount;
    private float startFillAmount;


    private void Start()
    {
        healthFillBar = transform.GetChild(1).GetComponent<Image>();

    }

    public void UpdateHealth(float amount)
    {
        currentTimeEffect += amount;
        currentTimeEffect = Mathf.Clamp(currentTimeEffect, 0f, maxTimeEffect);
        //UpdateHealthBar();
    }


    public void UpdateTime(float duration)
    {
    if (isActiveAndEnabled)
        {
            StartCoroutine(UpdateTimeEffect(duration));
            
        }       
    }



    private IEnumerator UpdateTimeEffect(float duration)
    {       
         startFillAmount = 1; 
         targetFillAmount = currentTimeEffect / maxTimeEffect;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }
       // healthFillBar.color = colorGradient.Evaluate(targetFillAmount);
    }
}
