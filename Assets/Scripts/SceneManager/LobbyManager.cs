using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("UI")]
    public Text userCoin;
    
    private UserData userData;

    void Start()
    {
        userData = 
            (UserData) Assets.Scripts.Data.DataManager.GetDataFromJson<UserData>("userData") 
            ?? new UserData();
        userCoin.text = userData.coin.ToString();
    }

    public void MoveScene(string sceneName)
    {
        LoadingSceneManager.LoadScene(sceneName);
    }
}
