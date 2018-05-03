using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<UnitControl> PlayerSpawnUnitList
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

    public List<UnitControl> EnemySpawnUnitList
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


    List<UnitControl> playerSpawnUnitList = new List<UnitControl>();
    List<UnitControl> enemySpawnUnitList = new List<UnitControl>();

    public List<Transform> CurrSpawnedUnitTrans = new List<Transform>();


    public Transform headUnitTrans;


    void Start()
    {
        
    }

    float tempPos;
    public int tempHead;

    private void FixedUpdate()
    {
        if(CurrSpawnedUnitTrans.Count > 0)
        {
            for (int i = 0; i < CurrSpawnedUnitTrans.Count; i++)
            {
                if (CurrSpawnedUnitTrans[i].position.x > tempPos)
                {
                    tempPos = CurrSpawnedUnitTrans[i].position.x;
                    tempHead = i;
                }
            }

            if(CurrSpawnedUnitTrans.Count.Equals(0))
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
        // 영웅 / 병사 테스트
        for (int i = 0; i < 10; i++)
        {
            UnitControl NewUnit = new UnitControl();

            ////// 기본
            ////NewUnit.unit_type = 0;
            ////NewUnit.soldier_id = 0;

            // 종류 구분 테스트
            if (i.Equals(0))
            {
                NewUnit.unit_type = 0;
                NewUnit.soldier_id = 0;
                NewUnit.unit_DamageType = 0;
                NewUnit.unit_Element = 2;
            }
            else if (i.Equals(1))
            {
                NewUnit.unit_type = 0;
                NewUnit.soldier_id = 1;
                NewUnit.unit_DamageType = 0;
                NewUnit.unit_Element = 1;
            }
            else if (i.Equals(2))
            {
                NewUnit.unit_type = 0;
                NewUnit.soldier_id = 2;
            }
            else if (i.Equals(3))
            {
                NewUnit.unit_type = 1;
                NewUnit.soldier_id = 0;
            }
            else
            {
                NewUnit.unit_type = 1;
                NewUnit.soldier_id = 1;
            }

            NewUnit.unit_id = i;
            NewUnit.level = 10;
            NewUnit.SetData(3000, 100, 0.03f, 0, NewUnit.level, true);
            PlayerSpawnUnitList.Add(NewUnit);
        }


        ////for (int i = 0; i < 20; i++)
        ////{
        ////    UnitControl NewUnit = new UnitControl();

        ////    ////// 기본
        ////    ////NewUnit.unit_type = 0;
        ////    ////NewUnit.soldier_id = 0;

        ////    ////// 종류 구분 테스트
        ////    ////if (i < 4)
        ////    ////{
        ////    ////    NewUnit.unit_type = 0;
        ////    ////    NewUnit.soldier_id = 0;
        ////    ////}
        ////    ////else if(i < 8)
        ////    ////{
        ////    ////    NewUnit.unit_type = 0;
        ////    ////    NewUnit.soldier_id = 1;
        ////    ////}
        ////    ////else if (i < 12)
        ////    ////{
        ////    ////    NewUnit.unit_type = 0;
        ////    ////    NewUnit.soldier_id = 2;
        ////    ////}
        ////    ////else if (i < 16)
        ////    ////{
        ////    ////    NewUnit.unit_type = 1;
        ////    ////    NewUnit.soldier_id = 0;
        ////    ////}
        ////    ////else
        ////    ////{
        ////    ////    NewUnit.unit_type = 1;
        ////    ////    NewUnit.soldier_id = 1;
        ////    ////}

        ////    NewUnit.unit_id = i;
        ////    NewUnit.level = 20;
        ////    NewUnit.SetData(0, 0, 0.03f, 0, NewUnit.level, true);
        ////    PlayerSpawnUnitList.Add(NewUnit);
        ////}


        // 적 유닛 생성
        for (int i = 0; i < 20; i++)
        {
            UnitControl NewUnit = new UnitControl();
            NewUnit.unit_team = 1;
            EnemySpawnUnitList.Add(NewUnit);
        }
    }

    public void DelUnit()
    {

    }

    // 유닛 레벨업 버튼으로 호출
    public void Unit_LevelUp(int unit_id, int level_tier)
    {
        int Request_Level = level_tier;

        UnitControl Unit = playerSpawnUnitList.Find(x => x.unit_id == unit_id);


        switch (level_tier)
        {
            case 0:
                {
                    Request_Level = 1;
                    GameManager.Instance.User.Have_gold -= Unit.upgold;
                    break;
                }
            case 1:
                {
                    Request_Level = 10;
                    GameManager.Instance.User.Have_gold -= Unit.up10gold;
                    break;
                }
            case 2:
                {
                    Request_Level = 100;
                    GameManager.Instance.User.Have_gold -= Unit.up100gold;
                    break;
                }
            default:
                {
                    Request_Level = 1;
                    GameManager.Instance.User.Have_gold -= Unit.upgold;
                    break;
                }
        }

        Unit.level += Request_Level;
        Unit.SetData(0, 0, 0.03f, 0, Unit.level, true);

        List<GameObject> PUnits = UnitSpawnManager.Instance.PUnitObj;
        for (int i = 0; i < PUnits.Count;i++)
        {
            PUnits[i].GetComponent<UnitControl>().SetData(0, 0, 0.03f, 0, Unit.level, true);
        }

        UnitController This_Controller = UIManager.Instance.Unit_Controllers.Find(x => x.This_Id == unit_id);
        This_Controller.Set_Info(Unit);
        This_Controller.Refresh_Info();

    }

    public void Unit_Buy(int unit_id)
    {
    }
}
