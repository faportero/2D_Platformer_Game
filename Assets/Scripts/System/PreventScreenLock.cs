using UnityEngine;

public class PreventScreenLock : MonoBehaviour
{
    private void Update()
    {
        //float fps = 1.0f / Time.deltaTime;
        //Debug.Log("Frame Rate: " + fps);
    }
    void Start()
    {
        // Evitar que la pantalla se apague
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Si la aplicación se pone en pausa, permitir que la pantalla se apague
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            // Cuando la aplicación se reanuda, evitar que la pantalla se apague
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    void OnApplicationQuit()
    {
        // Restaurar la configuración del sistema cuando la aplicación se cierra
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }
}
