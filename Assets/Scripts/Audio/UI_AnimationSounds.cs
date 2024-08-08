using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimationSounds : MonoBehaviour
{
    private PlayerMovementNew playerMovementNew;

    private void Awake()
    {
        playerMovementNew = GetComponent<PlayerMovementNew>();
    }
    public void ButtonNormalSound()
    {
        AudioManager.Instance.PlaySfx("btn_normal");
    }

    public void ButtonViajarSound()
    {
        AudioManager.Instance.PlaySfx("btn_viajar");
    }

    public void ButtonEntendidoSound()
    {
        AudioManager.Instance.PlaySfx("btn_normal");
    }

}
