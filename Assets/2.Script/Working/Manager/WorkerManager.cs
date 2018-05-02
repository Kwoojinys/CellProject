using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour {

    /// <summary>
    /// 일꾼 정보 리스트
    /// </summary>
    public List<Worker_Info> Worker_List;
    /// <summary>
    /// 스크롤뷰에 배치된 일꾼 오브젝트
    /// </summary>
    public List<GameObject> WorkerObj;

    private float Worker_Income = 0;

    private static WorkerManager instance = null;
    public static WorkerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("WorkerManager").AddComponent<WorkerManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        Worker_List = new List<Worker_Info>();

        for(int i = 1; i < 8;i++)
        {
            Worker_List.Add(new Worker_Info(i, 0, i * 50 * (i * i), i * 150 * (i * i), i * 150));
        }

        StartCoroutine(this.Income());
    }


    /// <summary>
    /// 일꾼 구입. 0레벨인경우 구매되지않음 / 1레벨이상시부터 구매판정
    /// </summary>
    /// <param name="id"></param>
    public void Buy_Worker(int id)
    {
        Worker_Info Target = Worker_List.Find(x => x.Id == id);
        Target.Level = 1;
        GameManager.Instance.User.Have_gold -= Target.Req_gold;

        Debug.Log("Worker Bought.");

        Worker_Controller TargetController = UIManager.Instance.Worker_Controllers.Find(x => x.This_Id == id);
        TargetController.Refresh_Info();
    }

    /// <summary>
    /// 일꾼 레벨업. 레벨업과 동시에 고정 수익 부분 재계산
    /// </summary>
    /// <param name="id"></param>
    public void Worker_LevelUp(int id)
    {
        Worker_Info Target = Worker_List.Find(x => x.Id == id);
        Target.Level += 1;
        GameManager.Instance.User.Have_gold -= Target.Upgold;

        Debug.Log("Worker LevelUp.");

        Worker_Controller TargetController = UIManager.Instance.Worker_Controllers.Find(x => x.This_Id == id);
        TargetController.Refresh_Info();
    }

    /// <summary>
    /// 일꾼 수입 계산. 특정 일꾼 레벨업시마다 재계산하여 합산함
    /// </summary>
    public void Refresh_Income()
    {
        Worker_Income = 0;
        for (int i = 0; i < Worker_List.Count;i++)
        {
            Worker_Income += Worker_List[i].Prod_gold;
        }

        Debug.Log("Income : " + Worker_Income);
    }

    /// <summary>
    /// 일꾼 수입 트리거
    /// </summary>
    /// <returns></returns>
    public IEnumerator Income()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            GameManager.Instance.User.Have_gold += Worker_Income;
        }
    }
}
