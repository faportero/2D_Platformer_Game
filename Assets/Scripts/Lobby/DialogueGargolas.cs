using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    private bool wasPreviousPlayerSpeaking;
    private bool firstTime = true;

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
       // print("FirstTimeDialogue: " + firstTime);
        //if (textComponent.text == dialogueLines[index].line) continueBtn.GetComponent<Button>().interactable = false;
        //else continueBtn.GetComponent<Button>().interactable = true;
        //if (textComponent.text == dialogueLines[0].line) backBtn.GetComponent<Button>().interactable = false;
        //else backBtn.GetComponent<Button>().interactable = true;
    }

    public void OnButtonDown()
    {
        //   if (autoAdvanceDialogue != null) StopCoroutine(autoAdvanceDialogue);

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
            textComponent.text = string.Empty;
            typeLineCoroutine = StartCoroutine(TypeLine());
        }
    }
    public void OnChangeButtonDown()
    {
        //  if (autoAdvanceDialogue != null) StopCoroutine(autoAdvanceDialogue);

       // gameObject.SetActive(false);
       GetComponent<AudioSource>().Stop();
        espejo.StartCoroutine(espejo.AnimacionGargolas());
        playerMovement.isFacingRight = true;
        playerMovement.Turn();
        //autoAdvanceDialogue = StartCoroutine(AutoAdvanceDialogue());
        // NextLine();
        StartCoroutine(TypeLastLine());


        // lobbyManager.PaneoCamera();
        //if (levelManager.currentScene == LevelManager.CurrentScene.Lobby) lobbyManager.PaneoCamera();
        // else if (levelManager.currentScene == LevelManager.CurrentScene.Limbo) ;
    }

    private IEnumerator TypeLastLine()
    {
        backBtn.GetComponent<Button>().interactable = false;
       // continueBtn.GetComponent<Button>().interactable = false;

        // backBtn.SetActive(false);
        //conrinueBtn.SetActive(false);
        yield return new WaitForSeconds(2); 
        index++;
        typeLineCoroutine = StartCoroutine(TypeLine());
        yield return new WaitForSecondsRealtime(11);
        GetComponent<Animator>().Play("Hide Animation");
        ente.EnteDisolve();
        playerMovement.isMoving = true;
        playerMovement.inputsEnabled = true;
       // gameObject.SetActive(false);
        
    }
    private void StartDialogue()
    {
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
        }
        firstTime = false;
    }
    private IEnumerator TypeLine()
    {
        // continueBtn.SetActive(false);
        // backBtn.SetActive(false);
        // Actualiza la imagen del personaje
        characterImage.sprite = dialogueLines[index].characterImage;

        if (dialogueLines[index].audioClip != null)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(dialogueLines[index].audioClip);
        }

        // Alineación de la imagen y el texto según quien hable
        if (dialogueLines[index].isPlayerSpeaking)
        {
            // Alinea la imagen a la derecha y el texto a la izquierda de la imagen
            characterImage.rectTransform.anchorMin = new Vector2(0.7901939f, 0.2722537f);
            characterImage.rectTransform.anchorMax = new Vector2(0.8766842f, 0.7227457f);
            textComponent.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            // Alinea la imagen a la izquierda y el texto a la derecha de la imagen
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
        if (index < dialogueLines.Count - 2)
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
            // Aquí termina los diálogos
            StopCoroutine(autoAdvanceDialogue);

            // Modificaciones
            // if (index == dialogueLines.Count - 1) // Si estamos en la penúltima línea
            // {
            //   StartCoroutine(WaitForAudioAndActivateButton());
            //}
            cambiarDestinoBtn.SetActive(true);
           // backBtn.SetActive(true);

            // Desactivar continueBtn
            continueBtn.GetComponent<Button>().interactable = false;
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
