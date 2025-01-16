using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowBigMap : MonoBehaviour
{
    public GameObject _feedbackPanel, _mapPanel;
    private bool _isShowingBigMap = false;

    public void ShowMap()
    {
        _isShowingBigMap = !_isShowingBigMap;
        if (_isShowingBigMap)
        {
            _feedbackPanel.SetActive(true);
            _mapPanel.SetActive(true);
        }
        else
        {
            _feedbackPanel.SetActive(false);
            _mapPanel.SetActive(false);
        }
    }
}
