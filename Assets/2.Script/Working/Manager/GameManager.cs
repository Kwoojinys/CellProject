using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Loading_State
{
    Unit_Loading = 0,
    PUnit_Create = 1,
    EUnit_Create = 2,
    UI_Loading = 3,
    Game_Start = 4,
}

public class GameManager : MonoBehaviour {

    bool Loading_Complete = false;
    public UserInfo User = new UserInfo();

    public int Current_Stage = 1;

    public Loading_State m_Loading = Loading_State.Unit_Loading;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }


    public void FixedUpdate()
    {
        if (Loading_Complete) return;

        switch (m_Loading) {
            case Loading_State.Unit_Loading:
                {
                    m_Loading = Loading_State.PUnit_Create;
                    UnitDataManager.Instance.AddUnit();
                    break;
                }
            case Loading_State.PUnit_Create:
                {
                    UnitSpawnManager.Instance.Init_PlayerUnitPool();
                    break;
                }
            case Loading_State.EUnit_Create:
                {
                    UnitSpawnManager.Instance.Init_EnemyUnitPool();
                    break;
                }
            case Loading_State.UI_Loading:
                {
                    UIManager.Instance.Init();
                    break;
                }
            case Loading_State.Game_Start:
                {
                    Loading_Complete = true;
                    GameStateManager.Instance.Stage_Start();
                    Debug.Log("Game Ready!");
                    float temp = 340282300000000000000000000000000000000.0f;
                    Debug.Log(ChangeValue(temp.ToString()));
                    break;
                }
        }

    }

    /*
    public string ChangeMoney(int value)
    {
        if (value > 1000000000)
            return string.Format("{0:#.#}C", (float)value / 1000000000);
        else if (value > 1000000)
            return string.Format("{0:#.#}B", (float)value / 1000000);
        else if (value > 1000)
            return string.Format("{0:#.#}A", (float)value / 1000);
        else
            return value.ToString();
    }
    */
    /// <summary>
    /// <para>숫자를 단위변환해서 스트링으로 변환. ex ) 1300 -> 1.3A</para>
    /// float을 쓸 경우 최대값은 340,282,300,000,000,000,000,000,000,000,000,000,000
    /// </summary>
    public string ChangeValue(string number)
    {
        float _value = Mathf.Round(System.Convert.ToSingle(number));
        string value = _value.ToString("0");

        //Debug.Log("value : " + value);

        string[] unit = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        float[] cVal = new float[27];

        int index = 0;

        while (true)
        {
            string last4 = "";
            //Debug.Log("value.Length " + value.Length);
            if (value.Length >= 4)
            {
                last4 = value.Substring(value.Length - 4);
                //Debug.Log("last4 " + last4);
                float intLast4 = float.Parse(last4);

                cVal[index] = intLast4 % 1000;

                //Debug.Log("cVal[" + index + "] " + cVal[index]);

                value = value.Remove(value.Length - 3);

                //Debug.Log("value " + value);
            }
            else
            {
                cVal[index] = float.Parse(value);
                //Debug.Log("cVal[" + index + "] " + cVal[index]);
                break;
            }

            index++;
        }

        if (index > 0)
        {
            float r = cVal[index] * 1000 + cVal[index - 1];
            //Debug.Log("r " + r);
            return string.Format("{0:#.#}{1}", (float)r / 1000f, unit[index]);
        }

        return value;
    }
}
