using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piezas : MonoBehaviour
{
    [SerializeField] private enum PieceType { P1, P2, P3, P4, }
    [SerializeField] private PieceType pieceType;
    [SerializeField] private RectTransform UIPiece;
    [SerializeField] private AnimationCurve animationItemScaleCurve, animationItemPositionCurve;
    private Vector3 targetPosition;
    private Vector3 endScale;
    private Vector3 startPosition;
    private Coroutine pieceAnimCoroutine;
    public static bool enableP1, enableP2, enableP3, enableP4;

    private void OnEnable()
    {
        SelectType();
    }
    void Awake()
    {
        targetPosition = transform.position;
        endScale = transform.localScale;
        startPosition = GetWorldPositionFromUI(UIPiece);

    }
    public void ShowPiece(float delay)
    {
        if (pieceAnimCoroutine != null)
        {
            StopCoroutine(pieceAnimCoroutine);
        }
        pieceAnimCoroutine = StartCoroutine(PiecedAnim(delay));
    }
    private void LoadTransforms()
    {
        //targetPosition = transform.position;
    }
    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        Vector3 screenPos = uiElement.position;
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, screenPos, Camera.main, out worldPos);
        return worldPos;
    }

    private IEnumerator PiecedAnim(float delay)
    {
        AudioManager.Instance.PlaySfx("Espejo_pieza");
        gameObject.SetActive(true);
        LoadTransforms();
        yield return new WaitForSeconds(delay);
        Vector3 startScale = Vector3.zero;
        // endScale = transform.localScale;

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
        transform.position = targetPosition;

    }
    private void SelectType()
    {
        switch (pieceType)
        {
            case (PieceType.P1):
                if (!enableP1)
                {
                    transform.position = GetWorldPositionFromUI(UIPiece);
                    transform.localScale = Vector3.zero;
                    enableP1 = true;
                }
                break;
            case (PieceType.P2):
                if (!enableP2)
                {
                    transform.position = GetWorldPositionFromUI(UIPiece);
                    transform.localScale = Vector3.zero;
                    enableP2 = true;
                }
                break;
            case (PieceType.P3):
                if (!enableP3)
                {
                    transform.position = GetWorldPositionFromUI(UIPiece);
                    transform.localScale = Vector3.zero;
                    enableP3 = true;
                }
                break;
            case (PieceType.P4):
                if (!enableP4)
                {
                    transform.position = GetWorldPositionFromUI(UIPiece);
                    transform.localScale = Vector3.zero;
                    enableP4 = true;
                }
                break;
        }
    }
}
