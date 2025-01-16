using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputFolder
{
    public class LockedSliderOnUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public bool IsLockedExtinguisherPressed { get; private set; }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsLockedExtinguisherPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsLockedExtinguisherPressed = false;
        }

    }
}