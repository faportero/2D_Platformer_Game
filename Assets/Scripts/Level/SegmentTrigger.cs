using UnityEngine;

[System.Serializable]
public class SegmentTrigger : MonoBehaviour
{
    public int segmentIndex; // Índice del segmento que este trigger representa
    public Vector3 newDirection; // Nueva dirección al atravesar este trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí puedes añadir lógica específica si es necesario
        }
    }
}   