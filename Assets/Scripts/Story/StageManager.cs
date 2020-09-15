using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int stageCount = 10;
    public Button stageButton;
    public Transform scrollView;
    
    private StoryData userStageData;

    private void Start()
    {
	    userStageData = new StoryData();
		userStageData.GetStageData(stageCount);

        for (int i = 0; i < stageCount; i++)
		{
            Transform newBtn = Instantiate(stageButton, Vector2.zero, Quaternion.identity).transform;
            newBtn.SetParent(scrollView);
            
            Text btnText = newBtn.GetChild(0).GetComponent<Text>();
            btnText.text = "Stage " + (i + 1);

            if (userStageData.stages[i].isCleared) btnText.text += "\n클리어!";
            else btnText.text += "\n아직!!";
           
            int x = i;
            newBtn.GetComponent<Button>().onClick.AddListener(() => StageLoader.Instance().SetStageData(x));
		}
        
        
	}


    public void StartInfiniteMode()
    {
        StageLoader.Instance().mode = GameMode.INFINITE;
        StageLoader.Instance().MoveScene("Main");
    }
}
