using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSounds : MonoBehaviour
{
    private PlayerMovementNew playerMovementNew;

    private void Awake()
    {
        playerMovementNew = GetComponent<PlayerMovementNew>();
    }
    public void FootStepsLeft()
    {
        AudioManager.Instance.PlaySfx("FootStep_Left");
    }
    public void FootStepsRight()
    {
        AudioManager.Instance.PlaySfx("FootStep_Right");
    }
    public void JumpSound()
    {
       // AudioManager.Instance.PlaySfx("Jump");
    }
    public void RollSound()
    {
        if (playerMovementNew.movementMode == PlayerMovementNew.MovementMode.RunnerMode && !playerMovementNew.isHitBadFloor && playerMovementNew.inputsEnabled && playerMovementNew.canMove) AudioManager.Instance.PlaySfx("Roll");
    }
}
