using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelTutorial;
    [SerializeField] private GameObject PanelDetectInput;
    [SerializeField] private GameObject hand;
     private SwipeDetector canvasSwipe;
    [SerializeField] private GameObject pingPongObject;
    private AudioPause audioPause;
    [SerializeField] private bool isInteractive;
    private PlayerMovementNew playerMovementNew;
    private Animator handAnim;
    [SerializeField] private int interactionIndex;   

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        if((!canvasSwipe)) canvasSwipe = playerMovementNew.swipeDetector;
        audioPause = FindAnyObjectByType<AudioPause>();
        if (!hand) hand = GameObject.FindGameObjectWithTag("Hand");
        handAnim = hand.GetComponent<Animator>();

        if (!TutorialManager.endTutorial)
        {
            playerMovementNew.inputsEnabled = false;
            playerMovementNew.tutorialActive = true;
        }
    }
    public void ActivePingPongOBjectl(bool active)
    {
        pingPongObject.GetComponent<PingPongScaleAnimation>().enabled = active;
    }
    public void StopPingPongOBjectl()
    {
        pingPongObject.GetComponent<PingPongScaleAnimation>().PauseAnimation();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && TutorialManager.showTutorial)
        {
            //panelTutorial.transform.parent.gameObject.SetActive(true);
            //canvasSwipe.gameObject.transform.parent.gameObject.SetActive(false);
            //canvasSwipe.enabled = false;
            //canvasHUD.gameObject.GetComponent<Image>().raycastTarget = false;
            if (pingPongObject != null) ActivePingPongOBjectl(true);
            //playerMovementNew.tutorialActive = true;
            audioPause.Pause(true);
            panelTutorial.SetActive(true);

            if (isInteractive)
            {
               // playerMovementNew.inputsEnabled = true;
                audioPause.Pause(true);
                panelTutorial.SetActive(true);
                if (PanelDetectInput != null) PanelDetectInput.SetActive(true);
                if (interactionIndex == 0)
                {
                    hand.SetActive(true);
                    handAnim.Play("Tap Animation");
                }
                else if (interactionIndex == 1) 
                {
                    hand.SetActive(true);
                    hand.SetActive(true); handAnim.Play("Roll Animation");
                }
                else if (interactionIndex == 2)
                {
                    hand.SetActive(true);
                    hand.SetActive(true); handAnim.Play("FallingTap AnimationDe");
                }
                else if (interactionIndex == 3)
                {
                    hand.SetActive(true);
                    hand.SetActive(true); handAnim.Play("FallingTap AnimationIz");
                }
            }
        }
    }
}
