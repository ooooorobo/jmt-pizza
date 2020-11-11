using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;


public class LobbyManager : MonoBehaviour
{
    [Header("UI")]
    public Text userCoin;
    
    private UserData userData;
    
// example
    void Start()
    {
        // 뒤끝 백엔드 초기화
        // [.net4][il2cpp] 사용 시 필수 사용
        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                // example
                // 버전체크 -> 업데이트
                Debug.Log("뒤끝 서버 초기화 완료");
                Backend.BMember.GuestLogin();
                
                BackendReturnObject rankList = Backend.Rank.GetRankByUuid("b0a52b30-235c-11eb-87fa-6f54f412f5f8");
                BackendReturnObject myRank = Backend.Rank.GetMyRank("b0a52b30-235c-11eb-87fa-6f54f412f5f8");

                JsonData rankRows = rankList.GetReturnValuetoJSON()["rows"];
                for (int i = 0; i < rankRows.Count; i++)
                {
                    Debug.Log("hi");
                    Debug.Log(rankRows["score"]);
                }
            }
            // 초기화 실패한 경우 실행
            else
            {
                Debug.Log("뒤끝 서버 초기화 실패");
            }
        });
        
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

