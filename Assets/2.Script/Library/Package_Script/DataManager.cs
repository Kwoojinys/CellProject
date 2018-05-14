using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Reflection;


public class DataManager : MonoBehaviour
{

    private static DataManager instance;

    public bool Version_Checking = false;
    public static bool Data_Receiving = false;
    public static bool Update_Need = false;

    private List<Sprite> Face_Sprites;

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

    public static List<string> json_Data;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        this.transform.parent = GameObject.Find("Managers").transform;

        CloudConnectorCore.processedResponseCallback.AddListener(ParseData);
    }

    public void Init()
    {
        if (Data_Receiving) return;

        Data_Receiving = true;

        json_Data = new List<string>();

        GetAllDatas(true);
    }

    public void Get_Version(bool runtime)
    {
        if (Version_Checking) return;

        Version_Checking = true;


        CloudConnectorCore.GetTable("Version", runtime);
    }

    public static void GetAllDatas(bool runtime)
    {
        if (Update_Need)
        {
            Debug.Log("<color=yellow>Retrieving all datas from the Cloud.</color>");
            CloudConnectorCore.GetAllTables(runtime);
        }
        else
        {
            GameManager.Instance.m_Loading = Loading_State.Sprite_Loading;
        }
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
                            PlayerPrefs.SetString("Data_Worker", jsonData[i]);
                            break;
                        }
                    case "Version":
                        {
                            //Debug.Log("Current DataTable Version : " + jsonData[i]);
                            //Version_Check(jsonData[i]);
                            break;
                        }
                    case "Soldier":
                        {
                            PlayerPrefs.SetString("Data_Soldier", jsonData[i]);
                            Debug.Log(jsonData[i]);
                            break;
                        }
                    case "Enemy":
                        {
                            PlayerPrefs.SetString("Data_Enemy", jsonData[i]);
                            Debug.Log(jsonData[i]);
                            break;
                        }
                    default:
                        {
                            Debug.Log("Default");
                            break;
                        }
                }
            }

            GameManager.Instance.m_Loading = Loading_State.Sprite_Loading;
        }
        else if (query == CloudConnectorCore.QueryType.getTable)
        {
            switch (objTypeNames[0])
            {
                case "Worker":
                    {
                        PlayerPrefs.SetString("Data_Worker", jsonData[0]);
                        break;
                    }
                case "Version":
                    {
                        Version_Check(jsonData[0]);
                        break;
                    }
                case "Soldier":
                    {
                        PlayerPrefs.SetString("Data_Soldier", jsonData[0]);
                        Debug.Log(jsonData[0]);
                        break;
                    }
                case "Enemy":
                    {
                        PlayerPrefs.SetString("Data_Enemy", jsonData[0]);
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

    public static void Version_Check(string json)
    {
        Debug.Log(json);

        var Data = JSON.Parse(json);

        string Current_Version = PlayerPrefs.GetString("Data_Version");

        string Server_Version = Data[0]["Version"];

        if (!Current_Version.Equals(Server_Version))
        {
            Update_Need = true;
            Debug.Log("Data Table Update.");
            PlayerPrefs.SetString("Data_Version", Server_Version);
        }

        GameManager.Instance.m_Loading = Loading_State.Data_Loading;
    }

    public void Load_Sprite()
    {
        Face_Sprites = new List<Sprite>();

        if (Face_Sprites.Count >= 1) return;

        Sprite[] Sprites = Resources.LoadAll<Sprite>("Faces/");

        Face_Sprites = new List<Sprite>();

        for (int i = 0; i < Sprites.Length; i++)
        {
            Face_Sprites.Add(Sprites[i]);
        }

        GameManager.Instance.m_Loading = Loading_State.UI_Loading;
    }

    public Sprite GetSprite(string name)
    {
        for(int i = 0; i < Face_Sprites.Count;i++)
        {
            if (Face_Sprites[i].name == name)
                return Face_Sprites[i];
        }

        Debug.LogError("Sprite " + name + " Not Found.");
        return null;
    }


}
