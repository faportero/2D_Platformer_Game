using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject panelPause;
    private PlayerMovementNew playerMovementNew;
    private SwipeDetector swipeDetector;
    [SerializeField] private bool isTutorialJump;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject canvasHUD;
 private  AudioPause audioPause;
    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        audioPause = FindAnyObjectByType<AudioPause>();
        swipeDetector = playerMovementNew.swipeDetector;
    }
    public void ActiveTutorial(bool pause)
    {
        playerMovementNew.tutorialActive = pause;
    }

    public void ActiveRayCastHUD(bool pause)
    {
        canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = pause;
    }


    private PointerEventData CreatePointerEventData(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        return eventData;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isTutorialJump)
        {
            if (playerMovementNew.isPC) 
            {
                swipeDetector.OnPointerUp(CreatePointerEventData(Input.mousePosition));
            }
            else
            {
                swipeDetector.OnPointerUp(CreatePointerEventData(Input.GetTouch(0).position));
            }
        }
        else
        {
            if (playerMovementNew.isPC)
            {
                swipeDetector.OnPointerUp(CreatePointerEventData(Input.mousePosition));
            }
            else
            {
                swipeDetector.OnPointerUp(CreatePointerEventData(Input.GetTouch(0).position));

            }

        }
        if (playerMovementNew.isPC)
        {
             if (Input.GetMouseButtonDown(1))
           // if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)

            {
                audioPause.Pause(false);
                playerMovementNew.tutorialActive = true;
                playerMovementNew.inputsEnabled = true;
                gameObject.transform.parent.gameObject.SetActive(false);
                hand.SetActive(false);
                playerMovementNew.DoRoll();
                canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            }
        }
        else
        {
            if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)
            {
                audioPause.Pause(false);
                playerMovementNew.tutorialActive = true;
                playerMovementNew.inputsEnabled = true;
                gameObject.transform.parent.gameObject.SetActive(false);
                hand.SetActive(false);
                playerMovementNew.DoRoll();
                canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTutorialJump)
        {
            if (playerMovementNew.isPC)
            {
                swipeDetector.OnPointerDown(CreatePointerEventData(Input.mousePosition));
            }
            else
            {
                swipeDetector.OnPointerDown(CreatePointerEventData(Input.GetTouch(0).position));
            }
        }
        else
        {
            if (playerMovementNew.isPC)
            {
                swipeDetector.OnPointerDown(CreatePointerEventData(Input.mousePosition));
            }
            else
            {
                swipeDetector.OnPointerDown(CreatePointerEventData(Input.GetTouch(0).position));
            }
            
        }

        if (playerMovementNew.isPC)
        {
            if (Input.GetMouseButtonDown(1))
            // if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)

            {
                audioPause.Pause(false);
                playerMovementNew.tutorialActive = true;
                playerMovementNew.inputsEnabled = true;
                gameObject.transform.parent.gameObject.SetActive(false);
                hand.SetActive(false);
                playerMovementNew.DoRoll();
                canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            }
        }
        else
        {
            if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)
            {
                audioPause.Pause(false);
                playerMovementNew.tutorialActive = true;
                playerMovementNew.inputsEnabled = true;
                gameObject.transform.parent.gameObject.SetActive(false);
                hand.SetActive(false);
                playerMovementNew.DoRoll();
                canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (playerMovementNew.isPC)
        //{
        //    swipeDetector.OnPointerUp(CreatePointerEventData(Input.mousePosition));
        //}
        //else
        //{
        //    swipeDetector.OnPointerUp(CreatePointerEventData(Input.GetTouch(0).position));
        //}
    }
}
