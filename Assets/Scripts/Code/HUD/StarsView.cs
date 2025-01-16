using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsView : MonoBehaviour
{
    private TimerHud _timer;
    public static int _score = 0;
    public static int _intentos1 = 1, _intentos2 = 1, _intentos3 = 1, _intentos4 = 1, _intentos5 = 1, _intentosBoss = 1;
    private Image _image1, _image2, _image3;
    private int _lvl;
    // Start is called before the first frame update
    private void Start()
    {
        _lvl = FindAnyObjectByType<CharacterInstaller>()._lvl;
        _image1 = transform.GetChild(0).GetComponent<Image>();
        _image2 = transform.GetChild(1).GetComponent<Image>();
        _image3 = transform.GetChild(2).GetComponent<Image>();
        _timer = FindObjectOfType<TimerHud>();
        if (!_timer) return;
        if(_lvl == 1)_score = (int)_timer._time / _intentos1;
        if(_lvl == 2)_score = (int)_timer._time / _intentos2;
        if(_lvl == 3)_score = (int)_timer._time / _intentos3;
        if(_lvl == 4)_score = (int)_timer._time / _intentos4;
        if(_lvl == 5)_score = (int)_timer._time / _intentos5;
        if(_score < (_timer._twoStarsTime) && _score > (_timer._threeStarsTime))
        {
            _image3.color = Color.black;
        }
        if(_score <= (_timer._threeStarsTime))
        {
            _image3.color = Color.black;
            _image2.color = Color.black;
        }
    }
}
