using Character;
using Character.ExtinguisherGas.Extinguisher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extinguisher
{
    public class ActiveExtinguisher : MonoBehaviour
    {
        [SerializeField] private Color[] _colors;
        private ExtinguisherController _extinguisher;
        private CharacterMediator _character;
        public bool _newExtinguisher;
        public int _idExtinguisher, _numExtinguishers;
        SpriteRenderer _sprite1, _sprite2, _sprite3;
        private bool isCreate = false;
        Transform _zoomGO, _zoomGO2;
        SpriteRenderer _backColor, _backColor2;

        // Start is called before the first frame update
        private void Start()
        {
            GameObject _player = GameObject.FindGameObjectWithTag("Player");
            _extinguisher = _player.GetComponent<ExtinguisherController>();
            _character = _player.GetComponent<CharacterMediator>();
            if (transform.childCount == 0) return;
            else if (transform.childCount > 0)
            {
                _sprite1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
                _sprite2 = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
                _sprite3 = transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>();
                _zoomGO = transform.GetChild(1).GetChild(2);
                _backColor = _zoomGO.GetChild(5).GetComponent<SpriteRenderer>();
                _zoomGO.rotation = _sprite1.transform.rotation;
                _sprite1.color = _colors[_idExtinguisher - 1];
                _sprite2.color = _colors[_idExtinguisher - 1];
                _sprite3.color = _colors[_idExtinguisher - 1];
                _backColor.color = _colors[_idExtinguisher - 1];
                _zoomGO2 = transform.GetChild(0).GetChild(1);
                _backColor2 = _zoomGO2.GetChild(5).GetComponent<SpriteRenderer>();
                _backColor2.color = _colors[_idExtinguisher - 1];
                _backColor2.color = new Color(_backColor2.color.r, _backColor2.color.g, _backColor2.color.b, .5f);
            }
            _zoomGO.rotation = _sprite1.transform.rotation;
            if (!_zoomGO.gameObject.activeSelf) _zoomGO.gameObject.SetActive(true);
            for (int i = 0; i < _zoomGO.childCount - 1; i++)
            {
                if (i == _idExtinguisher - 1)
                {
                    _zoomGO.GetChild(i).gameObject.SetActive(true);
                    _zoomGO2.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    _zoomGO.GetChild(i).gameObject.SetActive(false);
                    _zoomGO2.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        public void CreateExtinguisher()
        {
            _character.valorCarga = _extinguisher.extinguishersValues[_idExtinguisher]._valueExtinguishersCarga;
            StartCoroutine(TryCreateExtinguisher());
        }
        IEnumerator TryCreateExtinguisher()
        {
            //print("Valor Carga ANTES: " + _character.valorCarga);
            if (_extinguisher.GetValueId() != 0 && !isCreate)
            {
                isCreate = true;
                var instance = Instantiate(this, transform.position, transform.rotation, transform.parent);
                //Destroy(GetComponent<Collider2D>());
                instance._newExtinguisher = false;
                instance._numExtinguishers = CharacterMediator.NoExtinshers;
                instance._idExtinguisher = _extinguisher.GetValueId();
                instance.name = "ExtinguisherTrigger " + instance._idExtinguisher;
                var active = instance.GetComponent<ActiveDesactiveObjects>();
                if (active && active._once == true)
                    Destroy(active);
                //print("id: " + _extinguisher.GetValueId() + ". _character._NoExtinshers: " + CharacterMediator.NoExtinshers);
            }
            if (_newExtinguisher)
            {
                if (_numExtinguishers > CharacterMediator.NoExtinshers)
                {
                    _character.SetNoExtinguishers(_numExtinguishers);
                    _extinguisher.SetExtinguisher(new Vector2(_numExtinguishers, _idExtinguisher));
                }
                else
                {
                    _extinguisher.SetExtinguisher(new Vector2(CharacterMediator.NoExtinshers, _idExtinguisher));
                }
            }
            else _extinguisher.SetExtinguisher(new Vector2(CharacterMediator.NoExtinshers, _idExtinguisher));
            _extinguisher._activeIndicators = false;
            //print("Valor Carga DURANTE: " + _character.valorCarga);

            yield return new WaitForSeconds(.15f);
            //print("New id: " + _extinguisher.GetValueId() + ". New NoExtinshers: " + CharacterMediator.NoExtinshers);
            //print("Valor Carga DESPUES: " + _character.valorCarga);
            _extinguisher.SetColorCarga();
            _extinguisher.TryCreate();
            _extinguisher.extinguishersValues[_extinguisher.GetValueId()]._active = true;
        }
    }
}