using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class UserData: IData
    {
        public int maxScore = 0;
        public int clearCount = 0;
        private string fileName = "userData";

        // 스테이지 클리어 데이터는 어떻게 저장하지?

        public void SaveClearData(int score)
        {
            if (this.maxScore < score)
                this.maxScore = score;
            
            clearCount++;

            DataManager.SaveIntoJson(this, fileName);
        }

        public void SetInitialData()
        {
            UserData newData = (UserData) DataManager.GetDataFromJson<UserData>(fileName);
            
            if (newData == null) return;
            maxScore = newData.maxScore;
            clearCount = newData.clearCount;
        }
    }
}