using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;
    private LobbyManager lobbyManager;

    private PlayerMovementNew playerMovement;

    private int index;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
        lobbyManager = FindAnyObjectByType<LobbyManager>();
        print(playerMovement.anim.GetBool("SlowWalk"));
    }
    private void OnEnable()
    { 
        playerMovement.inputsEnabled = false;
        playerMovement.anim.SetBool("SlowWalk", false);
        print(playerMovement.anim.GetBool("SlowWalk"));
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
    public void OnButtonDown() // Método que se llamará cuando el botón sea presionado
    {
        NextLine();
    }

    public void OnButtonUp() // Método que se llamará cuando el botón sea soltado
    {
        StopAllCoroutines();
        textComponent.text = lines[index];
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            lobbyManager.PaneoCamera();
            //playerMovement.inputsEnabled = true;
            //playerMovement.anim.SetBool("SlowWalk", true);
        }
    }

}
