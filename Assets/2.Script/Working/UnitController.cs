using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour {

    public int This_Id;

    public GameObject Buy_Btn;
    public GameObject LevelUp_Btn;

    public Text LevelUp_Req_Gold;
    public Text Buy_Req_Gold;
    public Text Level;

    float Req_Gold = 0;
    float UpGold = 0;

    public void Set_Info(UnitControl This_Info)
    {
        This_Id = This_Info.unit_id;
        LevelUp_Btn.SetActive(false);
    }

    /// <summary>
    /// 병사 메뉴에서 보여지는 정보를 갱신
    /// 병사 생성 / 레벨업 / 구매시 갱신됨
    /// </summary>
    public void Refresh_Info()
    {
        UnitControl Info = UnitDataManager.Instance.PlayerSpawnUnitList.Find(x => x.unit_id == This_Id);

        Buy_Req_Gold.text = GameManager.Instance.ChangeValue(Info.unit_req_gold.ToString());
        LevelUp_Req_Gold.text = GameManager.Instance.ChangeValue(Info.upgold.ToString());
        Level.text = "Lv. " + Info.level.ToString();

        if (Info.level >= 1)
        {
            Buy_Btn.SetActive(false);
            LevelUp_Btn.SetActive(true);
        }

        Req_Gold = Info.unit_req_gold;
        UpGold = Info.upgold;
        Refresh_Avaliable();
    }

    public void Refresh_Avaliable()
    {
        if (Req_Gold > GameManager.Instance.User.Have_gold)
        {
            Buy_Btn.GetComponent<Button>().enabled = false;
            Buy_Btn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            Buy_Btn.GetComponent<Button>().enabled = true;
            Buy_Btn.GetComponent<Image>().color = Color.white;
        }

        if (UpGold > GameManager.Instance.User.Have_gold)
        {
            LevelUp_Btn.GetComponent<Button>().enabled = false;
            LevelUp_Btn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            LevelUp_Btn.GetComponent<Button>().enabled = true;
            LevelUp_Btn.GetComponent<Image>().color = Color.white;
        }
    }

    // 유닛 레벨업 버튼
    public void Level_Up()
    {
        UnitDataManager.Instance.Unit_LevelUp(This_Id);
    }

    public void Buy()
    {
        UnitDataManager.Instance.Unit_Buy(This_Id);
    }

}
