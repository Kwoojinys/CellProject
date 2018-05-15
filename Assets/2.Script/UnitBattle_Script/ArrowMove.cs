using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    UnitScript unitCont;

    public Transform lookatTrans;

    public Transform startTrans;
    public Transform targetTrans;


    float moveTime = 0.2f;
    float startTime;

    void Start()
    {
        //startTime = Time.time;

        //StartCoroutine("ArrowMoveCorou");

        unitCont = transform.parent.parent.GetComponent<UnitScript>();

        //ShotArrow(targetTrans);
    }


    public void ShotArrow(Transform _targetVec)
    {
        startTrans = this.transform.parent;
        targetTrans = _targetVec;

        this.transform.position = startTrans.position;

        startTime = Time.time;

        StartCoroutine("ArrowMoveCorou");
    }

    IEnumerator ArrowMoveCorou()
    {
        while (true)
        {
            Vector3 center = (startTrans.position + targetTrans.position) * 0.5f;
            center -= new Vector3(0, 1, 0);

            Vector3 thisRelCenter = startTrans.position - center;
            Vector3 targetRelCenter = targetTrans.position - center;

            float fracComplate = (Time.time - startTime) / moveTime;

            transform.position = Vector3.Slerp(thisRelCenter, targetRelCenter, fracComplate);
            transform.position += center;

            if (fracComplate > 1.0f)
            {
                unitCont.arrowArrival();
                break;
            }

            yield return null;
        }
    }


    void restart()
    {
        StopCoroutine("ArrowMoveCorou");

        startTime = Time.time;

        StartCoroutine("ArrowMoveCorou");
    }
}
