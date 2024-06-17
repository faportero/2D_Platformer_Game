using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UI_Lifes : MonoBehaviour
{
    
    [HideInInspector] public List<GameObject> lifes = new List<GameObject>();
    [SerializeField] private UI_Habilidades uiHabilidades;
    private PlayerController playerController;
    private PlayerMovementNew playerMovement;
    private Coroutine heartbBeatCoroutine = null;
    private Animator anim;
    public int lifesCount;
    private int auxLifes;
    private void Start()
    {
        
        playerController = FindAnyObjectByType<PlayerController>();
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();

        lifes.Clear();
        
        if (playerController != null && playerController.vidaExtra)
        {
            CreateLife();
            playerController.vidaExtra = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            lifes.Add(transform.GetChild(i).gameObject);
            anim = transform.GetChild(i).GetComponent<Animator>();
            anim.enabled = false;
        }

        lifesCount = lifes.Count;
    }
    public void UpdateLife()
    {
        if (lifesCount > 0)
        {
            GameObject lastChild = lifes[lifes.Count -1];  
            lastChild.GetComponent<Image>().color = Color.black;
            if (heartbBeatCoroutine != null) StopCoroutine(heartbBeatCoroutine);
                
            lastChild.GetComponent<Animator>().enabled = false;
            lifes.Remove(lastChild);
            lifesCount = lifes.Count;
            auxLifes = lifesCount;
            //print(lifesCount);
            //print(lifes.Count);
        }
        //if(lifes.Count == 1) playerMovement.canMove = false;
    }
    IEnumerator EsperaAnim()
    {
        if (heartbBeatCoroutine != null) StopCoroutine(heartbBeatCoroutine);
        anim.enabled = true;
        anim.Play("Heartbeat");
        AnimationClip animacion = anim.runtimeAnimatorController.animationClips[0];      
        yield return new WaitForSecondsRealtime(animacion.averageDuration);
        anim.enabled = false;
    }
    public void CreateLife()
    {
        var parent = GameObject.FindGameObjectWithTag("Lifes");
        if (playerController.vidaExtra)
        {
           
            Instantiate(parent.transform.GetChild(0).gameObject, parent.transform);
        }
        else
        {


            //GameObject lastChild = lifes[auxLifes - 1];
            //lastChild.GetComponent<Image>().color = Color.white;
            for (int i = 0; i < transform.childCount; i++)            {
                
         
                if(transform.GetChild(i).GetComponent<Image>().color != Color.white)
                {
                    var child = transform.GetChild(i);
                    child.GetComponent<Image>().color = Color.white;
                    anim = child.GetComponent<Animator>();
                    heartbBeatCoroutine = StartCoroutine(EsperaAnim());
                    //anim.enabled = true;
                    //anim.Play("Heartbeat");
                    //child.GetComponent<Animator>().enabled = true;
                    //child.GetComponent<Animator>().Play("Heartbeat");
                    
                    lifes.Add(transform.GetChild(i).gameObject);
                    lifesCount = lifes.Count;
                    //lifes.Add(parent.gameObject);
                    return;
                }
            }
            
        }   
        
    }
}
