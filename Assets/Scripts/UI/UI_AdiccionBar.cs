using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_AdiccionBar : MonoBehaviour
{
    public AnimationCurve curve;
    private float maxAdiccion = 100;
    public float currentAdiccion = 0;
    private float fillSpeed = 1.5f;
    [SerializeField] private Image adiccionFillBar;
    private float duration = 3;

    private void Start()
    {
        adiccionFillBar = transform.GetChild(1).GetComponent<Image>();
    }

    public void UpdateAdiccion(float amount)
    {
        currentAdiccion += amount;
        currentAdiccion = Mathf.Clamp(currentAdiccion, 0f, maxAdiccion);
        UpdateAdiccionBar();
    }

    private void UpdateAdiccionBar()
    {
        float targetFillAmount = currentAdiccion / maxAdiccion;
        adiccionFillBar.fillAmount = targetFillAmount;
        //StartCoroutine(LerpValue(0, 1));
       // adiccionFillBar.fillAmount = Mathf.Lerp(adiccionFillBar.fillAmount, targetFillAmount, curve.Evaluate(currentAdiccion * Time.deltaTime));
        adiccionFillBar.DOFillAmount(targetFillAmount, fillSpeed);
        //adiccionFillBar.fillAmount = curve.Evaluate(targetFillAmount * Time.deltaTime);
    }

    IEnumerator LerpValue(float start, float end)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t= curve.Evaluate(t);

            adiccionFillBar.fillAmount = Mathf.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }
}
