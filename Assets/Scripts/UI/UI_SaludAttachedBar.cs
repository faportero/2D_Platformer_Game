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
    [HideInInspector] public Coroutine updateTimeCoroutine;
    [HideInInspector] public bool startUpdateTimeCoroutine;

    private void Start()
    {
        healthFillBar = transform.GetChild(1).GetComponent<Image>();
        //UpdateTime(2);
    }

    public void UpdateTime(float duration)
    {

        if (updateTimeCoroutine != null)
        {

            StopCoroutine(updateTimeCoroutine);
            startUpdateTimeCoroutine = false;

        }
        if(!startUpdateTimeCoroutine) updateTimeCoroutine = StartCoroutine(UpdateTimeEffect(duration));
        //if(gameObject.activeSelf && transform.parent.gameObject.activeSelf)updateTimeCoroutine = StartCoroutine(UpdateTimeEffect(duration));

    }

    private IEnumerator UpdateTimeEffect(float duration)
    {
        startUpdateTimeCoroutine = true;
        transform.parent.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;

         startFillAmount = 1; 
         targetFillAmount = currentTimeEffect / maxTimeEffect;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
           // print("TiempoCorutina bar: " + elapsedTime + ". Duracion: " + duration);
            yield return null;
            //yield return new WaitForEndOfFrame();
        }
        // transform.parent.gameObject.SetActive(false);
        Color newColor = new Color (1, 1, 1, 0);
        transform.parent.gameObject.GetComponent<SpriteRenderer>().color = newColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = newColor;
        startUpdateTimeCoroutine = false;
    }
}
