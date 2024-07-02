using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piezas : MonoBehaviour
{
    public enum PieceType
    {
        A,
        B,
        C,
        D,
    }
    public Transform uiPosition;
    public PieceType pieceType;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    [SerializeField] private AnimationCurve animationItemScaleCurve, animationItemPositionCurve;

    // Start is called before the first frame update
    private void OnEnable()
    {
        SelectType();
        StartCoroutine(PiecedAnim());
    }

    // Update is called once per frame
    void Update()
    {
        SelectType();
    }
    public void SelectType()
    {

        //sustanceType = SustanceType.Cannabis;
        switch (pieceType)
        {
            case PieceType.A:
                startPosition = uiPosition.position;
                targetPosition = transform.position;
                break;

            case PieceType.B:
                startPosition = uiPosition.position;
                targetPosition = transform.position;

                break;
            case PieceType.C:
                startPosition = uiPosition.position;
                targetPosition = transform.position;

                break;
            case PieceType.D:
                startPosition = uiPosition.position;
                targetPosition = transform.position;

                break;
              

        }
    }
    private IEnumerator PiecedAnim()
    {
        
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = transform.localScale;

        float elapsedTime = 0;
        float duration = 0.5f;
        float duration2 = .5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float t1 = elapsedTime / duration2;
            float curvePosValue = animationItemPositionCurve.Evaluate(t);
            float curveScaleValue = animationItemScaleCurve.Evaluate(t1);
            transform.position = Vector3.Lerp(startPosition, targetPosition, curvePosValue);
            transform.localScale = Vector3.Lerp(startScale, endScale, curveScaleValue);
            yield return null;
        }

        // Asegurarse de que el valor final sea el targetPosition
        transform.position = targetPosition;

    }
}
