using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 카메라 움직임
public class CameraMove : MonoBehaviour
{
    public Camera cam;

    Transform thisTrans;
    Vector3 thisVector;

    Vector3 targetVector;   // 카메라가 따라갈 선두 유닛

    Vector2 prevPos;
    Vector2 nowPos;
    Vector2 movePos;

    int dir;
    float moveSpeed = 0.1f;

    bool isTouch;
    bool isOnDrag;
    float tempDir;

    float smoothSpeed = 0.125f;

    void Start()
    {
        thisTrans = this.gameObject.transform;
    }

    void FixedUpdate()
    {
        //if (!isTouch)
        //{
        //    //if (GameStateManager.Instance.gameState == GameStateManager.eGameState.stagePlaying)
        //    //{
        //    if (UnitDataManager.Instance.headUnitTrans)
        //    {
        //        targetVector = cam.transform.position;
        //        targetVector.x = UnitDataManager.Instance.headUnitTrans.position.x;
        //        //thisVector.x = targetVector.x;
        //        cam.transform.position = targetVector;
        //        MoveLimit();
        //    }
        //    //}
        //}
    }

    private void LateUpdate()
    {
        if(!isTouch)
        {
            if (UnitDataManager.Instance.headUnitTrans)
            {
                targetVector = cam.transform.position;
                targetVector.x = UnitDataManager.Instance.headUnitTrans.position.x;

                cam.transform.position = Vector3.Lerp(cam.transform.position, targetVector, smoothSpeed);
                MoveLimit();
            }
        }
    }

    private void Update()
    {
        //if(Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);


        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            Debug.Log("Touch Phase Began");
        //            prevPos = touch.position - touch.deltaPosition;
        //            break;
        //        case TouchPhase.Moved:
        //            Debug.Log("Moved");
        //            nowPos = touch.position - touch.deltaPosition;
        //            movePos = (Vector2)(prevPos - nowPos) * moveSpeed;

        //            thisTrans.Translate(movePos);

        //            MoveLimit();

        //            prevPos = touch.position - touch.deltaPosition;
        //            break;
        //        case TouchPhase.Ended:
        //            Debug.Log("Touch Phase Ended");
        //            break;
        //    }
        //}


        if(Input.GetMouseButtonDown(0))
        {
            isTouch = true;

            prevPos = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {

            nowPos = Input.mousePosition;

            movePos = nowPos - prevPos;

            tempDir = movePos.x;

            if (tempDir > 0)
            {
                isOnDrag = true;
                dir = -1;
            }
            else if (tempDir < 0)
            {
                isOnDrag = true;
                dir = 1;
            }
            else if (tempDir.Equals(0))
            {
                isOnDrag = false;
            }

            if(isOnDrag)
            {
                cam.transform.Translate(Vector2.right * dir * moveSpeed);
                MoveLimit();
            }

            prevPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isTouch = false;
        }


    }

    void MoveLimit()
    {
        Vector3 temp;
        temp.x = Mathf.Clamp(cam.transform.position.x, 0, 6.4f);
        temp.y = cam.transform.position.y;
        temp.z = -10;

        cam.transform.position = temp;
    }

    //public void OnDragTouch()
    //{
    //    isOnDrag = true;

    //    if (prevPos == Vector2.zero)
    //    {
    //        prevPos = Input.mousePosition;
    //        return;
    //    }

    //    if(Input.mousePosition.x - prevPos.x > 0)
    //    {
    //        dir = -1;
    //    }
    //    else
    //    {
    //        dir = 1;
    //    }


    //    cam.transform.Translate(Vector2.left * dir * moveSpeed);
    //}

    //public void OnEndDragTouch()
    //{
       
    //}
    

}
