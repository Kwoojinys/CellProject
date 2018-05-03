﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛 공격 스크립트
public class UnitAttackColl : MonoBehaviour
{
    UnitControl unitData;

    const string tagPlayerHitbox = "pHitbox";
    const string tagEnemyHitbox = "eHitbox";
    string targetTag;

    public Collider2D tempColl;
    bool isCollAttack;

    public List<GameObject> collUnit = new List<GameObject>();

    void Start()
    {
        unitData = transform.parent.GetComponent<UnitControl>();

        if(targetTag == null)
        {
            tartgetTagStringInit();
        }
    }

    public void Death()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }




    //public void OnTriggerStay2D(Collider2D coll) 
    //{
    //    tempColl = coll;

    //    if (unitData.unitState.Equals(UnitControl.eUnitState.attack) || unitData.unitState.Equals(UnitControl.eUnitState.death))
    //    {
    //        return;
    //    }

    //    if (coll.CompareTag(targetTag))
    //    {
    //        unitData.targetUnitData = coll.GetComponentInParent<UnitControl>();
    //        unitData.tempAttackSpeed = unitData.unit_attackSpeed;
    //        unitData.unitState = UnitControl.eUnitState.attack;
    //    }
    //}

    bool HQCheck()
    {
        if(collUnit.Count.Equals(1))
        {
            return false;
        }

        int tempType = collUnit[0].GetComponentInParent<UnitControl>().unit_type;

        if (tempType.Equals(2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (targetTag == null)
        {
            tartgetTagStringInit();
        }

        if (coll.CompareTag(targetTag))
        {
            collUnit.Add(coll.gameObject);

            if (HQCheck())
            {
                GameObject tempObj = collUnit[0];
                collUnit.RemoveAt(0);
                collUnit.Add(tempObj);

                unitData.targetUnitData = collUnit[0].GetComponentInParent<UnitControl>();
            }

            if (collUnit.Count.Equals(1))
            {
                unitData.targetUnitData = collUnit[0].GetComponentInParent<UnitControl>();
                unitData.tempAttackSpeed = unitData.unit_attackSpeed;
                unitData.unitState = UnitControl.eUnitState.attack;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag(targetTag))
        {
            collUnit.Remove(coll.gameObject);

            if(collUnit.Count.Equals(0))
            {
                unitData.unitState = UnitControl.eUnitState.move;
            }
            else
            {
                unitData.targetUnitData = collUnit[0].GetComponentInParent<UnitControl>();
            }

            //if (unitData.unitState.Equals(UnitControl.eUnitState.attack))
            //{
            //    unitData.unitState = UnitControl.eUnitState.move;
            //}
        }
    }




    void tartgetTagStringInit()
    {
        if(unitData == null)
        {
            unitData = transform.parent.GetComponent<UnitControl>();
        }

        if (unitData.unit_team.Equals(0))
        {
            targetTag = tagEnemyHitbox;
        }
        else
        {
            targetTag = tagPlayerHitbox;
        }
    }
}