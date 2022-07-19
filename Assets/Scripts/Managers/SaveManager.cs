using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveGame(PlayerProgress playerProgress)
    {
        SaveSystem.SaveData(playerProgress);
    }

    public void LoadGame()
    {
        SaveSystem.LoadData();
    }
}
