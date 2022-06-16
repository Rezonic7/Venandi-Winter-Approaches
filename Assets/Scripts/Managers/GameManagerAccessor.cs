using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerAccessor : MonoBehaviour
{
    public void Access_PlayGame()
    {
        GameManager.instance.PlayGame();
    }
    public void Access_TryAgain()
    {
        GameManager.instance.TryAgain();
    }
    public void Access_ReturnToTitle()
    {
        GameManager.instance.ReturnToTitle();
    }
    public void Access_SetMissionData(MissionData mission)
    {
        GameManager.instance.SetMissionData(mission);
    }
    public void Access_Quit()
    {
        GameManager.instance.Quit();
    }

}
