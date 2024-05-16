using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TouchPhaseDisplay : MonoBehaviour
{
    public TextMeshProUGUI phaseDisplayText;
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = .5f;

    void Update()
    {
        print(phaseDisplayText.text);
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Ended)
            {
                phaseDisplayText.text = theTouch.phase.ToString();  
                timeTouchEnded = Time.time;
            }
            else if (Time.time - timeTouchEnded > displayTime) 
            {
                phaseDisplayText.text = theTouch.phase.ToString();
                timeTouchEnded = Time.time;
            }
        }
        else if (Time.time - timeTouchEnded > displayTime) 
        {
            phaseDisplayText.text = "";
        }

    }
}
