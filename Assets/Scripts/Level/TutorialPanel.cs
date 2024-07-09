using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelTutorial;
    [SerializeField] private GameObject PanelDetectInput;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject canvasHUD;
    [SerializeField] private GameObject pingPongObject;
    private AudioPause audioPause;
    [SerializeField] private bool isInteractive;
    private PlayerMovementNew playerMovementNew;
    private Animator handAnim;
    [SerializeField] private int interactionIndex;   

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        audioPause = FindAnyObjectByType<AudioPause>();
        //hand = GameObject.FindGameObjectWithTag("Hand");
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
            if (pingPongObject != null) ActivePingPongOBjectl(true);
            canvasHUD.GetComponent<SwipeDetector>().enabled = false;
            canvasHUD.GetComponent<Image>().raycastTarget = false;
            //playerMovementNew.tutorialActive = true;
            audioPause.Pause(true);
            panelTutorial.SetActive(true);

            if (isInteractive)
            {
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
                    hand.SetActive(true); handAnim.Play("FallingTap Animation");
                }
            }
        }
    }
}
