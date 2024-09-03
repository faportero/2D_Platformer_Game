using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;
    [Header("Controls for lerping Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = .25f;
    [SerializeField] private float fallPanTime = .35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    public bool isLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;
    private Coroutine shakeCoroutine;
    
    public bool isShaking = false;
    public CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private Vector3 startingTrackedObjectOffset;
    private float normYPanAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        normYPanAmount = framingTransposer.m_YDamping;
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    #region Lerp the Y Damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;
        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = isPlayerFalling ? fallPanAmount : normYPanAmount;

        float elapsedTime = 0;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;
            yield return null;
        }
        isLerpingYDamping = false;
    }
    #endregion

    #region Pan Camera
    public void PanCameraOnContact(Vector3 panOffset, float panTime, bool panToStartingPos)
    {
        if (panCameraCoroutine != null)
        {
            StopCoroutine(panCameraCoroutine);
        }
        panCameraCoroutine = StartCoroutine(PanCamera(panOffset, panTime, panToStartingPos));
    }

    private IEnumerator PanCamera(Vector3 panOffset, float panTime, bool panToStartingPos)
    {
        Vector3 startPos = framingTransposer.m_TrackedObjectOffset;
        Vector3 endPos = panToStartingPos ? startingTrackedObjectOffset : startPos + panOffset;

        float elapsedTime = 0;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(startPos, endPos, elapsedTime / panTime);
            yield return null;
        }
    }
    #endregion

    #region Swap Cameras
    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if (currentCamera == cameraFromLeft && triggerExitDirection.x > 0)
        {
            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;
            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if (currentCamera == cameraFromRight && triggerExitDirection.x < 0)
        {
            cameraFromLeft.enabled = true;
            cameraFromRight.enabled = false;
            currentCamera = cameraFromLeft;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if (currentCamera == cameraFromLeft && triggerExitDirection.y < 0)
        {
            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;
            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void SingleSwapCamera(CinemachineVirtualCamera newCamera, float time)
    {
        if (currentCamera != null)
        {
            currentCamera.enabled = false;
        }

        newCamera.enabled = true;
        currentCamera = newCamera;
        framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (cinemachineBrain != null)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = time;
        }
        else
        {
            Debug.LogWarning("CinemachineBrain no encontrado en la cámara principal (Main Camera). Asegúrate de que la cámara principal tenga un componente CinemachineBrain.");
        }
    }
    #endregion
    public void StartCameraShake(float amplitude)
    {
        cinemachineBasicMultiChannelPerlin = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        isShaking = true;
        shakeCoroutine = StartCoroutine(CameraShake(amplitude));
    }

    public void StopCameraShake()
    {
        isShaking = false;
    }

    private IEnumerator CameraShake(float amplitude)
    {
        while (isShaking)
        {
            cinemachineBasicMultiChannelPerlin = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.PingPong(Time.time * amplitude, amplitude);
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        shakeCoroutine = null; 
    }
}

