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


    private void LateUpdate()
    {
        if (!isTouch)
        {
            if (UnitDataManager.Instance.headUnitTrans)
            {
                if (!cam)
                    cam = GameObject.Find("Main Camera").GetComponent<Camera>();

                targetVector = cam.transform.position;
                targetVector.x = UnitDataManager.Instance.headUnitTrans.position.x;

                cam.transform.position = Vector3.Lerp(cam.transform.position, targetVector, smoothSpeed);
                MoveLimit();
            }
        }
    }

    private void OnMouseDown()
    {
        isTouch = true;

        prevPos = Input.mousePosition;
    }

    private void OnMouseDrag()
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

        if (isOnDrag)
        {
            cam.transform.Translate(Vector2.right * dir * moveSpeed);
            MoveLimit();
        }

        prevPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        isTouch = false;
    }

    void MoveLimit()
    {
        Vector3 temp;
        temp.x = Mathf.Clamp(cam.transform.position.x, 0, 6.4f);
        temp.y = cam.transform.position.y;
        temp.z = -10;

        cam.transform.position = temp;
    }


}
