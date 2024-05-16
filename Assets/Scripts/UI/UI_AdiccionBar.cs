using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_AdiccionBar : MonoBehaviour
{
    public AnimationCurve curve;
    private float maxAdiccion = 1;
    public float currentAdiccion = 0;
    private float fillSpeed = 0.5f;
    [SerializeField] private Image adiccionFillBar;

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
        //adiccionFillBar.fillAmount = targetFillAmount;
        adiccionFillBar.fillAmount = Mathf.Lerp(adiccionFillBar.fillAmount, targetFillAmount, .25f);
        //adiccionFillBar.DOFillAmount(targetFillAmount, fillSpeed);
    }

}
