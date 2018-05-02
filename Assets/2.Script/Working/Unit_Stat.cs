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



public class Unit_Stat : MonoBehaviour {

[Header("Unit Stat")]
    public int soldier_id; // 유닛 종류 아이디
    public int unit_id; // 유닛 획득 순 별 아이디
    public int unit_team;   // 아군 / 적군 구분       0 : 플레이어  1 : 적
    public int unit_type;   // 임시로 근거리, 원거리 구분에 사용중

    public float unit_HP = 100.0f;  // HP
    public float unit_Max_HP = 100.0f;  // HP
    public float unit_Damage = 10.0f;   // 데미지
    public int unit_DamageType;   // 물리, 마법 데미지 타입 구분     0 : 물리  1 : 마법
    public int unit_Element;  // 유닛 속성    1 < 2 < 3 < 1
    public int unit_counter_element;    // 이 유닛의 역상성 속성
    public float unit_PhysicalDef;  // 물리 방어력
    public float unit_MagicDef; // 마법 방어력   
    public float unit_CriticalRate; // 크리티컬 확률
    public float unit_CriticalDamage;   // 크리티컬 데미지 배수

    public float unit_moveSpeed = 0.01f;    // 이동속도
    public float unit_attackSpeed = 1.0f;   // 공격속도
    public float unit_req_gold = 100;
    public float base_up_gold = 100;
    public float upgold = 100;
    public int level = 1;

    // 데미지 계산
    public float damage_up;    // 상성 데미지
    public float damage_down;  // 역상성 데미지
    public float damage_Critical;

    public void SetData(float _unit_hp, float _unit_Damage, float _unit_moveSpeed, float _unit_attackSpeed, int _level, bool levelup)
    {
        if (_unit_attackSpeed == 0)
            unit_attackSpeed = 1.0f;

        if (_unit_moveSpeed == 0)
            unit_moveSpeed = 0.01f;

        if (_unit_hp == 0)
        {
            unit_Max_HP = 100.0f;
            unit_HP = 100.0f;
        }

        if (_unit_Damage == 0)
            unit_Damage = 15.0f;

        level = _level;

        float level_increase = Mathf.Pow(GlobalVar.level_var, (level - 1));

        unit_moveSpeed = _unit_moveSpeed;
        unit_Damage *=level_increase;
        upgold = base_up_gold * level_increase;

        unit_PhysicalDef = 10;
        unit_MagicDef = 10;

        // 레벨업시, 현재 남은 체력을 레벨업 후 증가된 체력 비율로 증가
        if(levelup)
        {
            float percent = unit_HP / unit_Max_HP;

            unit_Max_HP = 100.0f;
            unit_HP = 100.0f;
            unit_Max_HP *= level_increase;
            unit_HP = unit_Max_HP * percent;
        } else
        {
            unit_Max_HP = 100.0f;
            unit_HP = 100.0f;
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

}
