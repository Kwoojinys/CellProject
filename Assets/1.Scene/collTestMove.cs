using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collTestMove : MonoBehaviour
{
    float spd = 0.05f;

    public GameObject testColl;
    public List<GameObject> testObj = new List<GameObject>();

    float tempDis;
    float shortDis = 1000;
    public GameObject targetObj;

    public GameObject test;
    public float distance;

    private void Start()
    {
        StartCoroutine("testCorou");
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * spd);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * spd);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * spd);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * spd);
        }

        distance = Vector2.Distance(transform.position, test.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.name + " Enter " + coll.offset);
        testColl = coll.gameObject;
        testObj.Add(coll.gameObject);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        Debug.Log(coll.name + " Exit");
        testObj.Remove(coll.gameObject);
    }

    IEnumerator testCorou()
    {
        while(true)
        {
            if(testObj.Count.Equals(0))
            {
                Debug.Log("null");
            }
            else if(testObj.Count.Equals(1))
            {
                Debug.Log("1");
            }
            else
            {
                shortDis = 1000;
                for (int i = 0; i < testObj.Count; i++)
                {
                    tempDis = Vector2.Distance(transform.position, testObj[i].transform.position);

                    if(shortDis > tempDis)
                    {
                        shortDis = tempDis;
                        targetObj = testObj[i];
                    }
                }

                Debug.Log("distance " + shortDis);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
