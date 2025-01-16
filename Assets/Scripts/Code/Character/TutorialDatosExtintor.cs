using Character.ExtinguisherGas.Extinguisher;
using InputFolder;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TutorialDatosExtintor: MonoBehaviour
    {
        [SerializeField] private ExtinguisherController _extinguisherController;
        [SerializeField] private GameObject _panelDatosExtintor;
        [SerializeField] private GameObject[] _paneles;
        [Header("Duplicar imágenes activas")]
        [SerializeField] private Button[] _botones;
        [SerializeField] private Sprite[] _sprites;

        [Header("Duplicar Parámetros")]
        [SerializeField] private Image _extinsherLife;
        [SerializeField] private Image[] _extinsherLifes;

        private void Start()
        {
            StartCoroutine(HideElement());
        }
        private void Update()
        {
            if (_extinguisherController.GetValueId() != 0)
                _panelDatosExtintor.SetActive(true);
            else
                _panelDatosExtintor.SetActive(false);
            for(int i = 0; i < _paneles.Length; i++)
            {
                if (_paneles[i].activeSelf)
                    _botones[i].Select();
            }
            for (int i = 0; i < _extinsherLifes.Length; i++)
                _extinsherLifes[i].fillAmount = _extinsherLife.fillAmount;
        }
        IEnumerator HideElement()
        {
            yield return new WaitForSeconds(10);
            _panelDatosExtintor.SetActive(false);
            enabled = false;
        }
    }
}