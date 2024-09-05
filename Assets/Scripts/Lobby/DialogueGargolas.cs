using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueLineGargola
{
    public string line;
    public AudioClip audioClip;
    public Sprite characterImage;
    public bool isPlayerSpeaking;
}

public class DialogueGargolas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] List<DialogueLineGargola> dialogueLines;
    [SerializeField] float textSpeed;
    [SerializeField] Image characterImage; // El Image donde se mostrará la imagen del personaje
    [SerializeField] GameObject continueBtn, backBtn, conrinueBtn, cambiarDestinoBtn;
    private LobbyManager lobbyManager;
    private PlayerMovementNew playerMovement;
    public int index;
    public int countDialogue;
    [SerializeField] Espejo espejo;
    [SerializeField] Ente ente;

    private Coroutine blinkCoroutine; // Corrutina para el efecto de "pestañeo"
    [HideInInspector] public Coroutine typeLineCoroutine, autoAdvanceDialogue;

    private bool wasPreviousPlayerSpeaking, cambiarDestino;
    private bool firstTime = true;
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
        lobbyManager = FindAnyObjectByType<LobbyManager>();
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
            //if (!dialogueLines[index].isPlayerSpeaking && !GetComponent<AudioSource>().isPlaying)
            //{
            //    if(autoAdvanceDialogue != null) StopCoroutine(autoAdvanceDialogue);
               
            //    cambiarDestinoBtn.SetActive(true);

            //}
            //else 
            //{
            //    cambiarDestinoBtn.SetActive(false);
            //}
        //print("PlayerSpeaking: "+ dialogueLines[index].isPlayerSpeaking+ ". Audio reproduciendo: " + GetComponent<AudioSource>().isPlaying + ". WasPreviousPlayer: "+ wasPreviousPlayerSpeaking); 

    }

    public void OnButtonDown()
    {
      
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
        if (index > 0)
        {
            index--;
            if (typeLineCoroutine != null)
            {
                StopCoroutine(typeLineCoroutine);
            }

            if (firstTime)
            {
                autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
            }

            textComponent.text = string.Empty;
            typeLineCoroutine = StartCoroutine(TypeLine());
        }
    }
    public void OnChangeButtonDown()
    {      
        AudioManager.Instance.PlaySfx("btn_normal");
       
        if (!cambiarDestino) 
        {
            cambiarDestinoBtn.SetActive(false);
            //autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
            NextLine();           
        }
        else
        {
            GetComponent<AudioSource>().Stop();
            espejo.StartCoroutine(espejo.AnimacionGargolas());
            playerMovement.isFacingRight = true;
            playerMovement.Turn();
            StartCoroutine(TypeLastLine());    
            cambiarDestinoBtn.SetActive(false);

        }
    }

    private IEnumerator TypeLastLine()
    {
        backBtn.GetComponent<Button>().interactable = false;
        continueBtn.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(2); 
        index++;
        typeLineCoroutine = StartCoroutine(TypeLine());
        yield return new WaitForSecondsRealtime(11);
        GetComponent<Animator>().Play("Hide Animation");
        ente.EnteDisolve();
        playerMovement.isMoving = true;
        playerMovement.inputsEnabled = true;
        espejo.panelHUD.SetActive(true);

        unpaused.TransitionTo(2.0f);
      
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
            autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
            //cambiarDestinoBtn.SetActive(true);
        }
        else
        {
            // continueBtn.SetActive(true);
            // backBtn.SetActive(true);
        }
    }
    private IEnumerator AutoAdvanceDialogue()
    {

        while (index < dialogueLines.Count - 2)
        {
            yield return new WaitForSeconds(dialogueLines[index].line.Length * textSpeed);
            if (dialogueLines[index].audioClip != null)
            {
                yield return new WaitUntil(() => !GetComponent<AudioSource>().isPlaying);

            }

            if (dialogueLines[index].isPlayerSpeaking)
            {
                NextLine();
            }
            else
            {
                cambiarDestinoBtn.SetActive(true);
                //if (autoAdvanceDialogue != null) StopCoroutine(autoAdvanceDialogue);
            }

            //NextLine();
        }
        firstTime = false;
        cambiarDestino = true;
    }

    private IEnumerator TypeLine()
    {
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

        textComponent.text = string.Empty;
        foreach (char c in dialogueLines[index].line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

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
        // Escala inicial
        Vector3 originalScale = transform.localScale;

        // Efecto de pestañeo
        float duration = 0.25f; // Duración total del pestañeo (encoger y crecer)
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
        //if (!dialogueLines[index].isPlayerSpeaking) autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
       // else if(autoAdvanceDialogue != null) StopCoroutine(autoAdvanceDialogue);

        if (index < dialogueLines.Count - 2)
        {
            index++;
            textComponent.text = string.Empty;
          
        
            // Verificar si el interlocutor ha cambiado
            if (dialogueLines[index].isPlayerSpeaking != wasPreviousPlayerSpeaking)
            {
                StartBlinkAnimation();
                wasPreviousPlayerSpeaking = dialogueLines[index].isPlayerSpeaking; // Actualizar el valor
                autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
            }

            if (typeLineCoroutine != null)
            {
                StopCoroutine(typeLineCoroutine);
            }
            typeLineCoroutine = StartCoroutine(TypeLine());

            //if (!dialogueLines[index].isPlayerSpeaking) autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());

            //if (!dialogueLines[index].isPlayerSpeaking && !GetComponent<AudioSource>().isPlaying)
            //{
            //    cambiarDestinoBtn.SetActive(true);
            //}
        }
        else
        {
            //StopCoroutine(autoAdvanceDialogue);           
            //cambiarDestino = true;         
            //continueBtn.GetComponent<Button>().interactable = false;
        }
      
    }
    private IEnumerator WaitForAudioAndActivateButton()
    {
        // Esperar a que se reproduzca el audio de la penúltima línea
        if (dialogueLines[index].audioClip != null)
        {
            yield return new WaitUntil(() => !GetComponent<AudioSource>().isPlaying);
        }

        // Activar cambiarDestinoBtn
        cambiarDestinoBtn.SetActive(true);

        // Desactivar continueBtn
        continueBtn.GetComponent<Button>().interactable = false;
    }
}
