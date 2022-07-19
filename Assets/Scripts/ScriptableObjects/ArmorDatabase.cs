using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Database", menuName = "Create Data/Database/Armor Database")]
public class ArmorDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ArmorData[] armors;
    public Dictionary<ArmorData, int> GetArmorID = new Dictionary<ArmorData, int>();
    public Dictionary<int, ArmorData> GetArmorData = new Dictionary<int, ArmorData>();
    public void OnAfterDeserialize()
    {
        GetArmorID = new Dictionary<ArmorData, int>();
        GetArmorData = new Dictionary<int, ArmorData>();

        for (int i = 0; i < armors.Length; i++)
        {
            GetArmorID.Add(armors[i], i);
            GetArmorData.Add(i, armors[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
