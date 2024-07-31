using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Dialogue dialogue;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dialogue.gameObject.activeSelf)
        {
            if(gameObject.name == "ButtonNext")
            {
                 dialogue.OnButtonDown();
            }
            else if (gameObject.name == "ButtonBack")
            {
                 dialogue.OnBackButtonDown();
            }
            else if (gameObject.name == "ButtonCambiar")
            {
                dialogue.OnChangeButtonDown();
                gameObject.SetActive(false);
            }
        }
    }


}
