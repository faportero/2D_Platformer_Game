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
    private GameObject canvasSwipe;
    private GameObject canvasTutorial;

    private  AudioPause audioPause;
    public static bool showTutorial = true, endTutorial;

    private void Awake()
    {
        canvasTutorial = GameObject.FindGameObjectWithTag("CanvasTutorial");
       // canvasSwipe = GameObject.FindGameObjectWithTag("PanelSwipe");
    }
    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        audioPause = FindAnyObjectByType<AudioPause>();
        swipeDetector = playerMovementNew.swipeDetector;
      //  print(swipeDetector.gameObject.name);
       // print(canvasSwipe.name);
    }
    public void ActiveTutorial(bool pause)
    {
        //playerMovementNew.tutorialActive = pause;
    }
    public void ShowTutorial(bool pause)
    {
        showTutorial = pause;
    }
    public void ShowInmunidadPanel(bool pause)
    {
        PlayerControllerNew.showInmunidadPanel = pause;
    }
    public void ShowPocaVidaPanel(bool pause)
    {
        PlayerControllerNew.showPocaVidePanel = pause;
    }

    public void ActiveRayCastHUD(bool pause)
    {
        StartCoroutine(ActiveRayCastDelay(pause));
    }
    private IEnumerator ActiveRayCastDelay(bool pause)
    {
        yield return new WaitForSeconds(.2f);
        canvasSwipe.GetComponent<SwipeDetector>().enabled = pause;
        canvasSwipe.GetComponent<UnityEngine.UI.Image>().raycastTarget = pause;

    }

    private PointerEventData CreatePointerEventData(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        return eventData;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (showTutorial)
        {

            if (isTutorialJump)
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
                    //gameObject.transform.parent.gameObject.SetActive(false);
                    gameObject.transform.parent.GetComponent<UI_PanelDissolve>().StartDissolve();
                    hand.SetActive(false);
                    playerMovementNew.DoRoll();
                    swipeDetector.gameObject.transform.parent.gameObject.SetActive(true);
                    swipeDetector.gameObject.SetActive(true);
                }
            }
            else
            {
                if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)
                {
                    audioPause.Pause(false);
                    playerMovementNew.tutorialActive = true;
                    playerMovementNew.inputsEnabled = true;
                    //gameObject.transform.parent.gameObject.SetActive(false);
                    gameObject.transform.parent.GetComponent<UI_PanelDissolve>().StartDissolve();
                    hand.SetActive(false);
                    playerMovementNew.DoRoll();
                    swipeDetector.gameObject.transform.parent.gameObject.SetActive(true);
                    swipeDetector.gameObject.SetActive(true);
                    //canvasHUD.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
                }
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (showTutorial)
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
                    //gameObject.transform.parent.gameObject.SetActive(false);
                    gameObject.transform.parent.GetComponent<UI_PanelDissolve>().StartDissolve();

                    hand.SetActive(false);
                    playerMovementNew.DoRoll();
                    playerMovementNew.swipeDetector.gameObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
                }
            }
            else
            {
                if (swipeDetector.swipeDirection == SwipeDetector.SwipeDirection.Down)
                {
                    audioPause.Pause(false);
                    playerMovementNew.tutorialActive = true;
                    playerMovementNew.inputsEnabled = true;
                    //gameObject.transform.parent.gameObject.SetActive(false);
                    gameObject.transform.parent.GetComponent<UI_PanelDissolve>().StartDissolve();

                    hand.SetActive(false);
                    playerMovementNew.DoRoll();
                    playerMovementNew.swipeDetector.gameObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;


                }
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
