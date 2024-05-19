using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public  class InputManager : MonoBehaviour
{
    //This is the Text for the Label at the top of the screen
    public static string m_DeviceType;
    public static bool isPC;

    void Awake()
    {
        //Output the device type to the console window
        //Debug.Log("Device type : " + m_DeviceType);

        //Check if the device running this is a console
        if (SystemInfo.deviceType == DeviceType.Console)
        {
            //Change the text of the label
            m_DeviceType = "Console";
        }

        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            m_DeviceType = "Desktop";
            isPC = true;
        }

        //Check if the device running this is a handheld
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            m_DeviceType = "Handheld";
            isPC = false;
        }

        //Check if the device running this is unknown
        if (SystemInfo.deviceType == DeviceType.Unknown)
        {
            m_DeviceType = "Unknown";
        }
       // print(m_DeviceType);
    }
    
}