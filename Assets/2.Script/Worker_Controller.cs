using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Worker_Controller : MonoBehaviour {

    public int This_Id;

    public GameObject Buy_Btn;
    public GameObject LevelUp_Btn;

    public Text LevelUp_Req_Gold;
    public Text Buy_Req_Gold;
    public Text Prod_Gold;
    public Text Name;

    public Image Worker_Face;

    float Req_Gold = 0;
    float UpGold = 0;

    public void Set_Info(Worker_Info This_Info)
    {
        This_Id = This_Info.Id;
        LevelUp_Btn.SetActive(false);
        Name.text = This_Info.Name;
    }

    /// <summary>
    /// 일꾼 메뉴에서 보여지는 정보를 갱신
    /// 워커 생성 / 레벨업 / 구매시 갱신됨
    /// </summary>
    public void Refresh_Info()
    {
        Worker_Info Info = WorkerManager.Instance.Worker_List.Find(x => x.Id == This_Id);

        Buy_Req_Gold.text = GameManager.Instance.ChangeValue(Info.Req_gold.ToString());
        LevelUp_Req_Gold.text = GameManager.Instance.ChangeValue(Info.Upgold.ToString());
        Prod_Gold.text = GameManager.Instance.ChangeValue(Info.Base_prod_gold.ToString()) + " g/s";

        Worker_Face.sprite = DataManager.Instance.GetSprite(Info.Face_sprite);

        if (Info.Level >= 1)
        {
            Buy_Btn.SetActive(false);
            LevelUp_Btn.SetActive(true);
            Prod_Gold.text = GameManager.Instance.ChangeValue(Info.Prod_gold.ToString()) + " g/s";

        }

        Req_Gold = Info.Req_gold;
        UpGold = Info.Upgold;
        Refresh_Avaliable();
    }

    public void Refresh_Avaliable()
    {
        if (Req_Gold > GameManager.Instance.User.Have_gold)
        {
            Buy_Btn.GetComponent<Button>().enabled = false;
            Buy_Btn.GetComponent<Image>().color = Color.gray;
        } else
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

    public void Level_Up()
    {
        WorkerManager.Instance.Worker_LevelUp(This_Id);
    }

    public void Buy()
    {
        WorkerManager.Instance.Buy_Worker(This_Id);
    }

}
