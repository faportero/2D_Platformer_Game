using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject panelPause;
    private PlayerMovementNew playerMovementNew;
    private SwipeDetector swipeDetector;
    [SerializeField] private bool isTutorialJump;
    [SerializeField] private GameObject hand;
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
        if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)
        {
            audioPause.Pause(false);
            hand.SetActive(false);
            gameObject.transform.parent.gameObject.SetActive(false);
            playerMovementNew.DoRoll();
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
