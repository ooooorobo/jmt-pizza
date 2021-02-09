using System;
using BackEnd;

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
            if (this.maxScore < score && score > 0)
            {
                this.maxScore = score;
                
                Param newScore = new Param();
                newScore.Add("score", score);

                if (Backend.IsInitialized)
                {
                    Backend.GameSchemaInfo.Insert(Environment.InfiniteTableName, newScore);
                }
            }
            
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