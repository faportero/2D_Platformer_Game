using UnityEngine;

namespace Character.CheckLimits
{
    public interface CheckLimits
    {
        Vector2 ClampFinalPosition(Vector2 _currentPosition);
    }
}