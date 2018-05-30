using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Menu_Btns
{
    World_Map = 0,
    Worker = 1,
    Soldier = 2,
    Gene = 3,
    Shop = 4,
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
    public Transform MyUnits;

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

    public Image Ud_Sprite;
    public Text[] Ud_Texts;

    public Transform Entry_TeamBtns;
    public Transform Battle_TeamBtn;

    private int Target_Unit_Id = -1;
    private int Selected_Elemental = 0;

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
            Workers.GetComponent<RectTransform>().sizeDelta = new Vector2(704, (i + 1) * 130);
            Worker_Objs.Add(Worker);

            Worker.GetComponent<Worker_Controller>().Set_Info(Worker_List[i]);
            Worker.GetComponent<Worker_Controller>().Refresh_Info();

            Worker_Controllers.Add(Worker.GetComponent<Worker_Controller>());
        }

        WorkersView.verticalNormalizedPosition = 1f; // 스크롤뷰 위치 초기화

        List<Unit_Stat> Unit_List = UnitDataManager.Instance.PlayerSpawnUnitList;
        Unit_Controllers = new List<UnitController>();
        GameObject UnitObj = Resources.Load("Unit_Child") as GameObject;
        Units.GetComponent<RectTransform>().sizeDelta = new Vector2(704, 650);

        for (int i = 0; i < Unit_List.Count; i++)
        {
            GameObject Unit = Instantiate(UnitObj, Units) as GameObject;
            Unit.transform.localPosition = new Vector2(0, 0);
            Unit_Objs.Add(Unit);

            Unit.GetComponent<UnitController>().Set_Info(Unit_List[i]);
            Unit.GetComponent<UnitController>().Refresh_Info();

            Unit_Controllers.Add(Unit.GetComponent<UnitController>());
        }

        UnitsView.verticalNormalizedPosition = 1f; // 스크롤뷰 위치 초기화

        Open_Menu((int)Menu_Btns.Soldier);

        Change_BattleTeam(PlayerPrefs.GetInt("BattleTeam", 0));
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
        if (Entry_ChangeMode)
        {
            Target_Unit_Id = -1;

            Entry_ChangeMode = false;

            Ud_Sprite.transform.parent.gameObject.SetActive(false);
            Entry_TeamBtns.gameObject.SetActive(true);

            return;
        }

        EntryMenu.gameObject.SetActive(!EntryMenu.gameObject.activeSelf);

        Change_Team(0);

        if (EntryMenu.gameObject.activeSelf)
        {
                for (int i = 0; i < Unit_Objs.Count; i++)
                {
                    Unit_Objs[i].GetComponent<UnitController>().Off_TeamMode();
                    Unit_Objs[i].transform.parent = MyUnits;
                    Unit_Objs[i].gameObject.SetActive(true);
                    Unit_Objs[i].GetComponent<UnitController>().Change_TeamMode();
                }
        } else
        {
            for (int i = 0; i < Unit_Objs.Count; i++)
            {
                Unit_Objs[i].GetComponent<UnitController>().Off_TeamMode();
                Unit_Objs[i].transform.parent = Units;
                Unit_Objs[i].GetComponent<UnitController>().Switch_By_Team(UnitSpawnManager.Instance.Selected_Team_Number);
            }
        }

        MyUnits.GetComponent<RectTransform>().sizeDelta = new Vector2(704, Unit_Objs.Count * 130);
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

        Refresh_Team_Sprite();

        for (int i = 0; i < Unit_Objs.Count; i++)
            Unit_Objs[i].GetComponent<UnitController>().Change_TeamMode();

        for (int i = 0; i < Entry_TeamBtns.childCount; i++)
            if (i == team_number)
                Entry_TeamBtns.GetChild(i).GetComponent<Image>().color = Color.cyan;
            else
                Entry_TeamBtns.GetChild(i).GetComponent<Image>().color = Color.gray;
    }

    public void Refresh_Team_Sprite()
    {
        for (int j = 0; j < UnitDataManager.Instance.TeamUnit_ids[Target_Team].Length; j++)
        {
            if (UnitDataManager.Instance.TeamUnit_ids[Target_Team][j] == -1)
            {
                EntryUnits[j].GetChild(0).GetComponent<Image>().sprite = null;
                continue;
            }

            Unit_Stat Target = UnitDataManager.Instance.PlayerSpawnUnitList.Find(x => x.unit_id == UnitDataManager.Instance.TeamUnit_ids[Target_Team][j]);
            EntryUnits[j].GetChild(0).GetComponent<Image>().sprite = DataManager.Instance.GetSprite(Target.face_sprite);
        }
    }

    public void OnClick_EntryUnit(int id)
    {
        if (Target_Unit_Id == -1) return;

        Target_EntryID = id;

        Unit_Stat Target = UnitDataManager.Instance.PlayerSpawnUnitList.Find(x => x.unit_id == Target_Unit_Id);
        if (Target.battle_team != -1)
        {
            UnitDataManager.Instance.Team_Entry_Out(Target.unit_id, Target.battle_team);
            Refresh_Team_Sprite();
        }

        Target.battle_team = Target_Team;

        UnitDataManager.Instance.TeamUnit_ids[Target_Team][Target_EntryID] = Target.unit_id;
        TeamUnits[Target_EntryID] = Target.unit_id;

        Debug.Log("Team " + Target_Team + "'s " + Target_EntryID + " Unit Id : " + UnitDataManager.Instance.TeamUnit_ids[Target_Team][Target_EntryID]);

        for (int i = 0; i < Unit_Objs.Count; i++)
        {
            Unit_Objs[i].GetComponent<UnitController>().Change_TeamMode();
        }

        Refresh_Team_Sprite();
        Target_Unit_Id = -1;
        Ud_Sprite.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 교체화면에서 유닛 선택시 발동
    /// </summary>
    /// <param name="Target"></param>
    public void Change_EntryUnit(Unit_Stat Target)
    {
        Entry_ChangeMode = true;

        Target_Unit_Id = Target.unit_id;

        Ud_Sprite.transform.parent.gameObject.SetActive(true);

        Ud_Sprite.sprite = DataManager.Instance.GetSprite(Target.face_sprite);
        Ud_Texts[0].text = Target.Name;
        Ud_Texts[1].text = Target.Tier_UI();
        Ud_Texts[2].text = Target.Type_UI();

        Entry_TeamBtns.gameObject.SetActive(false);
    }


    public void AInsert_EntryUnit(Unit_Stat Target)
    {
        for (int i = 0; i < UnitDataManager.Instance.TeamUnit_ids.Count; i++)
        {
            for (int j = 0; j < UnitDataManager.Instance.TeamUnit_ids[i].Length; j++)
            {
                if (UnitDataManager.Instance.TeamUnit_ids[i][j] == -1)
                {
                    Target.battle_team = i;
                    UnitDataManager.Instance.TeamUnit_ids[i][j] = Target.unit_id;
                    return;
                }
            }
        }
    }

    public void Change_BattleTeam(int teamnumber)
    {
        if (UnitDataManager.Instance.Team_Entry_Count(teamnumber) == 0)
        {
            // 팀 유닛을 배치해주세요
            return;
        }

        UnitSpawnManager.Instance.Selected_Team_Number = teamnumber;
        PlayerPrefs.SetInt("BattleTeam", teamnumber);
        Debug.Log("Team Change : " + UnitSpawnManager.Instance.Selected_Team_Number);

        for(int i = 0; i < Battle_TeamBtn.childCount;i++)
            if(i == teamnumber)
                Battle_TeamBtn.GetChild(i).GetComponent<Image>().color = Color.cyan;
            else
                Battle_TeamBtn.GetChild(i).GetComponent<Image>().color = Color.gray;

        for (int i = 0; i < Unit_Objs.Count; i++)
            Unit_Objs[i].GetComponent<UnitController>().Switch_By_Team(teamnumber);
    }

    public void Sort_By_Elemental(int Elemental)
    {
        if(Selected_Elemental == Elemental)
        {
            for (int i = 0; i < Unit_Objs.Count; i++)
                Unit_Objs[i].GetComponent<UnitController>().Switch_By_Elemental(0);

            return;
        }

        Selected_Elemental = Elemental;

        for (int i = 0; i < Unit_Objs.Count; i++)
            Unit_Objs[i].GetComponent<UnitController>().Switch_By_Elemental(Elemental);
    }
}
