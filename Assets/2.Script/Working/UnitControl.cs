using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;



// 각 유닛 객체 데이터
public class UnitControl : Unit_Stat
{
    [HideInInspector]
    public float tempAttackSpeed;   // 공격속도 계산용


    int direction;  // 이동방향

    float chase_MoveFloat = 0;

    Transform thisTrans;
    [Space(20)]
    public Transform targetTrans;
    public UnitControl targetUnitData;
    Transform HitBox;

    Animator anim;

    Transform unit_AttackColl;

    public SkeletonAnimation sAnim;
    public Spine.AnimationState sAnimState;

    bool isMoveAnim;
    bool isIdleAnim;
    public bool isAttackAnim;

    public Transform HP_Back;
    public Image HP_Bar;    // UI Image
    public Transform HP_Bar2D;    // 2D Sprite

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
        // HQ
        if (this.unit_type == 2)
        {
            //HP_Bar = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            HP_Bar2D = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Transform>();
            HitBox = gameObject.transform.GetChild(1);
            return;
        }

        thisTrans = this.transform;
        anim = this.gameObject.GetComponent<Animator>();

        unit_AttackColl = gameObject.transform.GetChild(1);
        //HP_Bar = gameObject.transform.GetChild(3).GetChild(0).GetComponent<Image>();
        HP_Bar2D = gameObject.transform.GetChild(3).GetChild(0).GetComponent<Transform>();

        HitBox = gameObject.transform.GetChild(2);

        sAnim = this.transform.GetChild(0).GetComponent<SkeletonAnimation>();
        sAnimState = sAnim.state;
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
        

        if(unit_type.Equals(0))
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
        else if(unit_type.Equals(1))
        {
            // 원거리
            // 근접
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

        if(unit_type.Equals(1))
        {
            arrowMove = transform.GetChild(5).GetChild(0).GetComponent<ArrowMove>();
        }


        if (unit_team.Equals(1))
        {
            sAnim.state.Event += HandleEvent;

            sAnim.state.Start += delegate (Spine.TrackEntry entry)
            {
            };
            sAnim.state.End += delegate
            {
            };
            sAnim.state.Complete += delegate
            {
            };
        }
        else
        {
            sAnimState.Event += HandleEvent;
            sAnimState.Complete += delegate
            {
                Debug.Log("complete " + sAnimState.GetCurrent(0));
            };

            sAnimState.Complete += delegate
            {
                if(sAnimState.GetCurrent(0).Equals("attack"))
                {
                    Debug.Log("Attack Complete");
                    isAttackAnim = false;
                }
                else if(sAnimState.GetCurrent(0).Equals("walk"))
                {
                    Debug.Log("walk Complete");
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


    public void INIT()
    {
        unitState = eUnitState.move;
        targetUnitData = null;
        isMoveAnim = false;

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
            switch (unitState)
            {
                case eUnitState.wait:
                    if (unit_team.Equals(0))
                    {
                        if (!isIdleAnim)
                        {
                            isIdleAnim = true;
                            // Melee Unit
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
                            // Melee Unit
                            sAnimState.SetAnimation(0, "walk", true);
                        }
                    }
                    else
                    {
                        if (!isMoveAnim)
                        {
                            isMoveAnim = true;
                            // Melee Unit
                            sAnimState.SetAnimation(0, "Move", true);
                        }
                    }
                    thisTrans.Translate(Vector3.right * direction * unit_moveSpeed * Time.timeScale);
                    break;
                case eUnitState.attack:

                    if(!isAttackAnim)
                    {
                        tempAttackSpeed += Time.deltaTime;
                    }

                    // 원거리 유닛 테스트
                    if (unit_type.Equals(1))
                    {
                        if (tempAttackSpeed >= unit_attackSpeed)
                        {
                            if (targetUnitData == null)
                            {
                                unitState = eUnitState.move;
                                break;
                            }

                            arrowMove.gameObject.transform.parent.gameObject.SetActive(true);

                            isMoveAnim = false;

                            sAnimState.SetAnimation(0, "attack", false);

                            arrowMove.ShotArrow(targetUnitData.transform);

                            tempAttackSpeed = 0;
                        }
                    }
                    else
                    {
                        if (tempAttackSpeed >= unit_attackSpeed)
                        {
                            if(unit_team.Equals(0))
                            {
                                Debug.Log("Attack Corou Start");
                            }
                            if (targetUnitData == null)
                            {
                                unitState = eUnitState.move;
                                break;
                            }

                            tempAttackSpeed = 0;
                            isMoveAnim = false;
                            isAttackAnim = true;

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


                            if (unit_team.Equals(0))
                            {
                                Debug.Log(targetUnitData.unit_HP);
                            }


                            //if (targetUnitData.unit_HP <= 0)
                            //{
                            //    targetUnitData = null;
                            //    StartCoroutine("DelayAfterEnemyDeath");
                            //}
                            //else
                            //{
                            //    targetUnitData.TakeDamage(unit_Damage);
                            //}

                            //Debug.Log(unit_team + " atack, enemy HP : " + targetUnitData.unit_HP);
                        }
                    }

                    break;
            }

            yield return null;
        }

    }

    bool AnimationCheck()
    {

        return true;
    }


    //IEnumerator DelayAfterEnemyDeath()
    //{
    //    unitState = eUnitState.wait;
    //    Debug.Log("적 죽음, 1초 대기 시작");
    //    unit_AttackColl.gameObject.SetActive(false);

    //    yield return new WaitForSeconds(5.0f);

    //    unitState = eUnitState.move;
    //    Debug.Log("대기 종료");
    //    unit_AttackColl.gameObject.SetActive(true);
    //}

    public void arrowArrival()
    {
        targetUnitData.TakeDamage(DamageCalcul());
        CancelInvoke("arrowEnd");
        Invoke("arrowEnd", 1.0f);
    }

    public void arrowEnd()
    {
        arrowMove.gameObject.transform.parent.gameObject.SetActive(false);
        tempAttackSpeed = unit_attackSpeed;
    }

    float DamageCalcul()
    {
        bool tempCri = false;

        float tempDamage = 0;

        float targetPhysicDef = targetUnitData.unit_PhysicalDef;
        float targetMagicDef = targetUnitData.unit_MagicDef;
        int targetElement = targetUnitData.unit_Element;
        int targetCounterElement = targetUnitData.unit_counter_element;

        //Debug.Log("========================================================");
        //Debug.Log("공격 " + gameObject.name + " -> " + targetUnitData.name);
        //Debug.Log("기본 데미지 " + unit_Damage);

        //Debug.Log("상성 (1 < 2 < 3 < 1) " + unit_Element + " -> " + targetElement);
        // 상성 확인
        if (unit_Element.Equals(targetElement))
        {
            tempDamage = unit_Damage;
            //Debug.Log("속성 같음, 데미지 " + tempDamage);
        }
        else
        {
            if(unit_Element.Equals(targetCounterElement))
            {
                // 상성 우위
                tempDamage = damage_up;
                //Debug.Log("상성 우위, 데미지 " + tempDamage);
            }
            else
            {
                // 상성 열세
                tempDamage = damage_down;
                //Debug.Log("상성 열세, 데미지 " + tempDamage);
            }
        }

        // 데미지 - 방어력
        //Debug.Log("공격 타입 (0 : 물리 / 1 : 마법) " + unit_DamageType);
        if(unit_DamageType.Equals(0))
        {
            tempDamage -= targetPhysicDef;
            //Debug.Log("적 물리 방어력 " + targetPhysicDef);
        }
        else
        {
            tempDamage -= targetMagicDef;
            //Debug.Log("적 마법 방어력 " + targetMagicDef);
        }

        if(tempDamage < 0)
        {
            tempDamage = 0;
        }

        //Debug.Log("최종 데미지 : " + tempDamage);

        return tempDamage;
    }

    public void TakeDamage(float damage)
    {
        this.unit_HP -= damage;

        if (unit_HP <= 0)
        {
            Death();
            //HP_Bar.rectTransform.sizeDelta = new Vector2(0, 100);
            HP_Bar2D.localScale = new Vector2(0, 1);
        } else
        {
            //HP_Bar.rectTransform.sizeDelta = new Vector2(((float) (unit_HP / unit_Max_HP) * 100), 100);
            HP_Bar2D.localScale = new Vector2(Mathf.Lerp(0, 1, (unit_HP / unit_Max_HP)), 1);
            //Debug.Log("Max HP : " + unit_Max_HP + " HP : " + unit_HP + " per " + (unit_HP / unit_Max_HP));
        }
    }


    public void Death()
    {
        // 플레이어 or 적 본부가 부서졌을경우
        if(this.unit_type == 2)
        {
            if(this.unit_team == 1)
            {
                GameStateManager.Instance.Game_Clear(true);
            } else
            {
                GameStateManager.Instance.Game_Clear(false);
            }
            return;
        }

        if(this.unit_team == 1)
        {
            GameManager.Instance.User.Have_gold += (this.level +  500100);
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
    /// 사망 애니메이션 발동 2초후 사라짐, 5초후 리스폰 (사망 7초후 리스폰)
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
        if(sAnimState != null)
        {
            // 스파인 애니메이션 초기화
            sAnimState.ClearTrack(0);
            sAnim.skeleton.SetToSetupPose();
        }
    }
}
