using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// 현재 스폰되있는 유닛 데이터 관리
public class UnitDataManager : MonoBehaviour
{
    private static UnitDataManager instance = null;
    public static UnitDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UnitDataManager").AddComponent<UnitDataManager>();
            }
            return instance;
        }
    }

    public List<Unit_Stat> PlayerSpawnUnitList
    {
        get
        {
            return playerSpawnUnitList;
        }

        set
        {
            playerSpawnUnitList = value;
        }
    }

    public List<Unit_Stat> EnemySpawnUnitList
    {
        get
        {
            return enemySpawnUnitList;
        }

        set
        {
            enemySpawnUnitList = value;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public List<Unit_Stat> Master_UnitData = new List<Unit_Stat>();
    public List<Unit_Stat> Enemy_UnitData = new List<Unit_Stat>();

    List<Unit_Stat> playerSpawnUnitList = new List<Unit_Stat>();
    List<Unit_Stat> enemySpawnUnitList = new List<Unit_Stat>();

    public List<Transform> CurrSpawnedUnitTrans = new List<Transform>();

    public Transform headUnitTrans;


    public List<int[]> TeamUnit_ids;

    public float[] Unit_Upgolds;

    void Start()
    {
        Unit_Upgolds = new float[4];
    }

    float tempPos;
    public int tempHead;

    private void FixedUpdate()
    {
        if (CurrSpawnedUnitTrans.Count > 0)
        {
            for (int i = 0; i < CurrSpawnedUnitTrans.Count; i++)
            {
                if (CurrSpawnedUnitTrans[i].position.x > tempPos)
                {
                    tempPos = CurrSpawnedUnitTrans[i].position.x;
                    tempHead = i;
                }
            }

            if (CurrSpawnedUnitTrans.Count.Equals(0))
            {
                return;
            }

            headUnitTrans = CurrSpawnedUnitTrans[tempHead].transform;

            tempPos = 0;
        }
        else
        {
            tempHead = 0;
        }
    }

    public void AddUnit()
    {
        Master_UnitData = new List<Unit_Stat>();
        Enemy_UnitData = new List<Unit_Stat>();

        string Unit_DataString = PlayerPrefs.GetString("Data_Soldier");

        var Data = JSON.Parse(Unit_DataString);

        for (int i = 0; i < Data.Count; i++)
        {
            Unit_Stat Unit = new Unit_Stat();
            Unit.soldier_id = Data[i]["id"].AsInt;
            Unit.Name = Data[i]["name"];
            Unit.unit_type = Data[i]["type"];
            Unit.unit_HP = Data[i]["hp"];
            Unit.unit_Damage = Data[i]["b_atk"].AsInt;
            Unit.unit_PhysicalDef = Data[i]["p_def"].AsInt;
            Unit.unit_MagicDef = Data[i]["m_def"].AsInt;
            Unit.unit_DamageType = Data[i]["damagetype"].AsInt;
            Unit.base_up_gold = Data[i]["base_up_gold"].AsInt;
            Unit.unit_moveSpeed = Data[i]["movespeed"].AsFloat * 0.01f;
            Unit.unit_CriticalDamage = Data[i]["criticaldamage"].AsInt;
            Unit.unit_CriticalRate = Data[i]["critical_rate"].AsInt;
            Unit.unit_Element = Data[i]["element"].AsInt;
            Unit.base_up_gold = Data[i]["base_up_gold"].AsInt;
            Unit.tier = Data[i]["tier"].AsInt;
            Unit.face_sprite = Data[i]["face_sprite"];
            Unit.unit_class = Data[i]["unit_class"];
            Unit.unit_team = 0;

            Master_UnitData.Add(Unit);
        }

        string Enemy_DataString = PlayerPrefs.GetString("Data_Enemy");
        var EnenmyData = JSON.Parse(Enemy_DataString);

        for (int i = 0; i < EnenmyData.Count; i++)
        {
            Unit_Stat Unit = new Unit_Stat();
            Unit.soldier_id = EnenmyData[i]["id"].AsInt;
            Unit.Name = EnenmyData[i]["name"];
            Unit.unit_type = EnenmyData[i]["type"];
            Unit.unit_HP = EnenmyData[i]["hp"];
            Unit.unit_Damage = EnenmyData[i]["b_atk"].AsInt;
            Unit.unit_PhysicalDef = EnenmyData[i]["p_def"].AsInt;
            Unit.unit_MagicDef = EnenmyData[i]["m_def"].AsInt;
            Unit.unit_DamageType = EnenmyData[i]["damagetype"].AsInt;
            Unit.base_up_gold = EnenmyData[i]["base_up_gold"].AsInt;
            Unit.unit_moveSpeed = EnenmyData[i]["movespeed"].AsFloat * 0.01f;
            Unit.unit_CriticalDamage = EnenmyData[i]["criticaldamage"].AsInt;
            Unit.unit_CriticalRate = EnenmyData[i]["critical_rate"].AsInt;
            Unit.unit_Element = EnenmyData[i]["element"].AsInt;
            Unit.base_up_gold = EnenmyData[i]["base_up_gold"].AsInt;
            Unit.tier = EnenmyData[i]["tier"].AsInt;
            Unit.unit_team = 1;

            Enemy_UnitData.Add(Unit);
        }

        TeamUnit_ids = new List<int[]>();

        for (int i = 0; i < 3; i++)
        {
            TeamUnit_ids.Add(new int[5] { -1, -1, -1, -1, -1 });
        }

        for (int i = 0; i < 16; i++)
        {
            int soldier_id = Random.Range(0, 6);

            Unit_Stat NewUnit = Master_UnitData[soldier_id].Clone();

            NewUnit.unit_id = i;
            NewUnit.level = 10;
            NewUnit.SetData(NewUnit.level, true);
            PlayerSpawnUnitList.Add(NewUnit);

            UIManager.Instance.AInsert_EntryUnit(NewUnit);
        }

        // 적 유닛 생성
        for (int i = 0; i < 3; i++)
        {
            Unit_Stat NewUnit = Enemy_UnitData[0].Clone();
            NewUnit.unit_team = 1;
            EnemySpawnUnitList.Add(NewUnit);
        }

        Set_AllUnitLevelUpGold();
    }

    public void DelUnit()
    {

    }

    public float Require_LvUpGold(int level_tier)
    {
        float Request_Gold = 0;

        for (int i = 0; i < playerSpawnUnitList.Count; i++)
        {
            Request_Gold += playerSpawnUnitList[i].upgolds[level_tier];
        }

        return Request_Gold;
    }

    public void Set_AllUnitLevelUpGold()
    {
        for (int i = 0; i < 4; i++)
        {
            Unit_Upgolds[i] = Require_LvUpGold(i);
        }
    }

    // 유닛 레벨업 버튼으로 호출
    public void Unit_LevelUp(int unit_id, int level_tier)
    {
        int Request_Level = level_tier;

        Unit_Stat Unit = playerSpawnUnitList.Find(x => x.unit_id == unit_id);

        switch (level_tier)
        {
            case 0:
                {
                    Request_Level = 1;
                    GameManager.Instance.User.Have_gold -= Unit.upgolds[level_tier];
                    break;
                }
            case 1:
                {
                    Request_Level = 10;
                    GameManager.Instance.User.Have_gold -= Unit.upgolds[level_tier];
                    break;
                }
            case 2:
                {
                    Request_Level = 100;
                    GameManager.Instance.User.Have_gold -= Unit.upgolds[level_tier];
                    break;
                }
            default:
                {
                    Request_Level = 1;
                    GameManager.Instance.User.Have_gold -= Unit.upgolds[level_tier];
                    break;
                }
        }

        Unit.level += Request_Level;
        Unit.SetData(Unit.level, true);

        List<GameObject> PUnits = UnitSpawnManager.Instance.PUnitObj;
        for (int i = 0; i < PUnits.Count; i++)
        {
            if (PUnits[i].GetComponent<UnitScript>().unit_id == unit_id)
                PUnits[i].GetComponent<UnitScript>().SetData(Unit.level, true);
        }

        UnitController This_Controller = UIManager.Instance.Unit_Controllers.Find(x => x.This_Id == unit_id);
        This_Controller.Set_Info(Unit);
        This_Controller.Refresh_Info();

        Set_AllUnitLevelUpGold();
    }

    public void All_LevelUp(int level_tier)
    {
        int Request_Level = level_tier;

        switch (level_tier)
        {
            case 0:
                {
                    Request_Level = 1;
                    break;
                }
            case 1:
                {
                    Request_Level = 10;
                    break;
                }
            case 2:
                {
                    Request_Level = 100;
                    break;
                }
            default:
                {
                    Request_Level = 1;
                    break;
                }
        }

        GameManager.Instance.User.Have_gold -= this.Unit_Upgolds[level_tier];

        for (int i = 0; i < playerSpawnUnitList.Count; i++)
        {
            Unit_Stat Unit = playerSpawnUnitList[i];
            Unit.level += Request_Level;
            Unit.SetData(Unit.level, true);

            List<GameObject> PUnits = UnitSpawnManager.Instance.PUnitObj;
            for (int j = 0; j < PUnits.Count; j++)
            {
                PUnits[j].GetComponent<UnitScript>().SetData(Unit.level, true);
            }

            UnitController This_Controller = UIManager.Instance.Unit_Controllers.Find(x => x.This_Id == Unit.unit_id);
            This_Controller.Set_Info(Unit);
            This_Controller.Refresh_Info();
        }

        Set_AllUnitLevelUpGold();
    }

    public void Unit_Buy(int unit_id)
    {

    }

    /// <summary>
    /// 편성 가능하면 true
    /// </summary>
    ///<param name = "id" > unit id</param>
    /// <returns></returns>
    public bool Check_Team(int id)
    {
        int Target_Team = UIManager.Instance.Target_Team;

            for (int i = 0; i < TeamUnit_ids[Target_Team].Length; i++)
            {
                if (TeamUnit_ids[Target_Team][i] == id)
                    return false;
            }

        return true;
    }

    /// <summary>
    /// 유닛이 이미 편성된 상태면 편성되있던 팀에서 제외시킴
    /// </summary>
    public void Team_Entry_Out(int Unit_Id, int Unit_Team)
    {
        for (int i = 0; i < TeamUnit_ids[Unit_Team].Length; i++)
        {
            if (TeamUnit_ids[Unit_Team][i] == Unit_Id)
                TeamUnit_ids[Unit_Team][i] = -1;
        }
    }

    /// <summary>
    /// 팀에 배치된 유닛 수를 계산함
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public int Team_Entry_Count(int team)
    {
        int count = 0;

        int teamnumber = team--;

        for (int i = 0; i < TeamUnit_ids[teamnumber].Length; i++)
        {
            if (TeamUnit_ids[teamnumber][i] >= 0)
                count++;
        }

        return count;
    }
}

