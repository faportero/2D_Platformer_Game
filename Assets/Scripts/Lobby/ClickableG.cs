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
       
        dialogue.OnButtonDown();

        espejo.playerMovement.transform.localScale = new Vector3(espejo.playerMovement.transform.localScale.x, espejo.playerMovement.transform.localScale.y, espejo.playerMovement.transform.localScale.z);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (dialogue.gameObject.activeSelf)
        {
            //espejo.SwitchPlayerTransform();
            //espejo.countVideoClips++;
            espejo.playerMovement.transform.localScale = new Vector3(-espejo.playerMovement.transform.localScale.x, espejo.playerMovement.transform.localScale.y, espejo.playerMovement.transform.localScale.z);
            espejo. videoPlayer.clip = espejo.videoClips[espejo.countVideoClips];
            espejo.videoPlayer.Play();
        }
    }


}
