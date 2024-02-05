using System;
using UnityEngine;

public class SwitchExitHandling
{
#if UNITY_SWITCH
    static void YourSystemMessageHandler(UnityEngine.Switch.Notification.Message message)
    {
        if (message == UnityEngine.Switch.Notification.Message.ExitRequest)
        {
            // Write save data, etc.
            SaveBridge.SaveOnQuit();
            UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();
        UnityEngine.Switch.Notification.notificationMessageReceived += YourSystemMessageHandler;
    }
#endif
}