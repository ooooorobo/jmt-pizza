using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class MyPageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BackendReturnObject rankList = Backend.Rank.GetRankByUuid(Environment.InfiniteRankUuid);
        BackendReturnObject myRank = Backend.Rank.GetMyRank(Environment.InfiniteRankUuid);

        JsonData rankRows = rankList.GetReturnValuetoJSON()["rows"];
        for (int i = 0; i < rankRows.Count; i++)
        {
            Debug.Log(rankRows["score"]);
            Debug.Log(rankRows["nickname"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
