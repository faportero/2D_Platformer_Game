//using UnityEngine;

//public class ArmSpawner : MonoBehaviour
//{
//    public Animator armAnimator;      // Referencia al Animator del brazo
//    public Transform handBone;        // Hueso de la mano en el brazo
//    private GameObject currentItem;   // Ítem actualmente atachado

//    // Método para spawnear un ítem
//    public void SpawnItem(GameObject itemPrefab)
//    {
//        // Instanciar el ítem y atacharlo al hueso de la mano
//        currentItem = Instantiate(itemPrefab, handBone.position, Quaternion.identity);
//        currentItem.transform.SetParent(handBone);

//        // Iniciar la animación de entrada del brazo
//        armAnimator.SetTrigger("SpawnItem");
//    }

//    // Llamado por la animación al terminar la animación de entrada
//    public void DetachItem()
//    {
//        if (currentItem != null)
//        {
//            currentItem.transform.SetParent(null); // Desatachar el ítem del brazo
//            // Iniciar la animación de salida del brazo
//            armAnimator.SetTrigger("DetachItem");
//        }
//    }

//    // Método para ser llamado por la animación de salida cuando se completa
//    public void OnDetachAnimationComplete()
//    {
//        // Aquí puedes eliminar el ítem si es necesario
//        if (currentItem != null)
//        {
//            Destroy(currentItem);
//            currentItem = null;
//        }
//    }
//}
using System.Collections;
using UnityEngine;

public class ArmSpawner : MonoBehaviour
{
    public Transform handBone; // Hueso de la mano en el brazo
    private GameObject currentItem; // Ítem actualmente atachado

    // Método para spawnear un ítem
    public void SpawnItem(GameObject itemPrefab)
    {
        currentItem = Instantiate(itemPrefab, handBone.position, Quaternion.identity);
        currentItem.transform.SetParent(handBone);

        // Solo para pruebas, el ítem se desatacha inmediatamente
        StartCoroutine(DetachItemAfterDelay());
    }

    private IEnumerator DetachItemAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        DetachItem();
    }

    public void DetachItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            Destroy(currentItem);
            currentItem = null;
        }
    }
}

