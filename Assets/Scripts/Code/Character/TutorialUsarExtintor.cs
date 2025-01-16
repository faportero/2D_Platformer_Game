using Character.ExtinguisherGas.Extinguisher;
using InputFolder;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TutorialUsarExtintor: MonoBehaviour
    {
        [SerializeField] private Slider _sliderBlock;
        [SerializeField] private Joystick _joystickDial;
        [SerializeField] private Image[] _images;
        private CharacterMediator _character;
        private InputInterface _input;
        private ExtinguisherController _extinguisherController;
        private DialButtonOnUI _dial;
        int value;

        private void Start()
        {
            _character = GetComponent<CharacterMediator>();
            _input = _character._input;
            _extinguisherController = GetComponent<ExtinguisherController>();
            _dial = _joystickDial.GetComponent<DialButtonOnUI>();
            value = 0;
            for (int i = 1; i < _images.Length; i++)
                _images[i].gameObject.SetActive(false);
        }
        private void Update()
        {
            if (_extinguisherController.GetValueId() != 0 && !_images[1].gameObject.activeSelf && value == 0)
            {
                //print("Está entrando 1");
                _images[0].gameObject.SetActive(false);
                _images[2].gameObject.SetActive(false);
                _images[3].gameObject.SetActive(false);
                _images[4].gameObject.SetActive(false);
                _images[1].gameObject.SetActive(true);
                value = 1;
                return;
            }
            if (!_dial.IsDialPressed && _dial.gameObject.activeSelf && _images[1].gameObject.activeSelf && value == 1)
            {
                _images[1].gameObject.SetActive(false);
                _images[2].gameObject.SetActive(true);
                _images[3].gameObject.SetActive(false);
                _images[4].gameObject.SetActive(false);
                value = 2;
                return;
            }
            if (_sliderBlock.gameObject.activeSelf && value > 1)
            {
                _images[2].gameObject.SetActive(false);
                _images[3].gameObject.SetActive(false);
                _images[4].gameObject.SetActive(false);
                value = 0;
                return;
            }
            if (_dial.IsDialPressed && _images[2].gameObject.activeSelf && value == 2)
            {
                StartCoroutine(Encender());
                return;
            }
            //if (!_dial.IsDialPressed && _dial.gameObject.activeSelf && _images[1].gameObject.activeSelf && value == 1)
            //{
            //    _images[1].gameObject.SetActive(false);
            //    _images[2].gameObject.SetActive(true);
            //    _images[3].gameObject.SetActive(true);
            //    _images[4].gameObject.SetActive(true);
            //    value = 2;
            //    return;
            //}
            //if (_dial.IsDialPressed && _images[2].gameObject.activeSelf && value == 2)
            //{
            //    _images[2].gameObject.SetActive(true);
            //    _images[3].gameObject.SetActive(true);
            //    _images[4].gameObject.SetActive(true);
            //    return;
            //}
            //if (_images[4].gameObject.activeSelf && _sliderBlock.gameObject.activeSelf && value == 3)
            //{
            //    value = 1;
            //    //Destroy(this);
            //}
        }
        private IEnumerator Encender()
        {
            _images[1].gameObject.SetActive(false);
            _images[3].gameObject.SetActive(false);
            _images[4].gameObject.SetActive(false);
            _images[2].gameObject.SetActive(false);
            yield return new WaitForSeconds(.2f);
            _images[2].gameObject.SetActive(false);
            _images[3].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _images[3].gameObject.SetActive(false);
            if (value == 2)
                _images[4].gameObject.SetActive(true);
        }
    }
}