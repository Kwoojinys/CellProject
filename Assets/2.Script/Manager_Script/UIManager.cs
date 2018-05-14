using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Menu_Btns
{
    Soldier = 0,
    Worker = 1,
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UIManager").AddComponent<UIManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public Text Stage_Text;
    public Text Gold_Text;
    public Transform Workers;
    public Transform Units;
    public ScrollRect WorkersView;
    public ScrollRect UnitsView;
    public List<GameObject> Worker_Objs;
    public List<GameObject> Unit_Objs;
    public List<Worker_Controller> Worker_Controllers;
    public List<UnitController> Unit_Controllers;

    public GameObject[] Menu_Panels;

    public Transform All_LvupMenu;
    public Transform[] All_LevelUpBtns;

    public Transform EntryMenu;
    public Transform[] EntryUnits;

    public int[] TeamUnits;

    public Image Block_UI;

    public bool Entry_ChangeMode;

    public int Target_Team;
    public int Target_EntryID;

    public void Refresh_Gold(float gold)
    {
        Gold_Text.text = GameManager.Instance.ChangeValue(gold.ToString());

        for (int i = 0; i < Worker_Controllers.Count; i++)
        {
            Worker_Controllers[i].Refresh_Avaliable();
        }

        for (int i = 0; i < Unit_Controllers.Count; i++)
        {
            Unit_Controllers[i].Refresh_Avaliable();
        }

        Set_AllLvupTexts();
    }

    private void Start()
    {
        GameManager.Instance.m_Loading = Loading_State.UI_LoadComplete;

        TeamUnits = new int[5];
    }

    public void Init()
    {
        List<Worker_Info> Worker_List = WorkerManager.Instance.Worker_List;
        Worker_Controllers = new List<Worker_Controller>();

        GameObject WorkerObj = Resources.Load("Worker_Child") as GameObject;

        for (int i = 0; i < Worker_List.Count; i++)
        {
            GameObject Worker = Instantiate(WorkerObj, Workers) as GameObject;
            Worker.transform.localPosition = new Vector2(0, 0);
            Workers.GetComponent<RectTransform>().sizeDelta = new Vector2(704, i * 150);
            Worker_Objs.Add(Worker);

            Worker.GetComponent<Worker_Controller>().Set_Info(Worker_List[i]);
            Worker.GetComponent<Worker_Controller>().Refresh_Info();

            Worker_Controllers.Add(Worker.GetComponent<Worker_Controller>());
        }

        WorkersView.verticalNormalizedPosition = 1f; // 스크롤뷰 위치 초기화

        List<UnitControl> Unit_List = UnitDataManager.Instance.PlayerSpawnUnitList;
        Unit_Controllers = new List<UnitController>();
        GameObject UnitObj = Resources.Load("Unit_Child") as GameObject;
        for (int i = 0; i < Unit_List.Count; i++)
        {
            GameObject Unit = Instantiate(UnitObj, Units) as GameObject;
            Unit.transform.localPosition = new Vector2(0, 0);
            Units.GetComponent<RectTransform>().sizeDelta = new Vector2(704, i * 130);
            Unit_Objs.Add(Unit);

            Unit.GetComponent<UnitController>().Set_Info(Unit_List[i]);
            Unit.GetComponent<UnitController>().Refresh_Info();

            Unit_Controllers.Add(Unit.GetComponent<UnitController>());
        }

        UnitsView.verticalNormalizedPosition = 1f; // 스크롤뷰 위치 초기화

        Open_Menu(0);
    }

    public void Open_Menu(int id)
    {
        Menu_Btns Menu = (Menu_Btns)id;

        for (int i = 0; i < Menu_Panels.Length; i++)
        {
            if (i == id)
            {
                Menu_Panels[i].SetActive(true);
            }
            else
            {
                Menu_Panels[i].SetActive(false);
            }
        }

        switch (Menu)
        {
            case Menu_Btns.Soldier:
                {
                    Debug.Log("Soldier Menu Open");
                    UnitsView.verticalNormalizedPosition = 1f; // 스크롤뷰 위치 초기화
                    break;
                }
            case Menu_Btns.Worker:
                {
                    Debug.Log("Worker Menu Open");
                    WorkersView.verticalNormalizedPosition = 1f;
                    break;
                }

        }
    }

    public void Switch_AllLvupMenu()
    {
        All_LvupMenu.gameObject.SetActive(!All_LvupMenu.gameObject.activeSelf);

        Set_AllLvupTexts();
    }

    public void Switch_EntryMenu()
    {
        EntryMenu.gameObject.SetActive(!EntryMenu.gameObject.activeSelf);

        Change_Team(0);

        for (int i = 0; i < Unit_Objs.Count; i++)
        {
            Unit_Objs[i].GetComponent<UnitController>().Off_TeamMode();
        }
    }

    public void Set_AllLvupTexts()
    {
        for (int i = 0; i < 4; i++)
        {
            All_LevelUpBtns[i].GetChild(1).GetComponent<Text>().text = GameManager.Instance.ChangeValue(UnitDataManager.Instance.Unit_Upgolds[i].ToString());

            if (UnitDataManager.Instance.Unit_Upgolds[i] > GameManager.Instance.User.Have_gold)
            {
                All_LevelUpBtns[i].GetComponent<Button>().enabled = false;
                All_LevelUpBtns[i].GetComponent<Image>().color = Color.gray;
            }
            else
            {
                All_LevelUpBtns[i].GetComponent<Button>().enabled = true;
                All_LevelUpBtns[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void Request_All_LevelUp(int level_tier)
    {
        UnitDataManager.Instance.All_LevelUp(level_tier);

        Set_AllLvupTexts();
    }

    public void Change_Team(int team_number)
    {
        TeamUnits = UnitDataManager.Instance.TeamUnit_ids[team_number];

        Target_Team = team_number;

        Target_EntryID = -1;
    }

    public void OnClick_EntryUnit(int id)
    {
        Entry_ChangeMode = true;

        for (int i = 0; i < Unit_Objs.Count; i++)
        {
            Unit_Objs[i].GetComponent<UnitController>().Change_TeamMode();
        }

        Target_EntryID = id;

        Debug.Log(TeamUnits[id]);
    }

    public void Change_EntryUnit(UnitControl Target)
    {
        Target.battle_team = Target_Team;

        UnitDataManager.Instance.TeamUnit_ids[Target_Team][Target_EntryID] = Target.unit_id;
        TeamUnits[Target_EntryID] = Target.unit_id;

        Debug.Log("Team " + Target_Team + "'s " + Target_EntryID + " Unit Id : " + UnitDataManager.Instance.TeamUnit_ids[Target_Team][Target_EntryID]);

        for (int i = 0; i < Unit_Objs.Count; i++)
        {
            Unit_Objs[i].GetComponent<UnitController>().Off_TeamMode();
        }
    }

    public void Change_BattleTeam(int teamnumber)
    {
        UnitSpawnManager.Instance.Selected_Team_Number = teamnumber;
    }
}
