using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UNIT_TYPE = 0 : 근접
//      SOIDIER_ID = 0 : 원색
//      SOIDIER_ID = 1 : 노란색
//      SOIDIER_ID = 2 : 빨간색
// UNIT_TYPE = 1 : 원거리
//      SOIDIER_ID = 0 : 초록색
//      SOIDIER_ID = 1 : 파란색
// UNIT_TYPE = 2 : HQ



public class Unit_Stat : MonoBehaviour
{

    public Unit_Stat Clone()
    {
        Unit_Stat Clones = new Unit_Stat();
        Clones.soldier_id = this.soldier_id;
        Clones.Name = this.Name;
        Clones.unit_type = this.unit_type;
        Clones.unit_HP = this.unit_HP;
        Clones.unit_Max_HP = this.unit_HP;
        Clones.unit_Damage = this.unit_Damage;
        Clones.unit_DamageType = this.unit_DamageType;
        Clones.unit_Element = this.unit_Element;
        Clones.unit_CriticalRate = this.unit_CriticalRate;
        Clones.unit_CriticalDamage = this.unit_CriticalDamage;
        Clones.unit_MagicDef = this.unit_MagicDef;
        Clones.unit_PhysicalDef = this.unit_PhysicalDef;
        Clones.unit_moveSpeed = this.unit_moveSpeed;
        Clones.unit_attackSpeed = this.unit_attackSpeed;
        Clones.base_up_gold = this.base_up_gold;
        Clones.unit_Element = this.unit_Element;
        Clones.tier = this.tier;
        Clones.face_sprite = this.face_sprite;

        return Clones;
    }

    [Header("Unit Stat")]
    public int soldier_id; // 유닛 종류 아이디
    public int unit_id = -1; // 유닛 획득 순 별 아이디
    public int unit_team;   // 아군 / 적군 구분       0 : 플레이어  1 : 적
    public int unit_type;   // 임시로 근거리, 원거리 구분에 사용중

    public string face_sprite; // UI 유닛 스프라이트 파일 이름

    public string Name;
    /// <summary>
    /// 스폰 순위. 0으로 갈수록 높음
    /// </summary>
    public int spawn_prefer = 0;
    public int tier = 0; // 유닛 단계
    /// <summary>
    /// 체력
    /// </summary>
    public float unit_HP = 100.0f;
    public float unit_Max_HP = 100.0f;  // HP
    public float unit_Damage = 10.0f;   // 데미지
    /// <summary>
    /// 물리, 마법 데미지 타입 구분     0 : 물리  1 : 마법
    /// </summary>
    public int unit_DamageType;
    /// <summary>
    /// 유닛 속성    1 - 2 - 3 - 1
    /// </summary>
    public int unit_Element;
    public int unit_counter_element;    // 이 유닛의 역상성 속성
    public float unit_PhysicalDef;  // 물리 방어력
    public float unit_MagicDef; // 마법 방어력   
    public float unit_CriticalRate; // 크리티컬 확률
    public float unit_CriticalDamage;   // 크리티컬 데미지 배수

    public float unit_moveSpeed = 0.01f;    // 이동속도
    public float unit_attackSpeed = 1.0f;   // 공격속도
    public float unit_req_gold = 100;
    public float base_up_gold = 100;

    public float[] upgolds;
    public int level = 1;

    public int battle_team = 0; // 전투 배치 팀 번호 (1,2,3)

    // 데미지 계산
    public float damage_up;    // 상성 데미지
    public float damage_down;  // 역상성 데미지
    public float damage_Critical;

    public void Init_Stat()
    {
        Unit_Stat Base_Data;

        if (unit_team == 0)
        {
            Base_Data = UnitDataManager.Instance.Master_UnitData.Find(x => x.soldier_id == soldier_id);
        }
        else
        {
            Base_Data = UnitDataManager.Instance.Enemy_UnitData.Find(x => x.soldier_id == soldier_id);
            //Debug.Log(Base_Data.Name);
        }

        this.soldier_id = Base_Data.soldier_id;
        this.Name = Base_Data.Name;
        this.unit_type = Base_Data.unit_type;
        this.unit_HP = Base_Data.unit_HP;
        this.unit_Max_HP = Base_Data.unit_HP;
        this.unit_Damage = Base_Data.unit_Damage;
        this.unit_DamageType = Base_Data.unit_DamageType;
        this.unit_Element = Base_Data.unit_Element;
        this.unit_CriticalRate = Base_Data.unit_CriticalRate;
        this.unit_CriticalDamage = Base_Data.unit_CriticalDamage;
        this.unit_MagicDef = Base_Data.unit_MagicDef;
        this.unit_PhysicalDef = Base_Data.unit_PhysicalDef;
        this.unit_moveSpeed = Base_Data.unit_moveSpeed;
        this.unit_attackSpeed = Base_Data.unit_attackSpeed;
        this.base_up_gold = Base_Data.base_up_gold;
        this.unit_Element = Base_Data.unit_Element;
        this.face_sprite = Base_Data.face_sprite;
    }

    public void SetData(int _level, bool levelup)
    {
        Unit_Stat Base_Data;

        if (unit_team == 0)
        {
            Base_Data = UnitDataManager.Instance.Master_UnitData.Find(x => x.soldier_id == soldier_id);
        }
        else
        {
            Base_Data = UnitDataManager.Instance.Enemy_UnitData.Find(x => x.soldier_id == soldier_id);
        }
        level = _level;
        float level_increase = Mathf.Pow(GlobalVar.level_var, (level - 1));

        upgolds = new float[4];

        upgolds[0] = Mathf.Round(base_up_gold * level_increase);
        upgolds[1] = Mathf.Round(Get_LevelUp_Gold(1));
        upgolds[2] = Mathf.Round(Get_LevelUp_Gold(2));
        upgolds[3] = Mathf.Round(Get_LevelUp_Gold(3));

        this.unit_Damage = Base_Data.unit_Damage;
        this.unit_MagicDef = Base_Data.unit_MagicDef;
        this.unit_PhysicalDef = Base_Data.unit_PhysicalDef;

        unit_Damage *= level_increase;
        unit_PhysicalDef *= level_increase;
        unit_MagicDef *= level_increase;


        // 레벨업시, 현재 남은 체력을 레벨업 후 증가된 체력 비율로 증가
        if (levelup)
        {
            float percent = unit_HP / unit_Max_HP;
            this.unit_HP = Base_Data.unit_HP;
            this.unit_Max_HP = Base_Data.unit_HP;
            unit_Max_HP *= level_increase;
            unit_HP = unit_Max_HP * percent;
        }
        else
        {

            unit_Max_HP = Base_Data.unit_HP;
            unit_HP = Base_Data.unit_HP;
            unit_Max_HP *= level_increase;
            unit_HP *= level_increase;
        }

        // 역상성
        switch (unit_Element)
        {
            case 1:
                unit_counter_element = 2;
                break;
            case 2:
                unit_counter_element = 3;
                break;
            case 3:
                unit_counter_element = 1;
                break;
        }


        // 데미지 계산
        damage_up = unit_Damage * 1.2f;
        damage_down = unit_Damage * 0.8f;
        damage_Critical = unit_Damage * 0.5f;
    }

    public float Get_LevelUp_Gold(int level_tier)
    {
        float Request_Gold = 0;
        int Request_Level = 0;
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
            case 3:
                {
                    Request_Level = 1000;
                    break;
                }
            default:
                {
                    Request_Level = 1;
                    break;
                }
        }

        Request_Level += level;

        for (int i = level; i < Request_Level; i++)
        {
            double level_increase2 = Mathf.Pow(GlobalVar.level_var, i - (level));
            Request_Gold += (float)(upgolds[0] * level_increase2);
        }

        return Request_Gold;
    }
}
