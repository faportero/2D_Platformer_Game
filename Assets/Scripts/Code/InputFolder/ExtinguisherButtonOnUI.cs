using Character;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputFolder
{
    public class ExtinguisherButtonOnUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private CharacterMediator _characterMediator;
        private Image _buttonImage; 
        public bool IsPressed { get; private set; }
        public bool IsLockedExtinguisherPressed { get; private set; }
        public int ExtinguisherActive { get; private set; }
        public int NoExtinguishers { get; private set; }
        private void Start()
        {
            _buttonImage = GetComponent<Image>();
            _buttonImage.sprite = _sprites[0];
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            //TapOnScreenInputAdapter._firstGyroZ = 0;
            NoExtinguishers = CharacterMediator.NoExtinshers;
            ExtinguisherActive += 1;
            if (ExtinguisherActive > NoExtinguishers)
                ExtinguisherActive = 0;
            IsPressed = true;
            _buttonImage.sprite = _sprites[ExtinguisherActive];
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
        public void SetValueExtinguisherActive(int value)
        {
            ExtinguisherActive = value;
            StartCoroutine(ActiveSprite());
        }
        IEnumerator ActiveSprite()
        {
            yield return new WaitForSeconds(.25f);
            _buttonImage.sprite = _sprites[ExtinguisherActive];
        }
    }
} 