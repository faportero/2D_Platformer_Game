using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementNew;

public class SwitchModeCollider : MonoBehaviour
{
    public enum PlayerMovementMode
    {
        TapMode,
        RunnerMode,
        FallingMode,
        FlappyMode
    }
    public PlayerMovementMode movementMode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            switch (movementMode)
            {
                case PlayerMovementMode.TapMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.TapMode;                   
                    break;
                case PlayerMovementMode.RunnerMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.RunnerMode;                                      
                    break;
                case PlayerMovementMode.FallingMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FallingMode;                                                         
                    break;
                case PlayerMovementMode.FlappyMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FlappyMode;                                                
                    break;
            }

        }

    }
}
