using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameMode
{
    LOBY,
    INFINITE,
    STORY,
    MULTI
}

public enum StageMode
{
    NONE,
    ORIGINAL,
    AVOID,
    CLEAN_DUST
}

public class StageLoader : MonoBehaviour
{
    private static StageLoader instance;
    public static StageLoader Instance()
	{
        if (instance == null)
        {
            return null;
        }
        else return instance;
	}


    private List<Dictionary<string, object>> stageDataList;

    public GameMode mode = GameMode.LOBY;

    // stage_id,mode_id,time_limit,min_score,cnt_x_topping,goal_topping
    /* Stage Data */
    public string stageId;
    public StageMode stageMode;
    public int timeLimit;
    public int minScore;
    public int cntXTopping;
    public int goalTopping;
    public int obstacleCount;


    void Awake()
	{
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
	}

    void Start()
    {
        stageDataList = CSVReader.Read("Datas/stage1");
        
    }

    public void SetStageData(int stageNumber)
	{
        Dictionary<string, object> stageData = stageDataList[stageNumber];

        stageId = stageData["stage_id"].ToString();
        timeLimit = System.Convert.ToInt32(stageData["time_limit"].ToString());
        minScore = System.Convert.ToInt32(stageData["min_score"].ToString());
        cntXTopping = System.Convert.ToInt32(stageData["cnt_x_topping"].ToString());
        goalTopping = System.Convert.ToInt32(stageData["goal_topping"].ToString());
        obstacleCount = System.Convert.ToInt32(stageData["obstacle_count"].ToString());

        switch (stageData["mode_id"])
		{
            case "original":
                stageMode = StageMode.ORIGINAL;
                break;
            case "avoid":
                stageMode = StageMode.AVOID;
                break;
            case "clean_dust":
                stageMode = StageMode.CLEAN_DUST;
                break;
            case "clean_scorch":
                stageMode = StageMode.CLEAN_DUST;
                break;
            default:
                stageMode = StageMode.ORIGINAL;
                break;
		}

        mode = GameMode.STORY;
        MoveScene("Main");
	}

    public void CleanStageData()
	{
        stageId = "";
        stageMode = StageMode.NONE;
        timeLimit = 0;
        minScore = 0;
        cntXTopping = 0;
        goalTopping = 0;
	}


    public void MoveScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
