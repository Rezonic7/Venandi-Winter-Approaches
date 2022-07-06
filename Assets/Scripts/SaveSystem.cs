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

        //PlayerData data = new PlayerData(playerProgress);

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
            PlayerProgress.instance.LoadPlayerProgress();


            //PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            //return data;
        }
        else
        {
            Debug.Log("No save data found");
            //return null;
        }
    }
}
