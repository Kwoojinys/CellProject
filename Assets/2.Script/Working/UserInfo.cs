using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    private float have_gold;

    /// <summary>
    /// 유저가 가지고 있는 골드
    /// 값이 변할때마다 UI창 골드 갱신
    /// </summary>
    public float Have_gold
    {
        get
        {
            return have_gold;
        }

        set
        {
            have_gold = value;
            UIManager.Instance.Refresh_Gold(have_gold);
        }
    }
}
