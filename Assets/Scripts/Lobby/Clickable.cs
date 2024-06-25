using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Dialogue dialogue;

    public void OnPointerDown(PointerEventData eventData)
    {
        dialogue.OnButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dialogue.OnButtonUp();
    }
}
