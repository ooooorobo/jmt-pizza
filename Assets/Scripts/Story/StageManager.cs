using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int stageCount = 10;
    public Button stageButton;
    public Transform scrollView;

    private void Start()
	{
        for (int i = 0; i < stageCount; i++)
		{
            Transform newBtn = Instantiate(stageButton, Vector2.zero, Quaternion.identity).transform;
            newBtn.SetParent(scrollView);
            newBtn.GetChild(0).GetComponent<Text>().text = "Stage " + (i + 1);
            int x = i;
            newBtn.GetComponent<Button>().onClick.AddListener(() => StageLoader.Instance().SetStageData(x));
		}
	}
}
