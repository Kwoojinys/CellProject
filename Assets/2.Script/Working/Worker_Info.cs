using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Worker_Info {

    private int id;
    private float level; // 0이면 구매 X, 1이상 구매 O
    private float prod_gold; // 생산 골드
    private float req_gold; // 구매시 필요 골드
    private float base_upgold; // 업그레이드시 기본 필요 골드
    private float base_prod_gold; //  기본 생산 골드
    private float upgold; // 업그레이드 필요 골드
    private string name;

    public Worker_Info(int id, float level, float base_prod_gold, float req_gold, float base_upgold)
    {
        this.id = id;
        this.level = level;
        this.base_prod_gold = base_prod_gold;
        this.req_gold = req_gold;
        this.base_upgold = base_upgold;
    }

    public Worker_Info(string json)
    {
        var Data = JSON.Parse(json);

        this.id = Data["id"];
        this.name = Data["name"];
        this.req_gold = Data["req_gold"];
        this.base_upgold = Data["base_upgold"];
        this.base_prod_gold = Data["base_prod_gold"];
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    /// <summary>
    /// 일꾼 레벨업시마다 레벨업 관련 정보를 갱신하는 트리거가 포함됨
    /// </summary>
    public float Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
            float level_increase = Mathf.Pow(GlobalVar.level_var, (level - 1));
            prod_gold = base_prod_gold * level_increase;
            upgold = base_upgold * level_increase;
            WorkerManager.Instance.Refresh_Income();
        }
    }

    /// <summary>
    /// 현재 일꾼이 생산하는 골드
    /// </summary>
    public float Prod_gold
    {
        get
        {
            return prod_gold;
        }

        set
        {
            prod_gold = value;
        }
    }

    /// <summary>
    /// 일꾼 구매에 필요한 골드
    /// </summary>
    public float Req_gold
    {
        get
        {
            return req_gold;
        }

        set
        {
            req_gold = value;
        }
    }
    /// <summary>
    /// 업그레이드에 필요한 기본 골드(1레벨상태)
    /// </summary>
    public float Base_upgold
    {
        get
        {
            return base_upgold;
        }

        set
        {
            base_upgold = value;
        }
    }

    /// <summary>
    /// 일꾼이 생산하는 기본 골드(1레벨상태)
    /// </summary>
    public float Base_prod_gold
    {
        get
        {
            return base_prod_gold;
        }

        set
        {
            base_prod_gold = value;
        }
    }

    /// <summary>
    /// 일꾼 업그레이드에 필요한 골드
    /// </summary>
    public float Upgold
    {
        get
        {
            return upgold;
        }

        set
        {
            upgold = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
}
