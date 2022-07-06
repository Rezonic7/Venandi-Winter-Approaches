using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Database", menuName = "Create Data/Database/Armor Database")]
public class ArmorDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ArmorData[] armors;
    public Dictionary<ArmorData, int> GetID = new Dictionary<ArmorData, int>();
    public Dictionary<int, ArmorData> GetArmors = new Dictionary<int, ArmorData>();
    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<ArmorData, int>();
        GetArmors = new Dictionary<int, ArmorData>();

        for (int i = 0; i < armors.Length; i++)
        {
            GetID.Add(armors[i], i);
            GetArmors.Add(i, armors[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
