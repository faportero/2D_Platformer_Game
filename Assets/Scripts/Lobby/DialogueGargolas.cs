using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] GameObject continueBtn;
    private LobbyManager lobbyManager;
    private PlayerMovementNew playerMovement;
    public int index;
    public int countDialogue;
   

    private Coroutine blinkCoroutine; // Corrutina para el efecto de "pestañeo"
    [HideInInspector] public Coroutine typeLineCoroutine;

    private bool wasPreviousPlayerSpeaking;

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

        }
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
    }

    private IEnumerator TypeLine()
    {
        continueBtn.SetActive(false);
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
        continueBtn.SetActive(true);

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
            gameObject.SetActive(false);
            //lobbyManager.PaneoCamera();
        }
    }
}
