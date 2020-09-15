using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class DataManager
    { 
        public static void SaveIntoJson(IData data, string fileName){
            string stringData = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + fileName + ".json", stringData);
        }

        public static object GetDataFromJson<T>(string fileName) where T : new()
        {
            string json;
            try
            {
                json = File.ReadAllText(Application.persistentDataPath + fileName + ".json");
            }
            catch (Exception)
            {
                return null;
            }
            
            return JsonUtility.FromJson<T>(json);
        }
    }
}