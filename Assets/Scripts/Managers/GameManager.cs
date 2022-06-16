using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private MissionData _missionToLoad;
    public MissionData MissionToLoad { get { return _missionToLoad; } set { _missionToLoad = value; } }
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
    public void SetMissionData(MissionData mission)
    {
        _missionToLoad = mission;
    }
    public void Quit()
    {
        Application.Quit();
    }

}
