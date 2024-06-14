using UnityEngine;

public class UI_Progress : MonoBehaviour
{
    public RectTransform indicator;
    public RectTransform startPoint; // Punto inicial en la HUD
    public RectTransform endPoint; // Punto final en la HUD
    public DistanceTracker distanceTracker;

    void Update()
    {
        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        if (distanceTracker == null || indicator == null || startPoint == null || endPoint == null)
            return;

        float progress = distanceTracker.playerProgress;

        // Interpolar la posición del indicador entre startPoint y endPoint
        indicator.anchoredPosition = Vector2.Lerp(startPoint.anchoredPosition, endPoint.anchoredPosition, progress);
    }
}
