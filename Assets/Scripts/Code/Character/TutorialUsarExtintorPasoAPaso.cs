using Character;
using Character.ExtinguisherGas.Extinguisher;
using InputFolder;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TutorialUsarExtintorPasoAPaso : MonoBehaviour
{
    [SerializeField] private Slider _sliderBlock;
    [SerializeField] private Joystick _joystickDial;
    [SerializeField] private GameObject[] _images;
    private CharacterMediator _character;
    private ExtinguisherController _extinguisherController;
    private DialButtonOnUI _dial;
    int value;

    private void Start()
    {
        _character = GetComponent<CharacterMediator>();
        _extinguisherController = GetComponent<ExtinguisherController>();
        _dial = _joystickDial.GetComponent<DialButtonOnUI>();
        value = 0;
        for (int i = 1; i < _images.Length; i++)
            _images[i].SetActive(false);
    }
    private void Update()
    {
        if (_sliderBlock.gameObject.activeSelf && !_images[0].activeSelf && value == 0)
        {
            //print("Está entrando 1");
            _images[0].SetActive(true);
            _images[1].SetActive(false);
            _images[2].SetActive(false);
            _images[3].SetActive(false);
            value = 1;
            return;
        }
        if (!_dial.IsDialPressed && _dial.gameObject.activeSelf && _images[0].activeSelf && value == 1)
        {
            _images[0].SetActive(false);
            _images[1].SetActive(true);
            _images[2].SetActive(false);
            _images[3].SetActive(false);
            value = 2;
            return;
        }
        if (_dial.IsDialPressed && _images[1].activeSelf && value == 2)
        {
            _images[0].SetActive(false);
            _images[1].SetActive(false);
            _images[2].SetActive(true);
            _images[3].SetActive(false);
            value = 3;
            return;
        }
        if (_dial.IsDialPressed && _dial.ValorAngulo() != Vector2.zero && _images[2].activeSelf && value == 3)
        {
            _images[0].SetActive(false);
            _images[1].SetActive(false);
            _images[2].SetActive(false);
            _images[3].SetActive(true);
            value = 4;
            return;
        }
        if (_sliderBlock.gameObject.activeSelf && _images[3].gameObject.activeSelf && value == 4)
        {
            _images[0].gameObject.SetActive(false);
            _images[1].gameObject.SetActive(false);
            _images[2].gameObject.SetActive(false);
            _images[3].gameObject.SetActive(false);
            value = 0;
            return;
        }
    }
}
