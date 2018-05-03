using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Reflection;


public class DataManager : MonoBehaviour {

    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                GameObject container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
            }

            return instance;
        }
    }

    public static string tableName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        Init();
    }

    public void Init()
    {
        CloudConnectorCore.processedResponseCallback.AddListener(ParseData);

        GetAllPlayers(true);
    }

    public static void GetAllPlayers(bool runtime)
    {
        Debug.Log("<color=yellow>Retrieving all players from the Cloud.</color>");

        tableName = "Worker";

        // Get all objects from table 'PlayerInfo'.
        CloudConnectorCore.GetAllTables(runtime);
    }

    public static void Get_Worker_Data(string json)
    {

    }

    public void Worker_Data()
    {

    }

    public static void ParseData(CloudConnectorCore.QueryType query, List<string> objTypeNames, List<string> jsonData)
    {
        for (int i = 0; i < objTypeNames.Count; i++)
        {
            Debug.Log("Data type/table: " + objTypeNames[i]);
        }

        // First check the type of answer.
        if (query == CloudConnectorCore.QueryType.getAllTables)
        {
            // Just dump all content to the console, sorted by table name.
            for (int i = 0; i < objTypeNames.Count; i++)
            {
                switch (objTypeNames[i])
                {
                    case "Worker":
                        {
                            Get_Worker_Data(jsonData[i]);
                            break;
                        }
                    case "Version":
                        {
                            Debug.Log("Current DataTable Version : " + jsonData[i]);
                            Version_Check(jsonData[i]);
                            break;
                        }
                    default:
                        {
                            Debug.Log("Default");
                            break;
                        }
                }
            }
        }
    }

    public static void Version_Check(string json)
    {
        Debug.Log(json);
    }


}
