using UnityEngine;

namespace InputFolder
{
    public interface InputInterface
    {
        Vector2 GetDirection(bool isCollision);
        Vector3 GetRotation();
        bool IsGasActionPressed();
        bool CanGasActionPress();
    }
}