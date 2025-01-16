using Character;
using Character.ExtinguisherGas.Extinguisher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class HudCharacter : MonoBehaviour
    {
        private ExtinguisherController _extinguisherController;
        [SerializeField] private SpriteRenderer _imageExtinguisherHUD;
        [SerializeField] private Color[] _colors;

        // Start is called before the first frame update
        void Start()
        {
            _extinguisherController = transform.parent.parent.GetComponent<ExtinguisherController>();
            _imageExtinguisherHUD = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        }
        // Update is called once per frame
        void Update()
        {
            if (_extinguisherController.GetValueId() == 0)
            {
                if(_imageExtinguisherHUD.gameObject.activeSelf) _imageExtinguisherHUD.gameObject.SetActive(false);
            }
            else
            {
                if (!_imageExtinguisherHUD.gameObject.activeSelf) _imageExtinguisherHUD.gameObject.SetActive(true);
                if(_imageExtinguisherHUD.color != _colors[_extinguisherController.GetValueId() - 1]) _imageExtinguisherHUD.color = _colors[_extinguisherController.GetValueId() - 1];
            }
        }
    }
}