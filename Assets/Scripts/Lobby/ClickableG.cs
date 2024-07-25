using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class ClickableG : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private DialogueGargolas dialogue;
    [SerializeField] private Espejo espejo;
    

    private void Start()
    {
        espejo.videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        //espejo.SwitchPlayerTransform();
        //dialogue.OnButtonDown();
        // espejo.playerMovement.transform.localScale = new Vector3(espejo.playerMovement.transform.localScale.x, espejo.playerMovement.transform.localScale.y, espejo.playerMovement.transform.localScale.z);
        espejo.SwitchPlayerTransform(true);
        espejo.panelDialogueGargolas.GetComponent<Animator>().enabled = true;
        espejo.panelDialogueGargolas.GetComponent<Animator>().Play("Show Animation");

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (dialogue.gameObject.activeSelf)
        {
            //espejo.SwitchPlayerTransform();
            //espejo.countVideoClips++;
            StartCoroutine(ChangeVideoCameraandPlayVideo());
        }
    }

    private IEnumerator ChangeVideoCameraandPlayVideo()
    {
        CameraManager.instance.SingleSwapCamera(espejo.cameraVideo, 1f);
        espejo.SwitchPlayerTransform(false);
      //  espejo.playerMovement.transform.localScale = new Vector3(-espejo.playerMovement.transform.localScale.x, espejo.playerMovement.transform.localScale.y, espejo.playerMovement.transform.localScale.z);
        yield return new WaitForSeconds(.1f);
        espejo.videoPlayer.clip = espejo.videoClips[espejo.countVideoClips];
        espejo.videoPlayer.Play();
        gameObject.SetActive(false);
        espejo.panelDialogueGargolas.GetComponent<Animator>().Play("Hide Animation");

    }
}
