using System;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    public Transform rankViewportContentParent;
    public Transform rankMine;
    
    void Start()
    {
    // 초기화 성공한 경우 실행
        if (Backend.IsInitialized)
        {
            // 버전체크 -> 업데이트
            Debug.Log("뒤끝 서버 초기화 완료");
            InitializeRankUI();
        }
        // 초기화 실패한 경우 실행
        else
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
                    // example
                    // 버전체크 -> 업데이트
                    Debug.Log("뒤끝 서버 초기화 완료");
                    Backend.BMember.GuestLogin();
                    
                    InitializeRankUI();
                }
                // 초기화 실패한 경우 실행
                else
                {
                    Debug.Log("뒤끝 서버 초기화 실패");
                }
            });
        }
    }

    private void InitializeRankUI()
    {
        BackendReturnObject rankList = Backend.Rank.GetRankByUuid(Environment.InfiniteRankUuid);
        BackendReturnObject myRank = Backend.Rank.GetMyRank(Environment.InfiniteRankUuid);

        JsonData rankRows = rankList.GetReturnValuetoJSON()["rows"];
        JsonData myRankRows = myRank.GetReturnValuetoJSON()["rows"];

        foreach (string key in rankRows[0].Keys)
        {
            Debug.Log(key);
        }
            
        for (int i = 0; i < rankRows.Count; i++)
        {
            Transform rankObject = rankViewportContentParent.GetChild(i);
            rankObject.gameObject.SetActive(true);
            
            rankObject.GetChild(0).GetComponent<Text>().text = rankRows[i]["rank"]?["N"].ToString() ?? "0";
            rankObject.GetChild(1).GetComponent<Text>().text = rankRows[i]["nickname"]?["S"].ToString() ?? "";
            rankObject.GetChild(2).GetComponent<Text>().text = rankRows[i]["score"]?["N"].ToString() ?? "0";
        }

        rankMine.GetChild(0).GetComponent<Text>().text = myRankRows[0]["rank"]?["N"].ToString() ?? "0";
        rankMine.GetChild(1).GetComponent<Text>().text = myRankRows[0]["nickname"]?["S"].ToString() ?? "";
        rankMine.GetChild(2).GetComponent<Text>().text = myRankRows[0]["score"]?["N"].ToString() ?? "0";

    }
}