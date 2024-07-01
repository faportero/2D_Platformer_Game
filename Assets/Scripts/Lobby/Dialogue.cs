using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueLine
{
    public string line;
    public Sprite characterImage;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] List<DialogueLine> dialogueLines;
    [SerializeField] float textSpeed;
    [SerializeField] Image characterImage; // El Image donde se mostrará la imagen del personaje

    private LobbyManager lobbyManager;
    private PlayerMovementNew playerMovement;
    private int index;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
        lobbyManager = FindAnyObjectByType<LobbyManager>();
    }

    private void OnEnable()
    {
        playerMovement.inputsEnabled = false;
        playerMovement.anim.SetBool("SlowWalk", false);
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
            StopAllCoroutines();
            textComponent.text = dialogueLines[index].line;
        }
    }

    public void OnButtonUp()
    {
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        // Actualiza la imagen del personaje
        characterImage.sprite = dialogueLines[index].characterImage;

        // Muestra el diálogo carácter por carácter
        textComponent.text = string.Empty;
        foreach (char c in dialogueLines[index].line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < dialogueLines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            lobbyManager.PaneoCamera();
        }
    }
}
