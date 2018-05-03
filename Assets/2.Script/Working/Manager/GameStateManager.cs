using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임 진행상태 관리
public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance = null;
    public static GameStateManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("GameStateManager").AddComponent<GameStateManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    int stage;

    bool isPlaying;
    bool isStageClear;

    public enum eGameState
    {
        stagePlaying,
        stageClear,
        stageWaiting,
        stop
    }

    public eGameState gameState;


    void Start()
    {

    }

    public void Stage_Start()
    {
        UnitSpawnManager.Instance.Start_Summon();
    }

    void GameStart()
    {
        StartCoroutine("GameStateCorou");
    }

    /// <summary>
    /// 승리시 스테이지 + 1, 패배시 그대로 (몹 레벨은 스테이지에 비례)
    /// </summary>
    /// <param name="Victory"></param>
    public void Game_Clear(bool Victory)
    {
        UIManager.Instance.Block_UI.GetComponent<Animation>().Play();

        UnitSpawnManager.Instance.Reset_All_Char();

        if (Victory)
        {
            GameManager.Instance.Current_Stage += 1;
            UIManager.Instance.Stage_Text.text = GameManager.Instance.Current_Stage + " Stage";
        }
        else
        {
            Debug.Log("Loooooooooooooooooooooooooooooooooooose");
        }
        Stage_Start();

    }

    IEnumerator GameStateCorou()
    {
        while(true)
        {
            if(gameState == eGameState.stop)
            {
                break;
            }

            switch(gameState)
            {
                case eGameState.stagePlaying:

                    break;
                case eGameState.stageClear:
                    UnitSpawnManager.Instance.Stop_Summon();
                    break;
                case eGameState.stageWaiting:
                    break;
            }

            yield return null;
        }
    }


}
