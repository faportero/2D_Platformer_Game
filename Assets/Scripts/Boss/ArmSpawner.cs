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
    public float initialTimeBeforeRestart; // Tiempo inicial de espera antes de reiniciar la animación
    public Transform levelEnd; // Posición final del nivel

    private float velocityX = 0.0f; // Velocidad para el suavizado del movimiento en X
    private float initialY; // Posición inicial en Y del brazo
    private PlayerMovementNew playerMovementNew; // Referencia al movimiento del jugador
    private GameObject currentItem; // Ítem actualmente atachado
    private Animator animator;
    private Coroutine animateYPosition;
    private float timeBeforeRestart;

    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        initialY = transform.position.y; // Guarda la posición inicial en Y
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animateYPosition = StartCoroutine(AnimateYPosition());
    }

    private void Update()
    {
        //// Movimiento suave del brazo en el eje X, siguiendo al jugador
        //float targetX = playerMovementNew.transform.position.x;
        //float smoothX = Mathf.SmoothDamp(transform.position.x, targetX + 20.0f, ref velocityX, smoothTime);
        //transform.position = new Vector3(smoothX, transform.position.y, 0f);

        // Calcula la distancia entre el jugador y el final del nivel solo en el eje X
     //   float distanceToEnd = Vector3.Distance(new Vector3(playerMovementNew.transform.position.x, 0, 0), new Vector3(levelEnd.position.x, 0, 0));

        // Ajusta el tiempo de espera antes de reiniciar la animación
       // timeBeforeRestart = initialTimeBeforeRestart * (distanceToEnd / 100.0f);
       // print("Time:" + timeBeforeRestart + ". Distance "+ distanceToEnd);
       // timeBeforeRestart = Mathf.Clamp(timeBeforeRestart, 0.01f, initialTimeBeforeRestart);
       // print("Time2:" + timeBeforeRestart + ". Distance2 "+ distanceToEnd / 100.0f);


    }

    private IEnumerator AnimateYPosition()
    {
        timeBeforeRestart = initialTimeBeforeRestart;
        while (true)
        {
            // Verifica si la velocidad en X del jugador es 0
            while (playerMovementNew.rb.velocity.x == 0)
            {
                // Espera un frame antes de volver a comprobar
                yield return null;
            }
            //// Calcula la distancia entre el jugador y el final del nivel
            //float distanceToEnd = Vector3.Distance(new Vector3(playerMovementNew.transform.position.x,0,0), new Vector3(levelEnd.position.x,0,0));

            //// Ajusta el tiempo de espera antes de reiniciar la animación
            //timeBeforeRestart = initialTimeBeforeRestart * (distanceToEnd / 100.0f);
            //timeBeforeRestart = initialTimeBeforeRestart - .01f;
            //if (timeBeforeRestart <= 0.1f) timeBeforeRestart = 0.1f;
            //timeBeforeRestart = Mathf.Clamp(initialTimeBeforeRestart,2,0.1f);
            //timeBeforeRestart = Mathf.Clamp(timeBeforeRestart, 0.01f, initialTimeBeforeRestart);
            //print("Time2:" + timeBeforeRestart + ". Distance2 "+ distanceToEnd);

            // Comprueba si el brazo ya tiene un ítem atachado
            if (currentItem != null)
            {
                // Espera a que el jugador se mueva antes de continuar
                while (playerMovementNew.rb.velocity.x == 0)
                {
                    yield return null;
                }

                // Continúa con la animación, pero no spawnea un nuevo ítem
                // Animación de entrada: mueve el brazo desde su posición inicial hasta la nueva posición
                float yOffset = yPositions[Random.Range(0, yPositions.Count)];
                yield return StartCoroutine(AnimateToPosition(initialY, yOffset, entryAnimationDuration));

                // Espera en la nueva posición
                yield return new WaitForSeconds(waitTime);

                // Suelta el ítem en el mundo solo si el jugador se está moviendo
                if (playerMovementNew.rb.velocity.x != 0)
                {
                    ReleaseItem();
                    animator.SetBool("Attach", false);
                }

                // Animación de salida: mueve el brazo de regreso a su posición inicial
                yield return StartCoroutine(AnimateToPosition(transform.position.y, initialY, exitAnimationDuration));

                // Espera antes de la siguiente animación
                yield return new WaitForSeconds(timeBeforeRestart);

                // Luego, sigue con el siguiente ciclo
                continue;
            }

            timeBeforeRestart = timeBeforeRestart - .01f;
            if (timeBeforeRestart <= 0.15f) timeBeforeRestart = 0.15f;
            print("Time:" + timeBeforeRestart);

            // Si no tiene un ítem, sigue con el spawn y la animación normalmente
            SpawnAndAttachItem();
            animator.SetBool("Attach", true);

            // Selecciona una nueva posición Y de manera aleatoria desde la lista
            float newYOffset = yPositions[Random.Range(0, yPositions.Count)];

            // Animación de entrada: mueve el brazo desde su posición inicial hasta la nueva posición
            yield return StartCoroutine(AnimateToPosition(initialY, newYOffset, entryAnimationDuration));

            // Espera en la nueva posición
            yield return new WaitForSeconds(waitTime);

            // Suelta el ítem en el mundo solo si el jugador se está moviendo
            if (playerMovementNew.rb.velocity.x != 0)
            {
                ReleaseItem();
                animator.SetBool("Attach", false);
            }

            // Animación de salida: mueve el brazo de regreso a su posición inicial
            yield return StartCoroutine(AnimateToPosition(transform.position.y, initialY, exitAnimationDuration));

            // Espera antes de la siguiente animación
            yield return new WaitForSeconds(timeBeforeRestart);
        }
    }

    private void SpawnAndAttachItem()
    {
        // Genera un número aleatorio entre 0 y 1
        float randomValue = Random.value;

        // Decide si spawnear una plataforma o un ítem basado en el número aleatorio
        if (randomValue < 0.5f)
        {
            // Spawnea una plataforma
            currentItem = SpawnManager.Instance.SpawnPlatform();
        }
        else
        {
            // Spawnea un ítem
            currentItem = SpawnManager.Instance.SpawnItem();
        }

        // Asegúrate de que currentItem no sea null antes de continuar
        if (currentItem != null)
        {
            // Attacha el objeto spawneado al handBone
            currentItem.transform.SetParent(handBone);

            // Coloca el objeto en la posición del handBone y ajusta la rotación
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;

            // Asegura que el ítem siga la rotación del handBone
            currentItem.transform.localRotation = handBone.localRotation;

            // Aplicar corrección de rotación adicional si es necesario
            currentItem.transform.localRotation *= Quaternion.Euler(0, 0, -90); // Ajusta el valor según sea necesario
        }
    }

    private void ReleaseItem()
    {
        if (currentItem != null)
        {
            if (currentItem.transform.GetChild(0).GetComponent<MovePlatforms>())
            {
                currentItem.transform.GetChild(0).GetComponent<MovePlatforms>().enabled = true;
            }
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
