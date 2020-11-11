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
        BackendReturnObject rankList = Backend.Rank.GetRankByUuid("b0a52b30-235c-11eb-87fa-6f54f412f5f8");
        BackendReturnObject myRank = Backend.Rank.GetMyRank("b0a52b30-235c-11eb-87fa-6f54f412f5f8");

        JsonData rankRows = rankList.GetReturnValuetoJSON()["rows"];
        for (int i = 0; i < rankRows.Count; i++)
        {
            Debug.Log(rankRows["score"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
