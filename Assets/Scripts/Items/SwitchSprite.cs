using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] Sprite _newSprite;
    private Image _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<Image>();
    }
    private void Start()
    {
       // _spriteRenderer = GetComponent<Image>();
    }
    public void SwitchNewSprite()
    {
        _spriteRenderer.sprite = _newSprite;
  
    }

}
