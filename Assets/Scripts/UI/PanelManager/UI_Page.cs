using ClipperLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Page : MonoBehaviour
{
    private AudioSource audioSource;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float AnimationSpeed = 1.0f;
    public bool ExitOnNewPagePush = false;
    [SerializeField]
    private AudioClip entryClip;
    [SerializeField]
    private AudioClip exitClip;
    [SerializeField]
    private EntryMode entryMode = EntryMode.SLIDE;
    [SerializeField]
    private AnimDirection entryDirection = AnimDirection.LEFT;
    [SerializeField]
    private EntryMode exitMode = EntryMode.SLIDE;
    [SerializeField]
    private AnimDirection exitDirection = AnimDirection.LEFT;

    [SerializeField]
    private UnityEvent PrePushAction;
    [SerializeField]
    private UnityEvent PostPushAction;
    [SerializeField]
    private UnityEvent PrePopAction;
    [SerializeField]
    private UnityEvent PostPopAction;


    private Coroutine animationCoroutine;
    private Coroutine audioCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;
        audioSource.enabled = false;
    }

    public void Enter(bool PlayAudio)
    {
        PrePushAction?.Invoke();

        switch(entryMode)
        {
            case EntryMode.SLIDE:
                SlideIn(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomIn(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeIn(PlayAudio);
                break;

        }
    }

    public void Exit(bool PlayAudio)
    {
        PrePopAction?.Invoke();

        switch (exitMode)
        {
            case EntryMode.SLIDE:
                SlideOut(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomOut(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeOut(PlayAudio);
                break;

        }
    }

    private void SlideIn(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.SlideIn(rectTransform, entryDirection, AnimationSpeed, PostPushAction ));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.SlideIn(rectTransform, entryDirection, AnimationSpeed, null ));
        PlayEntryClip(PlayAudio);
    }

    private void SlideOut(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.SlideOut(rectTransform, exitDirection, AnimationSpeed, PostPopAction));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.SlideOut(rectTransform, exitDirection, AnimationSpeed, null));
        PlayEntryClip(PlayAudio);
    }

    private void ZoomIn(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.ZoomIn(rectTransform, AnimationSpeed, PostPushAction));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.ZoomIn(rectTransform, AnimationSpeed, null));
        PlayEntryClip(PlayAudio);
    }

    private void ZoomOut(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.ZoomOut(rectTransform, AnimationSpeed, PostPopAction));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.ZoomOut(rectTransform, AnimationSpeed, null));
        PlayExitClip(PlayAudio);
    }

    private void FadeIn(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.FadeIn(canvasGroup, AnimationSpeed, PostPushAction));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.FadeIn(canvasGroup, AnimationSpeed, null));
        PlayEntryClip(PlayAudio);
    }

    private void FadeOut(bool PlayAudio)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        //animationCoroutine = StartCoroutine(UI_AnimationHelper.FadeOut(canvasGroup, AnimationSpeed, PostPopAction));
        animationCoroutine = StartCoroutine(UI_AnimationHelper.FadeOut(canvasGroup, AnimationSpeed, null));
        PlayExitClip(PlayAudio);
    }

    private void PlayEntryClip(bool PlayAudio)
    {
        if (PlayAudio && entryClip != null && audioSource != null)
        {
            if(audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
            }
            audioCoroutine = StartCoroutine(PlayClip(entryClip));
        }        
    }   
    
    private void PlayExitClip(bool PlayAudio)
    {
        if (PlayAudio && exitClip != null && audioSource != null)
        {
            if(audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
            }
            audioCoroutine = StartCoroutine(PlayClip(exitClip));
        }        
    }

    private IEnumerator PlayClip(AudioClip Clip) 
    { 
        audioSource.enabled = true;

        WaitForSeconds Wait = new WaitForSeconds(Clip.length);

        audioSource.PlayOneShot(Clip);

        yield return Wait;

        audioSource.enabled = false;
        
    }
}
