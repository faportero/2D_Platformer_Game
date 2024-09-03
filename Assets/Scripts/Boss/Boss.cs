using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel, bigBoss;

    private Coroutine bossSolidify, bossDissolve;
    private Material material;
    public bool isFinalBoss;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }
    private void OnEnable()
    {
        material = GetComponent<SpriteRenderer>().material;
        BossSolidify();
        
        if (isFinalBoss)
        {
            //bigBoss.SetActive(false);
            bigBoss.GetComponent<BigBoss>().BossDisolve();
        }

    }
    public void BossSolidify()
    {
        if (bossSolidify != null)
        {
            StopCoroutine(bossSolidify);
        }
        bossSolidify = StartCoroutine(BossSolidifyAnim());
    }
    public void BossDisolve()
    {
        if (bossDissolve != null)
        {
            StopCoroutine(bossDissolve);
        }
        bossDissolve = StartCoroutine(BossDisolveAnim());
    }
    private IEnumerator BossSolidifyAnim()
    {
        AudioManager.Instance.PlaySfx("Solidify");

        float dissolveAmount = 0;
        float duration = 2f;  
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        material.SetFloat("_DissolveAmmount", 0);
        dialoguePanel.SetActive(true);

    }

    private IEnumerator BossDisolveAnim()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = .5f;  
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }
        material.SetFloat("_DissolveAmmount", 1);
        //gameObject.SetActive(false);
        if (!isFinalBoss)
        {
            bigBoss.SetActive(true);
            bigBoss.GetComponent<BigBoss>().BossSolidify();
            dialoguePanel.SetActive(false);
        }
    }
}
