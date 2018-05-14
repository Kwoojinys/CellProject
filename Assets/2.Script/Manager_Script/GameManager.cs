using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Keiwando.BigInteger;

public enum Loading_State
{
    UI_Loading = 0,
    UI_LoadComplete = 1,
    Unit_Loading = 2,
    PUnit_Create = 3,
    EUnit_Create = 4,
    Battle_Loading = 5,
    Game_Start = 6,
}

public class GameManager : MonoBehaviour
{

    bool Loading_Complete = false;
    public UserInfo User = new UserInfo();

    public int Current_Stage = 1;

    public Loading_State m_Loading = Loading_State.UI_Loading;

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

    double testDouble = 2;
    double tempD = 1234567890123450000.0d;

    int index = 0;

    int indexTemp = 0;

    decimal testDe = 2;

    BigInteger big1 = new BigInteger("1234567890");

    BigInteger big2 = new BigInteger("1");

    public void FixedUpdate()
    {
        //Debug.Log("testDouble " + testDouble.ToString("0"));
        //Debug.Log(ChangeValue(testDouble));
        //testDouble *= 10;

        //Debug.Log("testDe " + testDe.ToString());
        //Debug.Log(ChangeValue(testDe));
        //testDe *= 10;

        //Debug.Log("index : " + index + " / tempD " + tempD);
        //Debug.Log(ChangeValue(tempD) + " + " + indexTemp);
        //tempD += 1000;
        //indexTemp += 1000;
        //index++;

        big1 = BigInteger.Add(big1, big1);

        Debug.Log(big1);
        

        if (Loading_Complete) return;

        switch (m_Loading)
        {
            case Loading_State.UI_Loading:
                {
                    SceneManager.LoadScene("Scene_Ui", LoadSceneMode.Additive);
                    m_Loading = Loading_State.UI_LoadComplete;
                    break;
                }
            case Loading_State.UI_LoadComplete:
                {
                    m_Loading = Loading_State.Unit_Loading;
                    break;
                }
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
            case Loading_State.Game_Start:
                {
                    Loading_Complete = true;
                    GameStateManager.Instance.Stage_Start();
                    UIManager.Instance.Init();
                    Debug.Log("Game Ready!");
                    //float temp = 340282300000000000000000000000000000000.0f;
                    //Debug.Log(ChangeValue(temp.ToString()));
                    //Debug.Log("Float " + ChangeValue(testF.ToString()) + " " + testF.ToString("0"));
                    //Debug.Log("Double " + ChangeValue(testD.ToString()) + " " + testD.ToString("0"));
                    Debug.Log("Decimal " + ChangeValue(testDC.ToString()) + " " + testDC.ToString("0"));
                    break;
                }
        }

    }

    float testF = float.MaxValue;
    double testD = double.MaxValue;
    decimal testDC = decimal.MaxValue;

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
    public string ChangeValue(double _value)
    {

        //float _value = Mathf.Round(System.Convert.ToSingle(number));
        //double _value = System.Math.Round(System.Convert.ToSingle(number));
        string value = _value.ToString("0");

        Debug.Log("value : " + value);

        string[] unit = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        float[] cVal = new float[27];

        int index = 0;

        while (true)
        {
            string last4 = "";
            if (value.Length >= 4)
            {
                last4 = value.Substring(value.Length - 4);

                float intLast4 = float.Parse(last4);

                cVal[index] = intLast4 % 1000;

                value = value.Remove(value.Length - 3);

            }
            else
            {
                cVal[index] = float.Parse(value);
                break;
            }

            index++;
        }

        if (index > 0)
        {
            float r = cVal[index] * 1000 + cVal[index - 1];
            return string.Format("{0:#.#}{1}", (float)r / 1000f, unit[index]);
        }

        return value;
    }


    public string ChangeValue(string gg)
    {
        return gg;
    }
}
