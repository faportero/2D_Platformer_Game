using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingScene : MonoBehaviour
{
    private Animator anim;
   

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.enabled = true;
        ////anim.Play("FogTransitionEnd");
        ////print(transform.GetChild(0).gameObject.name);
        //StartCoroutine(Openner());
        //if (LevelManager.isFogTransition)
        //{
        //   // StartCoroutine(Openner());
        //   // LevelManager.isFogTransition = false;
        //}

    }
    public void ShowOppener()
    {
        StopAllCoroutines();
        StartCoroutine(Openner());
    }
    private IEnumerator Openner()
    {       
        anim.Play("FogTransitionEnd");
        AnimationClip animacion = anim.runtimeAnimatorController.animationClips[1];
        print(animacion.name);
        yield return new WaitForSeconds(animacion.averageDuration);
       // yield return new WaitForSeconds(5);
        anim.enabled = false;
        gameObject.SetActive(false);
    }
}
