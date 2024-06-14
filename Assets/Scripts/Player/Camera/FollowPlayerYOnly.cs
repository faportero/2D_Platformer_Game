using UnityEngine;
using Cinemachine;

public class FollowPlayerYOnly : MonoBehaviour
{
    public Transform player; // Asigna el jugador aquí
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

    void Start()
    {
        if(virtualCamera != null) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null) transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Configura el offset inicial
        if (virtualCamera != null) transposer.m_FollowOffset = new Vector3(0, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);
    }
    private void OnEnable()
    {
        if (virtualCamera != null) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null) transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (virtualCamera != null) transposer.m_FollowOffset = new Vector3(0, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);
        
    }
    void Update()
    {
        if (player != null && virtualCamera != null)
        {
            // Actualiza el offset solo en el eje Y
            Vector3 offset = transposer.m_FollowOffset;
            offset.y = player.position.y;
            transposer.m_FollowOffset = offset;
        }
    }
}
