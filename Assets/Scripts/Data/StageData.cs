using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class StageData: IData
    {
        public int number = -1;
        public int maxScore = 0;
        public bool isCleared = false;
        public void SaveClearData(int num, int score)
        {
            number = num;
            
            if (this.maxScore < score)
                this.maxScore = score;

            isCleared = true;
        }
    }

    [Serializable]
    public class StoryData : IData
    {
        public StageData[] stages = new StageData[] { };
        private static string _fileName = "stageData";

        public void InitStageData(int len)
        {
            stages = new StageData[len];

            for (int i = 0; i < len; i++)
            {
                stages[i] = new StageData();
            }
        }

        public static void SaveStageData(int stageNumber, int score)
        {
            StoryData storyData = new StoryData();

            storyData.GetStageData(10);
            storyData.stages[stageNumber].SaveClearData(stageNumber, score);

            DataManager.SaveIntoJson(storyData, _fileName);
        }

        public StoryData GetStageData(int len)
        {
            this.InitStageData(len);

            StoryData newStoryData = (StoryData) DataManager.GetDataFromJson<StoryData>(_fileName);


            if (newStoryData == null)
            {
                return this;
            }

            foreach (var stage in newStoryData.stages)
            {
                if (stage.number > -1)
                    stages[stage.number] = stage;
            }

            return this;
        }
    }
}