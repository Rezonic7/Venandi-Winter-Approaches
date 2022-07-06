using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveGame()
    {
        SaveSystem.SaveData(PlayerProgress.instance);
    }
    public void SaveGame(PlayerProgress playerProgress)
    {
        SaveSystem.SaveData(playerProgress);
    }

    public void LoadGame()
    {
        //PlayerData data = SaveSystem.LoadData();
        SaveSystem.LoadData();
        //for(int i = 0; i < data.ListofPlayerWeapons.Count; i++)
        {
            //PlayerProgress.instance.ListofPlayerWeapons[i] = data.ListofPlayerWeapons[i].Weapon;
        }
    }
}
