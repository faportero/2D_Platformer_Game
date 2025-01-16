using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayCharacterSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audios;
    private bool isPlaying;
    [SerializeField] private TextMeshProUGUI _textoMonedas;
    [SerializeField] private TextMeshProUGUI[] _textosMonedas;
    [SerializeField] private GameObject _prefabMessages, _messagesParent;
    private int _contador;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _textoMonedas.SetText(ControlDatos._coins.ToString());
        for (int i = 0; i < _textosMonedas.Length; i++)
            _textosMonedas[i].SetText(ControlDatos._coins.ToString());
    }
    public void AddTextCoins()
    {
        _textoMonedas.SetText(ControlDatos._coins.ToString());
        for (int i = 0; i < _textosMonedas.Length; i++)
            _textosMonedas[i].SetText(ControlDatos._coins.ToString());
    }
    public void AddTextItems(string name, string text)
    {
        for(int item = 0; item < _messagesParent.transform.childCount; item++)
            if (_messagesParent.transform.GetChild(item).name == name) Destroy(_messagesParent.transform.GetChild(item).gameObject);
        var instance = Instantiate(_prefabMessages);
        var scale = instance.transform.localScale;
        instance.transform.parent = _messagesParent.transform;
        instance.transform.localScale = scale;
        instance.name = name;
        _contador++;
        instance.GetComponentInChildren<TextMeshProUGUI>().SetText(text);
        Destroy(instance.gameObject, 4);
    }
    public void PlaySound()
    {
        int value = Random.Range(0, _audios.Length);
        _audioSource.clip = _audios[value];
        //print("Clip: " + _audioSource.clip.name + ". Indice: " + value);
        StartCoroutine(PlaySoundCorrutine());
    }
    IEnumerator PlaySoundCorrutine()
    {
        isPlaying = true;
        float _time = _audioSource.clip.length;
        _audioSource.Play();
        yield return new WaitForSeconds(_time);
        _audioSource.Stop();
        yield return new WaitForSeconds(1);
        isPlaying = false;
    }
}
