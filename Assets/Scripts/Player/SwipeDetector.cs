using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isSwiping = false;
    //private bool isTap = false;
    //private bool isPressing = false;
    private float pressTime = 0f;
    public float pressThreshold = 0.05f; // Tiempo mínimo de presión para considerarla un "press and hold"

    public float tapThreshold = 10f; // Umbral de distancia para considerar un tap

    // Enumeración para las direcciones del swipe
    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Variable para almacenar la dirección del swipe
    public SwipeDirection swipeDirection;

    // Propiedad para acceder a TapPerformed de lectura y escritura
    public bool TapPerformed;

    // Propiedad para acceder a IsPressing de lectura y escritura
    public bool IsPressing = false;


    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position;
        isSwiping = true;
        TapPerformed = true; // Restablecer el valor de isTap en cada nuevo toque
       // IsPressing = true;
        Invoke("Pressing", .5f);
        pressTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPosition = eventData.position;
        isSwiping = false;
        IsPressing = false;
        TapPerformed = false;

        // Calcular la distancia entre la posición inicial y final
        float swipeDistance = Vector2.Distance(startPosition, endPosition);

        // Calcular la diferencia entre las posiciones en X y Y
        float deltaX = endPosition.x - startPosition.x;
        float deltaY = endPosition.y - startPosition.y;

        // Determinar la dirección del swipe comparando las coordenadas X e Y
        if (swipeDistance > tapThreshold)
        {
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                swipeDirection = (deltaX > 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {

                swipeDirection = (deltaY > 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
        else
        {
            swipeDirection = SwipeDirection.None; // Restablecer la dirección a "None"
            TapPerformed = true; 
            Invoke("ResetTap", 0.5f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TapPerformed = true;
        //Invoke("ResetTap", 0.5f);
    }

    private void Pressing()
    {
        IsPressing = true;
    }
    private void ResetTap()
    {
        //TapPerformed = false;
        IsPressing = false;
    }

    private void Update()
    {
        //Debug.Log("esta presionando" + IsPressing);
        if (IsPressing && Time.time - pressTime > pressThreshold)
        {
            // Si se mantiene presionado el tap durante más tiempo del umbral, hacemos algo aquí
           // Debug.Log(TapPerformed);
            //IsPressing = true;
        }
           
    }
}
