using Character;
using FireType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscogerAudio : MonoBehaviour
{
    public GameObject _panelTiempo, panelFeedbacks, bossGameObject;
    public AudioClip[] audiosNivel;
    public AudioClip[] audiosMision;
    [HideInInspector] public AudioSource audioSource;
    bool _isPause;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (panelFeedbacks)
        {
            if (panelFeedbacks.activeSelf)
            {
                if (Time.timeScale == 1)
                {
                    _isPause = false;
                    Time.timeScale = 0;
                }
            }
            else
                if (Time.timeScale == 0)
            {
                _isPause = true;
                Time.timeScale = 1;
            }
        }
        if (Time.timeScale == 1)
        {
            if (_isPause)
            {
                audioSource.UnPause();
                _isPause = false;
            }
            if (bossGameObject)
            {
                if (bossGameObject.activeSelf)
                {
                    int rnd = audiosMision.Length;
                    for (int i = 0; i < rnd; i++)
                    {
                        if (audiosMision[i] == audioSource.clip && !audioSource.isPlaying)
                        {
                            audioSource.clip = audiosMision[Random.Range(0, rnd)];
                            audioSource.Play();
                            return;
                        }
                        if (audiosMision[i] == audioSource.clip) return;
                    }
                    audioSource.clip = audiosMision[Random.Range(0, rnd)];
                    audioSource.Play();
                    return;
                }
            }
            if (!_panelTiempo.activeSelf)
            {
                int rnd = audiosNivel.Length;
                for (int i = 0; i < rnd; i++)
                {
                    if (audiosNivel[i] == audioSource.clip && !audioSource.isPlaying)
                    {
                        audioSource.clip = audiosNivel[Random.Range(0, rnd)];
                        audioSource.Play();
                        return;
                    }
                    if (audiosNivel[i] == audioSource.clip) return;
                }
                audioSource.clip = audiosNivel[Random.Range(0, rnd)];
                audioSource.Play();
                return;
            }
            else
            {
                int rnd = audiosMision.Length;
                for (int i = 0; i < rnd; i++)
                {
                    if (audiosMision[i] == audioSource.clip && !audioSource.isPlaying)
                    {
                        audioSource.clip = audiosMision[Random.Range(0, rnd)];
                        audioSource.Play();
                        return;
                    }
                    if (audiosMision[i] == audioSource.clip) return;
                }
                audioSource.clip = audiosMision[Random.Range(0, rnd)];
                audioSource.Play();
                return;
            }
        }
        else if (Time.timeScale == 0)
        {
            _isPause = true;
            audioSource.Pause();
        }
    }

    public void RestartBossFight()
    {
        if (StarsView._intentosBoss > 1)
        {
            var boss = FindAnyObjectByType<BossController>();
            boss.SetId(0);
            Destroy(boss.transform.GetComponentInChildren<Fire>().gameObject);
            boss.InstantiateCurrentFire();
            boss.transform.localPosition = Vector3.zero;
            var movementController = FindAnyObjectByType<MovementController>();
            var intentos = 1;
            if(StarsView._intentosBoss < 5)
            {
                intentos = StarsView._intentosBoss;
            }
            else
            {
                intentos = 5;
            }
            movementController.RestartLife(movementController._startVida - (intentos * 2));
            movementController.SetLifeSlider2Value();
            FindAnyObjectByType<CharacterInstaller>().GamePause(false);
            panelFeedbacks.SetActive(false);
            var player = FindAnyObjectByType<MovementController>();
            player.transform.position = bossGameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            SceneManager.LoadScene("6. Nivel 5");
        }
    }
}
