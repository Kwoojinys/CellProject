using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;



// 각 유닛 객체 데이터
public class UnitScript : Unit_Stat
{
    [HideInInspector]
    public float tempAttackSpeed;   // 공격속도 계산용

    int direction;  // 이동방향

    float chase_MoveFloat = 0;

    Transform thisTrans;
    [Space(20)]
    public Transform targetTrans;
    public UnitScript targetUnitData;
    Transform HitBox;

    Transform unit_AttackColl;

    public SkeletonAnimation sAnim;
    public Spine.AnimationState sAnimState;

    bool isMoveAnim;
    bool isIdleAnim;
    bool isAttackAnim;

    public Transform HP_Bar;

    UnitAttackColl scAttackColl;
    ArrowMove arrowMove;

    public enum eUnitState
    {
        death,
        wait,
        move,
        attack
    }
    public eUnitState unitState;



    void Start()
    {
        thisTrans = this.transform;

        // HQ
        if (this.unit_type == 2)
        {
            HP_Bar = thisTrans.GetChild(0).GetChild(0).GetComponent<Transform>();
            HitBox = thisTrans.GetChild(1);
            return;
        }


        sAnim = thisTrans.GetChild(0).GetComponent<SkeletonAnimation>();
        sAnimState = sAnim.state;

        unit_AttackColl = thisTrans.GetChild(1);
        HitBox = thisTrans.GetChild(2);
        HP_Bar = thisTrans.GetChild(3).GetChild(0).GetComponent<Transform>();


        scAttackColl = unit_AttackColl.GetComponent<UnitAttackColl>();


        if (unit_team.Equals(0))
        {
            direction = 1;
        }
        else
        {
            sAnim.skeleton.SetColor(Color.gray);
            direction = -1;
            sAnim.transform.Rotate(0, 180, 0);
        }


        if (unit_type.Equals(0))
        {
            // 근접
            switch (soldier_id)
            {
                case 0:
                    // 원색
                    break;
                case 1:
                    // 노란색
                    sAnim.skeleton.SetColor(Color.yellow);
                    unit_AttackColl.GetComponent<BoxCollider2D>().size = new Vector2(2, 10);
                    break;
                case 2:
                    // 빨간색
                    sAnim.skeleton.SetColor(Color.red);
                    unit_AttackColl.GetComponent<BoxCollider2D>().size = new Vector2(3, 10);
                    break;
            }
        }
        else if (unit_type.Equals(1))
        {
            // 원거리
            switch (soldier_id)
            {
                case 0:
                    // 초록색
                    sAnim.skeleton.SetColor(Color.green);
                    break;
                case 1:
                    // 파란색
                    sAnim.skeleton.SetColor(Color.blue);
                    unit_AttackColl.GetComponent<BoxCollider2D>().size = new Vector2(9, 10);
                    break;
            }
        }

        if (unit_type.Equals(1))
        {
            arrowMove = thisTrans.GetChild(5).GetChild(0).GetComponent<ArrowMove>();
        }


        // 스파인 애니메이션 이벤트
        if (unit_team.Equals(1))
        {
            //sAnim.state.Event += HandleEvent;

            sAnimState.Complete += delegate
            {
                if (sAnimState.GetCurrent(0).ToString().Equals("Attack"))
                {
                    HandleEvent_AttackComplete();
                }
            };
        }
        else
        {
            //sAnimState.Event += HandleEvent;

            sAnimState.Complete += delegate
            {
                if (sAnimState.GetCurrent(0).ToString().Equals("attack"))
                {
                    HandleEvent_AttackComplete();
                }
            };
        }



        unitState = eUnitState.move;


        StartCoroutine("UnitControlCorou");
    }


    // 스파인 애니메이션중 이벤트 호출
    void HandleEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        //Debug.Log(e.Data.name);
        //sAnimState.SetAnimation(0, "Idle", true);
    }


    // 공격 애니메이션이 완료된 후 호출
    void HandleEvent_AttackComplete()
    {
        isAttackAnim = false;

        if (scAttackColl.collUnit.Count.Equals(0))
        {
            unitState = eUnitState.move;
            tempAttackSpeed = unit_attackSpeed;
        }
    }


    public void INIT()
    {
        unitState = eUnitState.move;
        targetUnitData = null;
        isMoveAnim = false;

        tempAttackSpeed = unit_attackSpeed;
        isAttackAnim = false;

        if (HitBox != null)
        {
            HitBox.gameObject.SetActive(true);
            StartCoroutine(this.UnitControlCorou());
            unitState = eUnitState.move;
            unit_HP = unit_Max_HP;
            TakeDamage(0);
        }
    }


    IEnumerator UnitControlCorou()
    {
        while (true)
        {
            if(GameStateManager.Instance.isStageClear)
            {
                break;
            }

            switch (unitState)
            {
                case eUnitState.wait:
                    if (unit_team.Equals(0))
                    {
                        if (!isIdleAnim)
                        {
                            isIdleAnim = true;
                            sAnimState.SetAnimation(0, "breath", true);
                        }
                    }
                    break;
                case eUnitState.move:
                    if (unit_team.Equals(0))
                    {
                        if (!isMoveAnim)
                        {
                            isMoveAnim = true;
                            sAnimState.SetAnimation(0, "walk", true);
                        }
                    }
                    else
                    {
                        if (!isMoveAnim)
                        {
                            isMoveAnim = true;
                            sAnimState.SetAnimation(0, "Move", true);
                        }
                    }
                    thisTrans.Translate(Vector3.right * direction * unit_moveSpeed * Time.timeScale);
                    break;
                case eUnitState.attack:

                    isMoveAnim = false;
                    isIdleAnim = false;

                    if (!isAttackAnim)
                    {
                        tempAttackSpeed += Time.deltaTime;

                        if (targetUnitData == null)
                        {
                            unitState = eUnitState.move;
                            break;
                        }
                    }

                    if (tempAttackSpeed >= unit_attackSpeed)
                    {
                        if (unit_type.Equals(1))    // 원거리 유닛 테스트
                        {
                            arrowMove.gameObject.transform.parent.gameObject.SetActive(true);

                            sAnimState.SetAnimation(0, "attack", false);

                            arrowMove.ShotArrow(targetUnitData.transform);
                        }
                        else
                        {
                            // 공격 애니메이션 실행
                            if (unit_team.Equals(0))
                            {
                                sAnimState.SetAnimation(0, "attack", false);
                            }
                            else
                            {
                                sAnimState.SetAnimation(0, "Attack", false);
                            }

                            // 데미지
                            targetUnitData.TakeDamage(DamageCalcul());
                        }

                        isAttackAnim = true;
                        tempAttackSpeed = 0;
                    }

                    break;
            }
            yield return null;
        }
    }



    // 투사체 목표에 도착
    public void arrowArrival()
    {
        if (targetUnitData == null)
        {
            unitState = eUnitState.move;
            return;
        }

        targetUnitData.TakeDamage(DamageCalcul());
        arrowEnd();
        //CancelInvoke("arrowEnd");
        //Invoke("arrowEnd", 1.0f);
    }

    // 투사체 제거
    public void arrowEnd()
    {
        arrowMove.gameObject.transform.parent.gameObject.SetActive(false);
    }

    // 데미지 계산
    float DamageCalcul()
    {
        bool tempCri = false;

        float tempDamage = 0;

        // 공격 대상 스탯
        float targetPhysicDef = targetUnitData.unit_PhysicalDef;        // 물리 방어력
        float targetMagicDef = targetUnitData.unit_MagicDef;            // 마법 방어력
        int targetElement = targetUnitData.unit_Element;                // 속성
        int targetCounterElement = targetUnitData.unit_counter_element; // 약점속성


        // 크리티컬
        if(Random.Range(1, 100) >= unit_CriticalRate)
        {
            tempCri = true;
        }

        // 상성 확인
        if (unit_Element.Equals(targetElement))
        {
            tempDamage = unit_Damage;
        }
        else
        {
            if (unit_Element.Equals(targetCounterElement))
            {
                // 상성 우위
                tempDamage = damage_up;
            }
            else
            {
                // 상성 열세
                tempDamage = damage_down;
            }
        }

        // 데미지 - 방어력
        if (unit_DamageType.Equals(0))
        {
            tempDamage -= targetPhysicDef;
        }
        else
        {
            tempDamage -= targetMagicDef;
        }


        if (tempDamage < 0)
        {
            tempDamage = 0;
        }

        //if(unit_id == 0 && unit_team == 0)
        Debug.Log("최종 데미지 : " + tempDamage + "/" + this.unit_team);

        return tempDamage;
    }


    // 데미지 받음
    public void TakeDamage(float damage)
    {
        this.unit_HP -= damage;

        if (unit_HP <= 0)
        {
            Death();
            HP_Bar.localScale = new Vector2(0, 1);
        }
        else
        {
            HP_Bar.localScale = new Vector2(Mathf.Lerp(0, 1, (unit_HP / unit_Max_HP)), 1);
            //Debug.Log("Max HP : " + unit_Max_HP + " HP : " + unit_HP + " per " + (unit_HP / unit_Max_HP));
        }
    }


    public void Death()
    {
        // 플레이어 or 적 본부가 부서졌을경우
        if (this.unit_type == 2)
        {
            if (this.unit_team == 1)
            {
                GameStateManager.Instance.Game_Clear(true);
            }
            else
            {
                GameStateManager.Instance.Game_Clear(false);
            }
            return;
        }

        if (this.unit_team == 1)
        {
            GameManager.Instance.User.Have_gold += (this.level + 50000);
        }

        HitBox.gameObject.SetActive(false);

        unitState = eUnitState.death;

        if (this.unit_team == 1)
        {
            sAnimState.SetAnimation(0, "DeathBackward", false);
        }

        //if (this.gameObject == UnitDataManager.Instance.headUnitTrans)
        //{
        //    UnitDataManager.Instance.headUnitTrans = null;
        //}
        UnitDataManager.Instance.CurrSpawnedUnitTrans.Remove(this.transform);
        StopAllCoroutines();
        StartCoroutine(this.Wait_DeathTime());
    }

    /// <summary>
    /// 사망 애니메이션 발동 2초후 사라짐, 8초후 리스폰 (사망 10초후 리스폰)
    /// </summary>
    /// <returns></returns>
    public IEnumerator Wait_DeathTime()
    {
        yield return new WaitForSeconds(2f);

        UnitSpawnManager.Instance.Respawn(this.gameObject);
        this.unit_HP = unit_Max_HP;
        unitState = eUnitState.move;
        this.gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        if (sAnimState != null)
        {
            // 스파인 애니메이션 초기화
            sAnimState.ClearTrack(0);
            sAnim.skeleton.SetToSetupPose();
        }
    }
}
