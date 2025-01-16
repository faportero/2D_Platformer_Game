using Character;
using Character.ExtinguisherGas.Extinguisher;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FireType
{
    public class Fire1 : Fire
    {
        public int _tipo;
        float _valEscala = .05f;
        private string _tipoFuego;

        protected override float DistanceCalc(Transform t)
        {
            _distance = Vector2.Distance(t.transform.position, transform.position) * _constantValueMultiplier;
            //print(_id.Value + " Constant Value Multiplier: " + _constantValueMultiplier + ". Distance: " + _distance);
            return _distance;
        }

        protected override void DoDestroy()
        {
            if (_life > 0)
            {
                _life--;
                _slider.value -= 1;
                return;
            }
            if (transform.GetChild(1).GetComponentInChildren<Image>().color != Color.clear)
            {
                FireController._totalFires--;
                if (!_activeDesactiveObjects.enabled) _activeDesactiveObjects.enabled = true;
                _activeDesactiveObjects.DoStart();
            }
            _character._isInFireTrigger = false;
            _angle = 0;
            if(_id.Value == "Fire1") _tipoFuego = "A";
            if(_id.Value == "Fire2") _tipoFuego = "B";
            if(_id.Value == "Fire3") _tipoFuego = "C";
            if(_id.Value == "Fire4") _tipoFuego = "D";
            if(_id.Value == "Fire5") _tipoFuego = "K";
            if(transform.GetChild(0).localScale.x != 0) _sounds.AddTextItems("Fuego", "Conato de incendio tipo "+ _tipoFuego + " controlado");

            if (FireController._totalFires <= 0 && !_movementController._isInBossTrigger)
            {
                CharacterInstaller characterInstaller = GameObject.FindAnyObjectByType<CharacterInstaller>();
                if (characterInstaller._lvl == 1)
                {
                    ControlDatos._isWinnerLvl1 = true;
                    ControlDatos._currentNivel = 1;
                }
                if (characterInstaller._lvl == 2)
                {
                    ControlDatos._isWinnerLvl2 = true;
                    ControlDatos._currentNivel = 2;
                }
                if (characterInstaller._lvl == 3)
                {
                    ControlDatos._isWinnerLvl3 = true;
                    ControlDatos._currentNivel = 3;
                }
                if (characterInstaller._lvl == 4)
                {
                    ControlDatos._isWinnerLvl4 = true;
                    ControlDatos._currentNivel = 4;
                }
                if (characterInstaller._lvl == 5)
                {
                    ControlDatos._isWinnerLvl5 = true;
                    ControlDatos._currentNivel = 5;
                }

                //Como ya no tiene que caminar al trigger para enviar al siguiente nivel. Le sumo monedas al momento de ganar.

                ControlDatos._coins += characterInstaller._lvl * 50;
                TimerHud timerHud = FindAnyObjectByType<TimerHud>();
                timerHud.SetNoMonedas();
                ControlDatos._points += characterInstaller._lvl * 100 + (int)timerHud._time * 10;
                PlayerPrefs.SetInt("Coins", ControlDatos._coins);
                PlayerPrefs.SetInt("Points", ControlDatos._points);
                PlayerPrefs.Save();
                ControlDatos._listaObjetosInventario = new List<Vector2>
                {
                    new Vector2(2, ControlDatos._points),
                    new Vector2(3, ControlDatos._coins)
                };
                if (ControlDatos._points > ControlDatos._bestPoints)
                {
                    ControlDatos._bestPoints = ControlDatos._points;
                    ControlDatos._listaObjetosInventario.Add(new Vector2(1, ControlDatos._bestPoints));
                }
                if (ControlDatos._currentNivel > ControlDatos._nivel)
                {
                    ControlDatos._nivel = ControlDatos._currentNivel;
                    ControlDatos._listaObjetosInventario.Add(new Vector2(4, ControlDatos._nivel));
                }
                ControlDatos.CrearEditarObjetoInventario();
                FireController.ActiveDesactive();
                FindAnyObjectByType<ExtinguisherController>().StopAudioSourceCarga();


                GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag(tag) && obj.hideFlags != HideFlags.NotEditable && obj.hideFlags != HideFlags.HideAndDontSave)
                    {
                        if (obj.tag == "Time" || obj.tag == "Speed")
                        {
                            obj.tag = "Untagged";
                            obj.SetActive(false);
                            Destroy(obj);
                        }
                    }
                }
            }
            Destroy(gameObject, .1f);
        }

        protected override void DoStart()
        {
            _angle = 0;
            _sounds = FindAnyObjectByType<PlayCharacterSounds>();
            _canvas = GetComponentInChildren<Canvas>();
            if(_canvas) _canvas.worldCamera = Camera.main;
            _slider = GetComponentInChildren<Slider>();
            _slider.value = _life;
            _lineRendererParent = transform.GetChild(0).gameObject;
            _fireImage = transform.GetChild(2);
            _fireImage2 = transform.GetChild(3);
            _spriteFace = _fireImage.GetChild(2).GetComponent<SpriteRenderer>();
            _fireTypeParent = transform.GetChild(4);
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _textDistance = GetComponentInChildren<TextMeshPro>();
            _activeDesactiveObjects = GetComponent<ActiveDesactiveObjects>();
            _myTransform = transform;
            if (_lineRenderer)
            {
                _materialLine = _lineRenderer.material;
                _lineRenderer.gameObject.SetActive(false);
            }
            for (int i = 0; i < _fireTypeParent.childCount; i++)
            {
                if (i == _tipo || i == _fireTypeParent.childCount - 1)
                    _fireTypeParent.GetChild(i).gameObject.SetActive(true);
                else
                    _fireTypeParent.GetChild(i).gameObject.SetActive(false);
            }
            _fireTypeParent.gameObject.SetActive(false);
        }
        public void ChangeConstantValueMultiplier(float value)
        {
            _constantValueMultiplier = value;
        }
        public float GetConstantMultiplier()
        {
            return _constantValueMultiplier;
        }
        public void ChangeLife(int value)
        {
            _life = value;
        }
    }
}