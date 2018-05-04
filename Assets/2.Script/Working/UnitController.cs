using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour {

    public int This_Id;

    public GameObject Buy_Btn;
    public GameObject LevelUp_Btn;

    public GameObject Level10Up_Btn;
    public GameObject Level100Up_Btn;

    public Text LevelUp_Req_Gold;
    public Text Level10Up_Req_Gold;
    public Text Level100Up_Req_Gold;
    public Text Buy_Req_Gold;
    public Text Level;

    public GameObject TeamBtn;
    public GameObject ManagementBtn;

    float Req_Gold = 0;
    float UpGold = 0;
    float Up10Gold = 0;
    float Up100Gold = 0;

    private bool Clicked = false;

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
        LevelUp_Req_Gold.text = GameManager.Instance.ChangeValue(Info.upgolds[0].ToString());
        Level10Up_Req_Gold.text = GameManager.Instance.ChangeValue(Info.upgolds[1].ToString());
        Level100Up_Req_Gold.text = GameManager.Instance.ChangeValue(Info.upgolds[2].ToString());
        Level.text = "Lv. " + Info.level.ToString();

        if (Info.level >= 1)
        {
            Buy_Btn.SetActive(false);
            LevelUp_Btn.SetActive(true);
        }

        Req_Gold = Info.unit_req_gold;
        UpGold = Info.upgolds[0];
        Up10Gold = Info.upgolds[1];
        Up100Gold = Info.upgolds[2];
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

        if (Up10Gold > GameManager.Instance.User.Have_gold)
        {
            Level10Up_Btn.GetComponent<Button>().enabled = false;
            Level10Up_Btn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            Level10Up_Btn.GetComponent<Button>().enabled = true;
            Level10Up_Btn.GetComponent<Image>().color = Color.white;
        }

        if (Up100Gold > GameManager.Instance.User.Have_gold)
        {
            Level100Up_Btn.GetComponent<Button>().enabled = false;
            Level100Up_Btn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            Level100Up_Btn.GetComponent<Button>().enabled = true;
            Level100Up_Btn.GetComponent<Image>().color = Color.white;
        }
    }

    // 유닛 레벨업 버튼
    public void Level_Up(int level_tier)
    {
        UnitDataManager.Instance.Unit_LevelUp(This_Id, level_tier);

        if(Clicked)
        {
            Level100Up_Btn.SetActive(true);
            Level10Up_Btn.SetActive(true);
            StartCoroutine(this.Off_Additional_Btns());
        } else
        {
            StartCoroutine(this.Detect_Click());
        }
    }

    public IEnumerator Detect_Click()
    {
        Debug.Log("발동");

        Clicked = true;

        yield return new WaitForSecondsRealtime(2.0f);

        Clicked = false;

        StopCoroutine(this.Detect_Click());
    }

    public IEnumerator Off_Additional_Btns()
    {
        yield return new WaitForSecondsRealtime(4f);

        Level100Up_Btn.SetActive(false);
        Level10Up_Btn.SetActive(false);
    }

    public void Buy()
    {
        UnitDataManager.Instance.Unit_Buy(This_Id);
    }

    public void Change_Team()
    {
        UnitControl Info = UnitDataManager.Instance.PlayerSpawnUnitList.Find(x => x.unit_id == This_Id);

        UIManager.Instance.Change_EntryUnit(Info);
    }

    public void Change_TeamMode()
    {
        TeamBtn.SetActive(true);
        ManagementBtn.SetActive(false);
    }

    public void Off_TeamMode()
    {
        TeamBtn.SetActive(false);
        ManagementBtn.SetActive(true);
    }

}
