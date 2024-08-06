using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ente : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    private PlayerMovementNew playerMovementNew;
    private SpriteRenderer spriteRenderer;
    private Material material;
    public bool isGargolaEnte;

    private void Awake()
    {
        
    }
    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }
    private void OnEnable()
    {
        if (isGargolaEnte)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            material = spriteRenderer.material;
            StartCoroutine(PlayerSolidify());
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerMovementNew.isMoving = false; // Detener el movimiento
            playerMovementNew.inputsEnabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            playerMovementNew.anim.SetBool("SlowWalk", false); // Desactivar animación de caminar
            StartCoroutine(PlayerSolidify());
            
        }
    }

    private IEnumerator PlayerSolidify()
    {
        AudioManager.Instance.PlaySfx("Solidify");

        float dissolveAmount = 0;
        float duration = 2f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
          //  print(material.GetFloat("_DissolveAmmount"));
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 1
        material.SetFloat("_DissolveAmmount", 0);
        dialoguePanel.SetActive(true);
       
    }
    public void EnteDisolve()
    {
        StartCoroutine(PlayerDisolve());
    }
    private IEnumerator PlayerDisolve()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = .5f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            //print(material.GetFloat("_DissolveAmmount"));
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 1
        material.SetFloat("_DissolveAmmount", 1);
        gameObject.SetActive(false);
        dialoguePanel.SetActive(false);

    }
}
