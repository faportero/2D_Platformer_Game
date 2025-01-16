using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamparasActividad : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private GameObject _parentFires;
    [HideInInspector] public int _currentIndex;
    private SpriteRenderer _sprite, _spriteRoom;
    private bool ejecutarCorrutina;
    private List<GameObject> _triggers;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _spriteRoom = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _triggers = new List<GameObject>();
        _triggers.Clear();
        for (int i = 0; i < _spriteRoom.transform.childCount; i++)
        {
            _triggers.Add(_spriteRoom.transform.GetChild(i).gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(ejecutarCorrutina) StartCoroutine(LuzIntermitente());
        if (_currentIndex > 0)
        {
            _spriteRoom.sprite = _sprites[_currentIndex];
            if (!_triggers[_currentIndex - 1].activeSelf)
            {
                for (int i = 0; i < _spriteRoom.transform.childCount; i++)
                {
                    if (i == _currentIndex - 1)
                    {
                        _triggers[i].SetActive(true);
                    }
                    else
                    {
                        _triggers[i].SetActive(false);
                    }
                }
            }
        }
        else
        {
            _spriteRoom.sprite = null;
            for (int i = 0; i < _spriteRoom.transform.childCount; i++)
            {
                _triggers[i].SetActive(false);
            }
        }
    }
    void OnEnable()
    {
        ejecutarCorrutina = true;
    }
    IEnumerator LuzIntermitente()
    {
        ejecutarCorrutina = false;
        _sprite.enabled = false;
        yield return new WaitForSeconds(Random.Range(.25f, 1.5f));
        _sprite.enabled = true;
        yield return new WaitForSeconds(Random.Range(.01f, .1f));
        ejecutarCorrutina = true;
    }
}
