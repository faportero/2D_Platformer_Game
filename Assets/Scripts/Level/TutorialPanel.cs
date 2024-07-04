using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelTutorial;
    [SerializeField] private GameObject PanelDetectInput;
    [SerializeField] private GameObject hand;
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

        playerMovementNew.inputsEnabled = false;
        playerMovementNew.tutorialActive = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
            playerMovementNew.tutorialActive = true;
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
            }
        }
    }
}
