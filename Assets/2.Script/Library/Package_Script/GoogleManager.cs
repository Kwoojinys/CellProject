using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleManager : MonoBehaviour {

    private void Start()
    {
        Google_Login();
    }

    public void Google_Login()
    {
        UM_GameServiceManager.Instance.Connect();
        UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
        UM_GameServiceManager.OnPlayerDisconnected += OnPlayerDisconnected;
    }

    public void OnPlayerConnected()
    {
        Debug.Log("OnPlayerConnected");
        Debug.Log("Player Info : " + UM_GameServiceManager.Instance.Player.PlayerId);
    }

    public void OnPlayerDisconnected()
    {
        Debug.Log("OnPlayerDisconnected");
    }
}
