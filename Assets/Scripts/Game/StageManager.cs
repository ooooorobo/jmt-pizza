using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static Environment.GameMode gameMode = Environment.GameMode.INFINITE;
    public static  Environment.StageMode stageMode = Environment.StageMode.ORIGINAL;

    public static void SetGameMode(Environment.GameMode mode)
    {
        gameMode = mode;
    }
}
