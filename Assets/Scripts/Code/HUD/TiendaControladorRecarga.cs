using Character.ExtinguisherGas.Extinguisher;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TiendaControladorRecarga : MonoBehaviour
{
    [SerializeField] private ExtinguisherController _extinguisherController;
    [SerializeField] private TextMeshProUGUI _textoMonedas;
    [SerializeField] private int[] _valorRecarga;
    [SerializeField] private List<Image> _imagenes, _imagenesRecarga;
    [SerializeField] private GameObject _panelErrorEnCompra, _panelCompraRealizada, _panelRecargaExtintor;
    [SerializeField] private float _distanceButtons;
    [SerializeField] private TimerHud _timer;
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        if(!_timer) _timer = FindAnyObjectByType<TimerHud>();
        if (_imagenes.Count > 0) _imagenes.Clear();
        if (_imagenesRecarga.Count > 0) _imagenesRecarga.Clear();
        for (int i = 0; i < 5; i++)
        {
            _imagenes.Add(null);
            _imagenesRecarga.Add(null);
        }
    }
    public void DoStart()
    {
        StartCoroutine(DoStartCoroutine());
    }
    IEnumerator DoStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(.01f);
        _textoMonedas.SetText("SALDO DISPONIBLE $" + ControlDatos._coins);
        if (!_extinguisherController) _extinguisherController = FindFirstObjectByType<ExtinguisherController>();
        for (int i = 0; i < 5; i++)
        {
            _imagenes[i] = transform.GetChild(3).GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            _imagenesRecarga[i]= _panelRecargaExtintor.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            transform.GetChild(3).GetChild(i).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().SetText("$" + _valorRecarga[i]);
        }
        for (int i = 0; i < 5; i++)
        {
            _imagenes[i].fillAmount = ExtinguisherController._valueExtinguishersCarga[i + 1];
            _imagenesRecarga[i].fillAmount = ExtinguisherController._valueExtinguishersCarga[i + 1];
            //_imagenes[i].fillAmount = _extinguisherController.extinguishersValues[i + 1]._valueExtinguishersCarga;
            //_imagenesRecarga[i].fillAmount = _extinguisherController.extinguishersValues[i + 1]._valueExtinguishersCarga;
            //if (_extinguisherController.NoExtinguishers >= i + 1)
            if (_extinguisherController.GetValueId() == i + 1)
            {
                if (_extinguisherController.extinguishersValues[i + 1]._valueExtinguishersCarga < .95f)
                {
                    transform.GetChild(3).GetChild(i).GetComponent<Button>().interactable = true;
                    transform.GetChild(3).GetChild(i).GetChild(3).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(3).GetChild(i).GetComponent<Button>().interactable = false;
                    transform.GetChild(3).GetChild(i).GetChild(3).gameObject.SetActive(true);
                }
                RectTransform rect = transform.GetChild(3).GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(- _distanceButtons * (_extinguisherController.GetValueId() - 1), rect.anchoredPosition.y);
                transform.GetChild(3).GetChild(i).transform.localScale = Vector3.one;
            }
            else
            {
                transform.GetChild(3).GetChild(i).transform.localScale = Vector3.zero;
                transform.GetChild(3).GetChild(i).GetComponent<Button>().interactable = false;
                transform.GetChild(3).GetChild(i).GetChild(3).gameObject.SetActive(true);
            }
        }
    }
    public void Compra(int indice)
    {
        StartCoroutine(CompraCoroutine(indice));
    }
    IEnumerator CompraCoroutine(int indice)
    {
        if (ControlDatos._coins < _valorRecarga[indice - 1])
        {
            _panelErrorEnCompra.SetActive(true);
            _panelCompraRealizada.SetActive(false);
            for (int i = 0; i < _panelErrorEnCompra.transform.childCount; i++)
            {
                if (i == indice - 1)
                    _panelErrorEnCompra.transform.GetChild(i).gameObject.SetActive(true);
                else
                    _panelErrorEnCompra.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _panelCompraRealizada.transform.childCount; i++)
            {
                if (i == indice - 1)
                {
                    if (ExtinguisherController._valueExtinguishersCarga[i + 1] > .98f)
                        yield break;
                    _panelRecargaExtintor.SetActive(true);
                    _panelRecargaExtintor.transform.GetChild(i).gameObject.SetActive(true);
                    yield return new WaitForSecondsRealtime(.5f);
                    _audio.Play();
                    //CerrarPantalla(indice);
                    while (ExtinguisherController._valueExtinguishersCarga[i + 1] < 1)
                    {
                        _imagenes[i].fillAmount = _imagenesRecarga[i].fillAmount = _extinguisherController.extinguishersValues[i + 1]._valueExtinguishersCarga =  ExtinguisherController._valueExtinguishersCarga[i + 1] += .005f;
                        if (ExtinguisherController._valueExtinguishersCarga[i + 1] >= .95f)
                            _imagenes[i].fillAmount = _imagenesRecarga[i].fillAmount = _extinguisherController.extinguishersValues[i + 1]._valueExtinguishersCarga = ExtinguisherController._valueExtinguishersCarga[i + 1] = 1;
                        yield return new WaitForSecondsRealtime(.025f);
                    }
                    yield return new WaitForSecondsRealtime(.25f);
                    _panelCompraRealizada.transform.GetChild(i).gameObject.SetActive(true);
                    _extinguisherController.ChangeValueCarga(1);
                    _extinguisherController.SetColorCarga();

                    ControlDatos._coins -= _valorRecarga[indice - 1];
                    if (_timer) _timer.SetNoMonedas();
                    if (_textoMonedas) _textoMonedas.SetText("SALDO DISPONIBLE $" + ControlDatos._coins);
                    _panelErrorEnCompra.SetActive(false);
                    _panelCompraRealizada.SetActive(true);
                    _panelRecargaExtintor.transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    _panelCompraRealizada.transform.GetChild(i).gameObject.SetActive(false);
                    _panelRecargaExtintor.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        _audio.Stop();
        _panelRecargaExtintor.SetActive(false);
        gameObject.SetActive(false);
    }
    public void CerrarPantalla(int indice)
    {
        if(_imagenes[indice].fillAmount == 1)
        {
            _panelCompraRealizada.transform.GetChild(indice).gameObject.SetActive(true);
            _extinguisherController.ChangeValueCarga(1);
            _extinguisherController.SetColorCarga();

            ControlDatos._coins -= _valorRecarga[indice - 1];
            if(_timer) _timer.SetNoMonedas();
            if (_textoMonedas) _textoMonedas.SetText("SALDO DISPONIBLE $" + ControlDatos._coins);
            _panelErrorEnCompra.SetActive(false);
            _panelCompraRealizada.SetActive(true);
            _panelRecargaExtintor.transform.GetChild(indice).gameObject.SetActive(false);
        }
    }
}
