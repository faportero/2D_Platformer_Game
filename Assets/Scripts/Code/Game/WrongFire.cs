using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongFire : MonoBehaviour
{
    public List<GameObject> _objectsToShow = new List<GameObject>();
    public GameObject _gameCanvas;
    public void DoStart()
    {
        if (_gameCanvas) _gameCanvas.SetActive(false);
        foreach (var _objectToShow in _objectsToShow)
            _objectToShow.SetActive(true);
        Time.timeScale = 0f;
        //Destroy(this, .05f);
    }
}
