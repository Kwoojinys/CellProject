using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛 공격 스크립트
public class UnitAttackColl : MonoBehaviour
{
    UnitScript unitData;

    const string tagPlayerHitbox = "pHitbox";
    const string tagEnemyHitbox = "eHitbox";
    string targetTag;

    public Collider2D tempColl;
    bool isCollAttack;

    public List<GameObject> collUnit = new List<GameObject>();

    private void Awake()
    {
        unitData = transform.parent.GetComponent<UnitScript>();


        if (unitData.unit_team.Equals(0))
        {
            targetTag = tagEnemyHitbox;
        }
        else
        {
            targetTag = tagPlayerHitbox;
        }
    }

    //void Start()
    //{
    //    unitData.tempAttackSpeed = unitData.unit_attackSpeed;
    //}



    //public void Death()
    //{
    //    this.GetComponent<BoxCollider2D>().enabled = false;
    //}


    /// <summary>
    /// HQ 체크 - 
    /// 리스트의 0번 (현재 공격대상) 이 HQ 일때, HQ 의 공격 순위를 가장 마지막으로 변경
    /// </summary>
    /// <returns></returns>
    void HQCheck()
    {
        if (collUnit.Count.Equals(1))
        {
            return;
        }

        int tempType = collUnit[0].GetComponentInParent<UnitScript>().unit_type;

        if (tempType.Equals(2)) // HQ
        {
            GameObject tempObj = collUnit[0];
            collUnit.RemoveAt(0);
            collUnit.Add(tempObj);
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag(targetTag))
        {
            collUnit.Add(coll.gameObject);


            HQCheck();


            if (collUnit.Count.Equals(1))
            {
                unitData.targetUnitData = collUnit[0].GetComponentInParent<UnitScript>();
                unitData.unitState = UnitScript.eUnitState.attack;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag(targetTag))
        {
            collUnit.Remove(coll.gameObject);

            if (!collUnit.Count.Equals(0))
            {
                unitData.targetUnitData = collUnit[0].GetComponentInParent<UnitScript>();
            }
            else
            {
                unitData.targetUnitData = null;
            }
        }
    }


}
