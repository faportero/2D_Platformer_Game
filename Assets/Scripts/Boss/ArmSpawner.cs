using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpawner : MonoBehaviour
{
    public Transform handBone; // Hueso de la mano en el brazo
    public List<float> yPositions; // Lista de posiciones en el eje Y definidas en el inspector
    public float smoothTime = 0.3f; // Tiempo de suavizado para el movimiento en X
    public float entryAnimationDuration = 1.0f; // Duración de la animación de entrada
    public float exitAnimationDuration = 1.0f; // Duración de la animación de salida
    public float waitTime = 0.5f; // Tiempo que el brazo permanece en la nueva posición
    public float timeBeforeRestart = 1.0f; // Tiempo de espera antes de reiniciar la animación

    private float velocityX = 0.0f; // Velocidad para el suavizado del movimiento en X
    private float initialY; // Posición inicial en Y del brazo
    private PlayerMovementNew playerMovementNew; // Referencia al movimiento del jugador
    private GameObject currentItem; // Ítem actualmente atachado

    private Animator animator;
    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        initialY = transform.position.y; // Guarda la posición inicial en Y
        animator = GetComponent<Animator>();    
    }

    private void Start()
    {
        StartCoroutine(AnimateYPosition());
    }

    private void Update()
    {
        // Movimiento suave del brazo en el eje X, siguiendo al jugador
        float targetX = playerMovementNew.transform.position.x;
        float smoothX = Mathf.SmoothDamp(transform.position.x, targetX + 20.0f, ref velocityX, smoothTime);

        transform.position = new Vector3(smoothX, transform.position.y, 0f);
    }

    private IEnumerator AnimateYPosition()
    {
        while (true)
        {
            // Spawnea el objeto y lo atacha al handBone
            SpawnAndAttachItem();
            animator.SetBool("Attach", true);
            // Selecciona una nueva posición Y de manera aleatoria desde la lista
            float yOffset = yPositions[Random.Range(0, yPositions.Count)];

            // Animación de entrada: mueve el brazo desde su posición inicial hasta la nueva posición
            yield return StartCoroutine(AnimateToPosition(initialY, yOffset, entryAnimationDuration));

            // Espera en la nueva posición
            yield return new WaitForSeconds(waitTime);

            // Suelta el item en el mundo
            ReleaseItem();
            animator.SetBool("Attach", false);

            // Animación de salida: mueve el brazo de regreso a su posición inicial
            yield return StartCoroutine(AnimateToPosition(transform.position.y, initialY, exitAnimationDuration));

            // Espera antes de la siguiente animación
            yield return new WaitForSeconds(timeBeforeRestart);
        }
    }

    private void SpawnAndAttachItem()
    {
        // Spawnea el objeto desde el SpawnManager y lo asigna a currentItem
        currentItem = SpawnManager.Instance.SpawnItem();

        if (currentItem != null)
        {
            // Attacha el objeto spawneado al handBone
            currentItem.transform.SetParent(handBone);

            // Coloca el objeto en la posición del handBone y ajusta la rotación
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;

            // Asegura que el ítem siga la rotación del handBone
            // Ajusta la rotación local para corregir cualquier desalineación
            currentItem.transform.localRotation = handBone.localRotation;

            // Aplicar corrección de rotación adicional si es necesario
            // Por ejemplo, si los ítems están rotados 90 grados en Z, puedes aplicar una corrección
            currentItem.transform.localRotation *= Quaternion.Euler(0, 0, -90); // Ajusta el valor según sea necesario
        }
    }


    private void ReleaseItem()
    {
        if (currentItem != null)
        {
            // Desatacha el objeto del handBone
            currentItem.transform.SetParent(null);

            // El objeto ya no está atachado y permanece en el mundo en la posición final
            currentItem = null;
        }
    }

    private IEnumerator AnimateToPosition(float startY, float endY, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);

            // Actualiza la posición del objeto en el eje Y
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        // Asegúrate de que la posición final sea exacta
        transform.position = new Vector3(transform.position.x, endY, transform.position.z);
    }
}
