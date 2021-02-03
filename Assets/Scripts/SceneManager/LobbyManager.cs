using System;
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

    public GameObject panUpdateNickname;
    public Text txtUpdateNicknameErr;
    public InputField txtUpdateNicknameInput;
    
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
                
                Backend.BMember.GetUserInfo( ( callback ) =>
                {
                    Debug.Log(callback.GetReturnValuetoJSON()["row"]["nickname"]);
                    if (callback.GetReturnValuetoJSON()["row"]["nickname"] == null)
                    {
                        panUpdateNickname.SetActive(true);
                    }
                });
                
                panUpdateNickname.SetActive(false);
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

    public void OnClickUpdateNickname()
    {
        Backend.BMember.CreateNickname( txtUpdateNicknameInput.text, ( callback ) =>
        {
            if (callback.IsSuccess())
            {
                panUpdateNickname.SetActive(false);
            }
            else
            {
                txtUpdateNicknameErr.text = callback.GetMessage();
            }
        });
    }

    public void MoveScene(string sceneName)
    {
        LoadingSceneManager.LoadScene(sceneName);
    }
}

