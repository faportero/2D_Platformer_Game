using UnityEngine;

namespace Character.CheckLimits
{
    public class ViewPortCheckLimits : CheckLimits
    {
        private readonly Camera _camera;
        public ViewPortCheckLimits(Camera camera)
        {
            _camera = camera;
        }
        public Vector2 ClampFinalPosition(Vector2 currentPosition)
        {
            var viewportPoint = _camera.WorldToViewportPoint(currentPosition);
            viewportPoint.x = Mathf.Clamp(viewportPoint.x, .03f, .97f);
            viewportPoint.y = Mathf.Clamp(viewportPoint.y, .03f, .97f);
            return _camera.ViewportToWorldPoint(viewportPoint);
        }
    }
}