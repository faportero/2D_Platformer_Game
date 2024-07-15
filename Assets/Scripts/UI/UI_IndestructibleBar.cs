using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_IndestructibleBar : MonoBehaviour
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
    [HideInInspector] public Coroutine updateTimeCoroutine;
    [HideInInspector] public bool startUpdateTimeCoroutine;

    private void Start()
    {
        healthFillBar = transform.GetChild(0).GetComponent<Image>();      
    }

    public void UpdateTime(float duration)
    {

        if (updateTimeCoroutine != null)
        {

            StopCoroutine(updateTimeCoroutine);
            startUpdateTimeCoroutine = false;

        }
        updateTimeCoroutine = StartCoroutine(UpdateTimeEffect(duration));
    }

    private IEnumerator UpdateTimeEffect(float duration)
    {
        //startUpdateTimeCoroutine = true;

        startFillAmount = 1;
        targetFillAmount = currentTimeEffect / maxTimeEffect;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }
        Color newColor = new Color(1, 1, 1, 0);
    }
}
