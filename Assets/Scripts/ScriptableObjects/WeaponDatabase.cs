using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Database", menuName = "Create Data/Database/Weapon Database")]
public class WeaponDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public WeaponTypeData[] weapons;
    public Dictionary<WeaponTypeData, int> GetWeaponID = new Dictionary<WeaponTypeData, int>();
    public Dictionary<int, WeaponTypeData> GetWeaponData = new Dictionary<int, WeaponTypeData>();

    public void OnAfterDeserialize()
    {
        GetWeaponID = new Dictionary<WeaponTypeData, int>();
        GetWeaponData = new Dictionary<int, WeaponTypeData>();

        for(int i = 0; i < weapons.Length; i++)
        {
            GetWeaponID.Add(weapons[i], i);
            GetWeaponData.Add(i, weapons[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
