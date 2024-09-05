using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueBossLine
{
    public string line;
    public AudioClip audioClip;
    public Sprite characterImage;
    public bool isPlayerSpeaking;
}

public class DialogueBoss : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] GameObject ente, finalExplosion;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] List<DialogueLine> dialogueLines;
    [SerializeField] float textSpeed;
    [SerializeField] Image characterImage; // El Image donde se mostrará la imagen del personaje
    [SerializeField] GameObject continueBtn, backBtn, cambiarDestinoBtn;
    private LobbyManager lobbyManager;
    private PlayerMovementNew playerMovement;
    private LevelManager levelManager;
    private int index;

    private Coroutine blinkCoroutine; // Corrutina para el efecto de "pestañeo"
    [HideInInspector] public Coroutine typeLineCoroutine, autoAdvanceDialogue;

    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    private bool wasPreviousPlayerSpeaking;
    private bool firstTime = true;
    public bool isFinalPanel;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
        lobbyManager = FindAnyObjectByType<LobbyManager>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnEnable()
    {
        playerMovement.inputsEnabled = false;
        playerMovement.anim.SetBool("SlowWalk", false);
        StartBlinkAnimation();
    }

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void Update()
    {
        playerMovement.anim.SetBool("SlowWalk", false);


    }

    public void OnButtonDown()
    {
        AudioManager.Instance.PlaySfx("btn_normal");

        if (textComponent.text == dialogueLines[index].line)
        {
            NextLine();
        }
        else
        {
            if (typeLineCoroutine != null) StopCoroutine(typeLineCoroutine);
            textComponent.text = dialogueLines[index].line;
            continueBtn.SetActive(true);
            backBtn.SetActive(true);

        }
    }
    public void OnBackButtonDown()
    {
        AudioManager.Instance.PlaySfx("btn_normal");

        if (index > 0)
        {
            index--;
            if (typeLineCoroutine != null)
            {
                StopCoroutine(typeLineCoroutine);
            }
            textComponent.text = string.Empty;
            typeLineCoroutine = StartCoroutine(TypeLine());
        }
    }
    public void OnChangeButtonDown()
    {
        unpaused.TransitionTo(.5f);

        AudioManager.Instance.PlaySfx("btn_normal");

        gameObject.SetActive(false);
        lobbyManager.PaneoCamera();
     
    }
    private void StartDialogue()
    {
        paused.TransitionTo(.5f);

        index = 0;
        wasPreviousPlayerSpeaking = dialogueLines[index].isPlayerSpeaking; // Inicializar con el primer valor
        if (typeLineCoroutine != null)
        {
            StopCoroutine(typeLineCoroutine);
        }
        typeLineCoroutine = StartCoroutine(TypeLine());

        if (firstTime)
        {
            autoAdvanceDialogue =  StartCoroutine(AutoAdvanceDialogue());
        }
        else
        {
           // continueBtn.SetActive(true);
           // backBtn.SetActive(true);
        }
    }
    private IEnumerator AutoAdvanceDialogue()
    {
        while (index < dialogueLines.Count)
        {
            yield return new WaitForSeconds(dialogueLines[index].line.Length * textSpeed);
            if (dialogueLines[index].audioClip != null)
            {
                yield return new WaitUntil(() => !GetComponent<AudioSource>().isPlaying);
            }
            
            NextLine();
            if (!isFinalPanel)
            {

                if (index == 3 || index == 6)
                {
                    ente.SetActive(true);
                    ente.GetComponent<Ente>().EnteSolidify();
                    ente.transform.GetChild(0).gameObject.SetActive(true);
                    ente.transform.GetChild(1).gameObject.SetActive(true);
                }
                if (ente.activeSelf && index == 4)
                {
                    //ente.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmmount", 1);
                    ente.GetComponent<Ente>().EnteDisolve();

                }
            }
            else
            {
                if (index == 3)
                {
                    ente.SetActive(true);
                    ente.GetComponent<Ente>().EnteSolidify();
                    ente.transform.GetChild(0).gameObject.SetActive(true);
                    ente.transform.GetChild(1).gameObject.SetActive(true);
                    yield return null;
                }

            }
        }
        firstTime = false;
    }
    private IEnumerator TypeLine()
    {
       
        // Actualiza la imagen del personaje
        characterImage.sprite = dialogueLines[index].characterImage;

        if (dialogueLines[index].audioClip != null)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(dialogueLines[index].audioClip);
        }

        if (dialogueLines[index].isPlayerSpeaking)
        {
            characterImage.rectTransform.anchorMin = new Vector2(0.7901939f, 0.2722537f);
            characterImage.rectTransform.anchorMax = new Vector2(0.8766842f, 0.7227457f);
            textComponent.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            characterImage.rectTransform.anchorMin = new Vector2(0.1002947f, 0.2801455f);
            characterImage.rectTransform.anchorMax = new Vector2(0.186785f, 0.7306374f);
            textComponent.alignment = TextAlignmentOptions.Center;
        }

        // Muestra el diálogo carácter por carácter
        textComponent.text = string.Empty;
        foreach (char c in dialogueLines[index].line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        //continueBtn.SetActive(true);
        //backBtn.SetActive(true);

    }

    public void StartBlinkAnimation()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        if (gameObject.activeSelf) blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        AudioManager.Instance.PlaySfx("Panel_blink");
        // Escala inicial
        Vector3 originalScale = transform.localScale;

        float duration = 0.25f; 
        float halfDuration = duration / 2f;

        // Encoger
        float timer = 0f;
        while (timer < halfDuration)
        {
            float scale = Mathf.Lerp(1f, 0f, timer / halfDuration);
            transform.localScale = new Vector3(originalScale.x, scale, originalScale.z);
            timer += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que termine en escala 0
        transform.localScale = new Vector3(originalScale.x, 0f, originalScale.z);

        // Crecer
        timer = 0f;
        while (timer < halfDuration)
        {
            float scale = Mathf.Lerp(0f, 1f, timer / halfDuration);
            transform.localScale = new Vector3(originalScale.x, scale, originalScale.z);
            timer += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que termine en escala 1
        transform.localScale = new Vector3(originalScale.x, 1f, originalScale.z);
    }

    private void NextLine()
    {
        if (index < dialogueLines.Count - 1)
        {
            index++;



            textComponent.text = string.Empty;

            // Verificar si el interlocutor ha cambiado
            if (dialogueLines[index].isPlayerSpeaking != wasPreviousPlayerSpeaking)
            {
                StartBlinkAnimation();
                wasPreviousPlayerSpeaking = dialogueLines[index].isPlayerSpeaking; // Actualizar el valor
            }

            if (typeLineCoroutine != null)
            {
                StopCoroutine(typeLineCoroutine);
            }
            typeLineCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            //Aqui termina los dialogos
            unpaused.TransitionTo(.5f);
            StopCoroutine(autoAdvanceDialogue);
            //backBtn.SetActive(true);
            //continueBtn.SetActive(true);
            //cambiarDestinoBtn.SetActive(true);
            if (!isFinalPanel)
            {
                boss.BossDisolve();
                if (ente.activeSelf) ente.GetComponent<Ente>().EnteDisolve();
            }
            else
            {
                //playerMovement.inputsEnabled = true;
                StartCoroutine(FinalLevel());
            }
        }

    }
    private IEnumerator FinalLevel()
    {
        yield return new WaitForSeconds(2);
        finalExplosion.SetActive(true);
        finalExplosion.GetComponent<FinalExplosion>().StartExplosion();
        yield return new WaitForSeconds(1);
        StartCoroutine(StartRunner());
    }
        
    private IEnumerator StartRunner()
    {
        yield return new WaitForSeconds(1f);
        boss.gameObject.SetActive(false);
        ente.SetActive(false);
        gameObject.SetActive(false);
        playerMovement.isMoving = true;
        playerMovement.canMove = true;
        playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
        playerMovement.anim.SetBool("Walk", true);
    }
}
