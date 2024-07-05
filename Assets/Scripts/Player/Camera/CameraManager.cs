using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
 public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for lerping Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = .25f;
    [SerializeField] private float fallPanTime = .35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    public bool isLerpingYDamping {  get; private set; }
    public bool LerpedFromPlayerFalling {  get; set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    public CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    [SerializeField] private CinemachineBlenderSettings blend;

    private Vector2 statingTrackedObjectOffset;

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

        statingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
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
        float endDampAmount = 0;

        if(isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }

        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float LerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallPanTime));
            framingTransposer.m_YDamping = LerpedPanAmount;

            yield return null;
        }
        isLerpingYDamping = false;
    }

    #endregion

    #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos  = Vector2.zero; 
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }

            endPos *= panDistance;
            startingPos = statingTrackedObjectOffset;
            endPos += startingPos;
        }

        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = statingTrackedObjectOffset;
        }

        float elapsedTime = 0;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }
    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        print(triggerExitDirection);
        print(currentCamera);
        if (currentCamera == cameraFromLeft && triggerExitDirection.y < 0 && triggerExitDirection.x < 0)
        {
            cameraFromLeft.enabled = false;
            cameraFromRight.enabled = true;

            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        else if (currentCamera == cameraFromRight && triggerExitDirection.y > 0)
        {
            cameraFromLeft.enabled = true;
            cameraFromRight.enabled = false;

            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        else if (currentCamera == cameraFromLeft && triggerExitDirection.x > 0 && triggerExitDirection.y < 0)
        {
            cameraFromLeft.enabled = false;
            cameraFromRight.enabled = true;

            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        else if (currentCamera == cameraFromRight && triggerExitDirection.x < 0) 
        {
            cameraFromLeft.enabled = true;
            cameraFromRight.enabled = false;

            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
    public void SingleSwapCamera(CinemachineVirtualCamera newCamera)
    {
        if (currentCamera != null)
        {
            currentCamera.enabled = false;
            //currentCamera.GetComponent<CinemachineBrain>().m_CustomBlends = blend;
        }   
        newCamera.enabled = true;
        currentCamera = newCamera;
        framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
       
    }
    #endregion
}
