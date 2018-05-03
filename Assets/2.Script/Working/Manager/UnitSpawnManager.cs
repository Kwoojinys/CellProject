using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛 스폰 관리
public class UnitSpawnManager : MonoBehaviour
{
    public GameObject unit_Prefab;


    /// <summary>
    /// 플레이어 유닛을 보관해두는 부모 오브젝트
    /// </summary>
    private Transform PlayerUnits;
    /// <summary>
    /// 적 유닛을 보관해두는 부모 오브젝트
    /// </summary>
    private Transform EnemyUnits;

    public Transform PlayerHQ;
    public Transform EnemyHQ;

    public List<GameObject> PUnitObj;
    public List<GameObject> PMinionObj;
    public List<GameObject> EUnitObj;

    private static UnitSpawnManager instance = null;
    public static UnitSpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UnitSpawnManager").AddComponent<UnitSpawnManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        Debug.Log("Time Scale Control ");
        Time.timeScale = 1.0f;
    }

    public void Init_PlayerUnitPool()
    {
        PlayerUnits = GameObject.Find("PlayerUnit").transform;
        List<UnitControl> Unit_List = UnitDataManager.Instance.PlayerSpawnUnitList;

        for (int i = 0; i < Unit_List.Count; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                if(Unit_List[i].unit_type.Equals(0))
                {
                    // Melee
                    unit_Prefab = Resources.Load("Melee_Prefab") as GameObject;
                }
                else
                {
                    // Ranged
                    unit_Prefab = Resources.Load("Ranged_Prefab") as GameObject;
                }
                
                GameObject Unit = Instantiate(unit_Prefab);
                Unit.transform.parent = PlayerUnits;
                PUnitObj.Add(Unit);
                Unit.SetActive(false);
            }
        }

        //GameManager.Instance.m_Loading = Loading_State.EUnit_Create;
        Init_PlayerMinionPool();
    }

    // 영웅 부하 풀
    public void Init_PlayerMinionPool()
    {
        PlayerUnits = GameObject.Find("PlayerMinion").transform;
        List<UnitControl> Unit_List = UnitDataManager.Instance.PlayerSpawnUnitList;

        for (int i = 0; i < Unit_List.Count; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (Unit_List[i].unit_type.Equals(0))
                {
                    // Melee
                    unit_Prefab = Resources.Load("MeleeMinion_Prefab") as GameObject;
                }
                else
                {
                    // Ranged
                    unit_Prefab = Resources.Load("RangedMinion_Prefab") as GameObject;
                }
                GameObject Unit = Instantiate(unit_Prefab);
                Unit.transform.parent = PlayerUnits;
                PMinionObj.Add(Unit);
                Unit.SetActive(false);

            }
        }

        GameManager.Instance.m_Loading = Loading_State.EUnit_Create;
    }

    public void Init_EnemyUnitPool()
    {
        EnemyUnits = GameObject.Find("EnemyUnit").transform;
        List<UnitControl> Unit_List = UnitDataManager.Instance.EnemySpawnUnitList;
        for (int i = 0; i < Unit_List.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject Enemy_Prefab = Resources.Load("Enemy_Prefab") as GameObject;
                GameObject Unit = Instantiate(Enemy_Prefab);
                Unit.transform.parent = EnemyUnits;
                EUnitObj.Add(Unit);
                Unit.SetActive(false);
            }
        }

        GameManager.Instance.m_Loading = Loading_State.Game_Start;
    }

    public void Start_Summon()
    {
        StartCoroutine(this.Summon_PlayerUnit());
        StartCoroutine(this.Summon_EnemyUnit());
    }

    public void Stop_Summon()
    {
        StopCoroutine(this.Summon_PlayerUnit());
    }

    public IEnumerator Summon_PlayerUnit()
    {
        List<UnitControl> Unit_List = UnitDataManager.Instance.PlayerSpawnUnitList;

        PlayerHQ = GameObject.Find("PlayerHQ").transform;
        for (int i = 0; i < Unit_List.Count; i++)
        {
            for (int j = 0; j < PUnitObj.Count; j++)
            {
                if (PUnitObj[j].activeSelf) continue;
                PUnitObj[j].SetActive(true);

                PUnitObj[j].transform.localPosition = Spawn_Position(0);
                PUnitObj[j].GetComponent<UnitControl>().unit_Element = Unit_List[i].unit_Element;
                PUnitObj[j].GetComponent<UnitControl>().unit_type = Unit_List[i].unit_type;
                PUnitObj[j].GetComponent<UnitControl>().soldier_id = Unit_List[i].soldier_id;
                PUnitObj[j].GetComponent<UnitControl>().SetData(0, 0, 0.03f, 0, Unit_List[j].level, false);
                PUnitObj[j].GetComponent<UnitControl>().INIT();
                UnitDataManager.Instance.CurrSpawnedUnitTrans.Add(PUnitObj[j].gameObject.transform);
                break;
            }

            //StartCoroutine(this.Summon_Minion(Unit_List[i].unit_id, Unit_List[i].unit_type, Unit_List[i].soldier_id, 10));

            yield return new WaitForSeconds(0.8f);
        }

        yield return new WaitForSeconds(0.1f);

    }

    public IEnumerator Summon_Minion(int id, int unit_type, int soldier_id, int count) 
    {
        //Debug.Log("Summon Minion " + id*10 + " PMinionObj " + PMinionObj.Count);

        PlayerHQ = GameObject.Find("PlayerHQ").transform;

        for (int i = id*10; i < count*(id+1); i++)
        {
            if (PMinionObj[i].activeSelf) continue;

            PMinionObj[i].SetActive(true);
            PMinionObj[i].transform.localPosition = Spawn_Position(0);
            PMinionObj[i].GetComponent<UnitControl>().unit_type = unit_type;
            PMinionObj[i].GetComponent<UnitControl>().soldier_id = soldier_id;
            PMinionObj[i].GetComponent<UnitControl>().SetData(0, 0, 0.035f, 0, 1, false);
            PMinionObj[i].GetComponent<UnitControl>().INIT();

            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator Summon_EnemyUnit()
    {
        List<UnitControl> Unit_List = UnitDataManager.Instance.EnemySpawnUnitList;

        EnemyHQ = GameObject.Find("EnemyHQ").transform;
        for (int i = 0; i < Unit_List.Count; i++)
        {
            for (int j = 0; j < EUnitObj.Count; j++)
            {
                if (EUnitObj[j].activeSelf) continue;
                EUnitObj[j].SetActive(true);
                EUnitObj[j].transform.localPosition = Spawn_Position(1);
                int rand = Random.Range(1, 4);
                EUnitObj[j].GetComponent<UnitControl>().unit_Element = rand;
                EUnitObj[j].GetComponent<UnitControl>().SetData(0, 0, 0.03f, 0, GameManager.Instance.Current_Stage, false);
                EUnitObj[j].GetComponent<UnitControl>().INIT();
                break;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.1f);

    }
    /// <summary>
    /// 모든 캐릭터 삭제
    /// </summary>
    public void Reset_All_Char()
    {
        UnitControl PHQ = PlayerHQ.GetComponent<UnitControl>();
        PHQ.unit_HP = PHQ.unit_Max_HP;
        PHQ.TakeDamage(0);

        UnitControl EHQ = EnemyHQ.GetComponent<UnitControl>();
        EHQ.unit_HP = EHQ.unit_Max_HP;
        EHQ.TakeDamage(0);

        for (int j = 0; j < EUnitObj.Count; j++)
        {
            if (!EUnitObj[j].activeSelf) continue;

            EUnitObj[j].SetActive(false);
        }

        for (int j = 0; j < PUnitObj.Count; j++)
        {
            if (!PUnitObj[j].activeSelf) continue;

            PUnitObj[j].SetActive(false);
        }

        for (int j = 0; j < PMinionObj.Count; j++)
        {
            if (!PMinionObj[j].activeSelf) continue;

            PMinionObj[j].SetActive(false);
        }

        UnitDataManager.Instance.CurrSpawnedUnitTrans.Clear();
        UnitDataManager.Instance.tempHead = 0;

        StopAllCoroutines();
    }
    
    /// <summary>
    /// 사망시 5초후 리스폰
    /// </summary>
    /// <param name="Target"></param>
    public void Respawn(GameObject Target)
    {
        StartCoroutine(this.Respawning(Target));
    }

    public IEnumerator Respawning(GameObject Target)
    {
        yield return new WaitForSeconds(5f);

        UnitControl UnitInfo = Target.GetComponent<UnitControl>();
        Transform TargetHQ;

        if (UnitInfo.unit_team == 1)
        {
            TargetHQ = EnemyHQ;
        } else
        {
            TargetHQ = PlayerHQ;
        }

        Target.transform.localPosition = Spawn_Position(UnitInfo.unit_team);
        Target.SetActive(true);

        UnitInfo.INIT();
    }

    public Vector2 Spawn_Position(int team)
    {
        Transform TargetHQ;

        if (team == 1)
        {
            TargetHQ = EnemyHQ;
        } else
        {
            TargetHQ = PlayerHQ;
        }

        Vector2 Position = TargetHQ.localPosition;

        float range = Random.Range(-0.08f, 0.21f);
        Position.y += range;
        return Position;
    }

}
