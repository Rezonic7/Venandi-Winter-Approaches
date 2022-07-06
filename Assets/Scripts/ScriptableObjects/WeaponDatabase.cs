using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Database", menuName = "Create Data/Database/Weapon Database")]
public class WeaponDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public WeaponTypeData[] weapons;
    public Dictionary<WeaponTypeData, int> GetID = new Dictionary<WeaponTypeData, int>();
    public Dictionary<int, WeaponTypeData> GetWeapon = new Dictionary<int, WeaponTypeData>();

    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<WeaponTypeData, int>();
        GetWeapon = new Dictionary<int, WeaponTypeData>();

        for(int i = 0; i < weapons.Length; i++)
        {
            GetID.Add(weapons[i], i);
            GetWeapon.Add(i, weapons[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
