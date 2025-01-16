using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputFolder
{
    public class GasButtonOnUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private Image button;
        public bool IsPressed { get; set; }
        private void Start()
        {
            button = GetComponent<Image>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (button.raycastTarget)
            {
                IsPressed = true;
                //TapOnScreenInputAdapter._firstGyroZ = 0;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
    }
}