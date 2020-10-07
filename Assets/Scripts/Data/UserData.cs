using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class UserData: IData
    {
        public int maxScore = 0;
        public int clearCount = 0;
        public int coin = 0;
        private string fileName = "userData";
        
        public void SaveClearData(int score)
        {
            if (this.maxScore < score)
                this.maxScore = score;
            
            clearCount++;

            DataManager.SaveIntoJson(this, fileName);
        }

        public void SaveCoin(int amount)
        {
            coin += amount;
            
            DataManager.SaveIntoJson(this, fileName);
        }
    }
}