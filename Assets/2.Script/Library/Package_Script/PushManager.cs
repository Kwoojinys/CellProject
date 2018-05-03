using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        return;
        OneSignal.StartInit("1e10d386-6be4-4fe9-8f98-6c6f9d13eb32")
  .HandleNotificationOpened(HandleNotificationOpened)
  .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
    }

    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {

    }
}
