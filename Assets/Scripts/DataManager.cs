using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public Text rowInput;
    public Text colInput;
    public Text initSpeedInput;
    public Text accelInput;
    public Text spawnDelayInput;
    public Text destroyInput;
    public Text oScoreInput;
    public Text xScoreInput;
    public Text initScoreInput;
    public Text cheeseGoalInput;

    public void onLoadInputData()
    {
        GameManager gm = GameManager.Instance();

        try
        {
            int row = Convert.ToInt32(rowInput.text);
            if (row > 0)
                gm.row = row;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        try
        {
            int col = Convert.ToInt32(colInput.text);
            if (col > 0)
                gm.column = col;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            float speed = Convert.ToSingle(initSpeedInput.text);
            if (speed > 0)
                gm.speed = speed;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            float accel = Convert.ToSingle(accelInput.text);
            if (accel > 0)
                gm.accelerate = accel;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            float destroy = Convert.ToSingle(destroyInput.text);
            if (destroy > 0)
                gm.destroydelay = destroy;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            int oscore = Convert.ToInt32(oScoreInput.text);
            if (oscore > 0)
                gm.oToppingScore = oscore;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            int xscore = Convert.ToInt32(xScoreInput.text);
            if (xscore > 0)
                gm.xToppingScore = xscore;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            int initscore = Convert.ToInt32(initScoreInput.text);
            if (initscore > 0)
                gm.initialScore = initscore;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            int cheese = Convert.ToInt32(cheeseGoalInput.text);
            if (cheese > 0)
                gm.cheeseGoal = cheese;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
