using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{
    [SerializeField] private CharacterMediator _characterMediator;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private GameObject[] _gameObjectsToHide, _gameObjectsToShow;
    [Header("Objects in Finish Game\n")]
    [SerializeField] private GameObject[] _objectsToShowWinner;
    [SerializeField] private GameObject[] _objectsToHideWinner;
    private bool isWinner = false;
    CharacterInstaller characterInstaller;
    // Start is called before the first frame update
    void Awake()
    {
        if (!_characterMediator) _characterMediator = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMediator>();
        characterInstaller = GameObject.FindAnyObjectByType<CharacterInstaller>();
    }

    public void MoveToAnotherPos()
    {
        if(!_characterMediator) _characterMediator = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMediator>();
        _characterMediator.enabled = false;
        if (characterInstaller._lvl == 1 && ControlDatos._isWinnerLvl1) isWinner = true;
        if (characterInstaller._lvl == 2 && ControlDatos._isWinnerLvl2) isWinner = true;
        if (characterInstaller._lvl == 3 && ControlDatos._isWinnerLvl3) isWinner = true;
        if (characterInstaller._lvl == 4 && ControlDatos._isWinnerLvl4) isWinner = true;
        if (characterInstaller._lvl == 5 && ControlDatos._isWinnerLvl5) isWinner = true;
        StartCoroutine(EncenderMediator());
    }
    IEnumerator EncenderMediator()
    {
        foreach (var gameObjectToShow in _gameObjectsToShow)
            if (!gameObjectToShow.activeSelf)
                gameObjectToShow.SetActive(true);
        if (isWinner)
        {
            foreach (var gameObjectToShow in _objectsToShowWinner)
                if (!gameObjectToShow.activeSelf)
                    gameObjectToShow.SetActive(true);
        }
        _characterMediator.transform.position = _targetPosition.position;
        yield return new WaitForSecondsRealtime(.1f);
        _characterMediator.enabled = true;
        _characterMediator._input.GetDirection(true);
        foreach (var gameObjectToHide in _gameObjectsToHide)
            if (gameObjectToHide.activeSelf)
                gameObjectToHide.SetActive(false);
        if (isWinner)
        {
            foreach (var gameObjectToHide in _objectsToHideWinner)
                if (gameObjectToHide.activeSelf)
                    gameObjectToHide.SetActive(false);
        }
    }
}
