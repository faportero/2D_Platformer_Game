using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class DistanceTrackerOld : MonoBehaviour
{
    public List<Transform> points = new List<Transform>(); // Lista de puntos
    public Transform player;
    public Color lineColor = Color.red; // Color de la línea
    public float totalDistance { get; private set; }
    public float playerProgress { get; private set; } // Valor entre 0 y 1

    private void Start()
    {
        CalculateTotalDistance();
    }

    private void Update()
    {
        CalculatePlayerProgress();
        //Debug.Log("Progreso: " + playerProgress);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        totalDistance = 0f;

        // Dibujar línea entre puntos
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
                totalDistance += Vector3.Distance(points[i].position, points[i + 1].position);
            }
        }

        // Mostrar distancia total en el editor
        Gizmos.color = Color.white;
        if (transform.position != null)
        {
            Gizmos.DrawCube(transform.position, Vector3.one * 0.1f); // Pequeño marcador en el GameObject
            Handles.Label(transform.position, "Total Distance: " + totalDistance.ToString("F2") + " units");
        }
    }

    private void CalculateTotalDistance()
    {
        totalDistance = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                totalDistance += Vector3.Distance(points[i].position, points[i + 1].position);
            }
        }
    }

    private void CalculatePlayerProgress()
    {
        float currentDistance = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Vector3 segmentStart = points[i].position;
                Vector3 segmentEnd = points[i + 1].position;
                Vector3 playerPosition = player.position;

                // Proyectar la posición del jugador en la línea segmento
                Vector3 segmentDirection = segmentEnd - segmentStart;
                Vector3 projection = Vector3.Project(playerPosition - segmentStart, segmentDirection) + segmentStart;

                // Asegurarse de que la proyección está dentro del segmento
                if (Vector3.Dot(projection - segmentStart, segmentDirection) >= 0 && Vector3.Dot(projection - segmentEnd, -segmentDirection) >= 0)
                {
                    currentDistance += Vector3.Distance(segmentStart, projection);
                    break;
                }
                else
                {
                    currentDistance += Vector3.Distance(segmentStart, segmentEnd);
                }
            }
        }

        playerProgress = currentDistance / totalDistance;
    }
}
