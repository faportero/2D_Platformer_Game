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
                    collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                    collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //collision.GetComponent<Rigidbody2D>().gravityScale = collision.GetComponent<PlayerMovementNew>().fallingGravity;
                    collision.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -collision.GetComponent<PlayerMovementNew>().fallingVelocity);
                    //collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    //Destroy(collision.GetComponent<Rigidbody2D>());
                    break;
                case PlayerMovementMode.FlappyMode:
                    collision.GetComponent<PlayerMovementNew>().movementMode = MovementMode.FlappyMode;
                    break;
            }

        }

    }
}
