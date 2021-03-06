﻿using System.Collections.Generic;
using UnityEngine;

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

    public List<Dictionary<string, object>> dialogList;
    private List<Dictionary<string, object>> stageDataList;

    public Environment.GameMode mode = Environment.GameMode.LOBBY;

    // stage_id,mode_id,time_limit,min_score,cnt_x_topping,goal_topping
    /* Stage Data */
    public string stageId;
    public Environment.StageMode stageMode;
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
        dialogList = CSVReader.Read("Datas/Dialog/kingdom_script_stage_" + (stageNumber + 1));
        
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
                stageMode = Environment.StageMode.ORIGINAL;
                break;
            case "avoid":
                stageMode = Environment.StageMode.AVOID;
                break;
            case "clean_dust":
                stageMode = Environment.StageMode.CLEAN_DUST;
                break;
            case "clean_scorch":
                stageMode = Environment.StageMode.CLEAN_DUST;
                break;
            default:
                stageMode = Environment.StageMode.ORIGINAL;
                break;
		}

        mode = Environment.GameMode.STORY;
        MoveScene("StoryDialog");
	}

    public void CleanStageData()
	{
        stageId = "";
        stageMode = Environment.StageMode.ORIGINAL;
        timeLimit = 0;
        minScore = 0;
        cntXTopping = 0;
        goalTopping = 0;
	}
    
    public void MoveScene(string sceneName)
    {
        LoadingSceneManager.LoadScene(sceneName);
    }
}
