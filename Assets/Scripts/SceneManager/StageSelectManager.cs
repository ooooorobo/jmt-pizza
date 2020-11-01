using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    public int stageCount = 10;
    public Button stageButton;
    public Transform scrollView;
    
    private StoryData userStageData;

    private void Start()
    {
	    userStageData = new StoryData();
		userStageData.GetStageData(stageCount);

		InitStageButtons();
    }

    private void InitStageButtons()
    {		
	    bool isDone = true;

	    for (int i = 0; i < stageCount; i++)
	    {
		    // 스크롤 뷰에 스테이지 선택 버튼 생성
		    Transform newBtn = Instantiate(stageButton, Vector2.zero, Quaternion.identity).transform;
		    newBtn.SetParent(scrollView);
			
		    // 스테이지 선택 버튼 텍스트 지정
		    Text btnText = newBtn.GetChild(0).GetComponent<Text>();
		    btnText.text = "Stage " + (i + 1);

		    // 마지막으로 클리어한 스테이지의 직후 스테이지까지만 플레이 가능함
			if (userStageData.stages[i].isCleared)
			{
				btnText.text += "\n클리어!";
				
				int x = i;
				newBtn.GetComponent<Button>().onClick.AddListener(() => StageLoader.Instance().SetStageData(x));
			}
		    else
		    {
			    btnText.text += "\n아직!!";
			    if (isDone) isDone = false;
			    else newBtn.GetComponent<Button>().interactable = false;
		    }
	    }
    }
}
