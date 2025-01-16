using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class ChangeBackground : MonoBehaviour
    {
        private Image _imageComponent;
        [SerializeField] private Sprite _spritePortrait, _spriteLandscape;
        // Start is called before the first frame update
        void Start()
        {
            _imageComponent = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Screen.width >= Screen.height)
            {
                _imageComponent.sprite = _spriteLandscape;
                return;
            }
            else
                _imageComponent.sprite = _spritePortrait;
        }
    }
}