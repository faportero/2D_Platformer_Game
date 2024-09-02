using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
public class BigBossTentacles : MonoBehaviour
{
   
    private PlayerMovementNew playerMovementNew;
    private Coroutine bossSolidify, bossDissolve;
    private Material material;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void OnEnable()
    {
        material = GetComponent<SpriteRenderer>().material;
        BossSolidify();
        
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
        gameObject.SetActive(false);
    }    

}
