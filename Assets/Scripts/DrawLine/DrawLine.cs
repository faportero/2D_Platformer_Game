using UnityEngine;

[ExecuteInEditMode]
public class DrawLineInEditor : MonoBehaviour
{
    public float lineLength = 5f; // Longitud de la línea
    public Color lineColor = Color.red; // Color de la línea

    // Este método dibuja la línea en el editor
    void OnDrawGizmos()
    {
        // Establecer el color de la línea
        Gizmos.color = lineColor;

        // Calcular la posición inicial (centro del objeto) y la posición final (hacia abajo)
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.down * lineLength;

        // Dibujar la línea
        Gizmos.DrawLine(startPosition, endPosition);

        // Dibujar un pequeño cubo en la posición final para mejor visualización
        Gizmos.DrawWireCube(endPosition, Vector3.one * 0.1f);
    }
}
