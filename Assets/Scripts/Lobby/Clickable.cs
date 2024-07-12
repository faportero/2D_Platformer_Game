using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Dialogue dialogue;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dialogue.gameObject.activeSelf)
        {
            dialogue.OnButtonDown();
           // dialogue.StartBlinkAnimation();
        }
    }


}
