using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInventoryClass
{
    public int ID;
    public WeaponTypeData WeaponTypeData;

    public WeaponInventoryClass()
    {
        ID = 0;
        WeaponTypeData = null;
    }
    public WeaponInventoryClass(int _iD)
    {
        ID = _iD;
    }
    public WeaponInventoryClass(int _iD, WeaponTypeData _weaponTypeData)
    {
        ID = _iD;
        WeaponTypeData = _weaponTypeData;
    }
}
