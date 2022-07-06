using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorInventoryClass
{
    public int ID;
    public ArmorData ArmorData;

    public ArmorInventoryClass()
    {
        ID = 0;
        ArmorData = null;
    }
    public ArmorInventoryClass(int _iD)
    {
        ID = _iD;
    }
    public ArmorInventoryClass(int _iD, ArmorData _armorData)
    {
        ID = _iD;
        ArmorData = _armorData;
    }
}
