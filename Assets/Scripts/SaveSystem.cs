using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   public static void SaveData(PlayerProgress playerProgress)
   {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        List<System.Object> objects = new List<System.Object>();
        
        objects.Add(playerProgress.WeaponIDs);
        objects.Add(playerProgress.ArmorIDs);
        objects.Add(playerProgress.InventoryIDs);
        objects.Add(playerProgress.InventoryAmount);
        objects.Add(playerProgress.ChestIDs);
        objects.Add(playerProgress.ChestAmount);

        formatter.Serialize(stream, objects);
        stream.Close();
   }
   
    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/PlayerData.sav";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            object serializedObject = formatter.Deserialize(stream);
            List<System.Object> objects = serializedObject as List<System.Object>;

            PlayerProgress.instance.WeaponIDs = (List<int>)objects[0];
            PlayerProgress.instance.ArmorIDs = (List<int>)objects[1];
            PlayerProgress.instance.InventoryIDs = (List<int>)objects[2];
            PlayerProgress.instance.InventoryAmount = (List<int>)objects[3];
            PlayerProgress.instance.ChestIDs = (List<int>)objects[4];
            PlayerProgress.instance.ChestAmount = (List<int>)objects[5];
            PlayerProgress.instance.LoadPlayerProgress();

            stream.Close();

        }
        else
        {
            Debug.Log("No save data found");
        }
    }
}
