using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : Singleton<PlayerProgress>, ISerializationCallbackReceiver
{
    public WeaponDatabase weaponDatabase;
    public ArmorDatabase armorDatabase;

    [SerializeField] private List<WeaponInventoryClass> _listOfPlayerWeapons = new List<WeaponInventoryClass>();
    public List<int> WeaponIDs;
    [SerializeField] private List<ArmorInventoryClass> _listOfPlayerArmors = new List<ArmorInventoryClass>();
    public List<int> ArmorIDs;
    public List<WeaponInventoryClass> ListOfPlayerWeapons { get { return _listOfPlayerWeapons; } set { _listOfPlayerWeapons = value; } }
    public List<ArmorInventoryClass> ListOfPlayerArmors { get { return _listOfPlayerArmors; } set { _listOfPlayerArmors = value; } }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < _listOfPlayerWeapons.Count; i++)
        {
            _listOfPlayerWeapons[i].WeaponTypeData = weaponDatabase.GetWeapon[_listOfPlayerWeapons[i].ID];
        }
        for (int i = 0; i < _listOfPlayerArmors.Count; i++)
        {
            _listOfPlayerArmors[i].ArmorData = armorDatabase.GetArmors[_listOfPlayerArmors[i].ID];
        }
        SavePlayerProgress();
    }

    public void OnBeforeSerialize()
    {

    }

    private void Start()
    {
        OnAfterDeserialize();
    }
    public void SavePlayerProgress()
    {
        WeaponIDs = new List<int>();
        for (int i = 0; i < _listOfPlayerWeapons.Count; i++)
        {
            WeaponIDs.Add(ListOfPlayerWeapons[i].ID);
        }

        ArmorIDs = new List<int>();
        for(int i = 0; i < _listOfPlayerArmors.Count; i++)
        {
            ArmorIDs.Add(ListOfPlayerArmors[i].ID);
        }

    }
    public void LoadPlayerProgress()
    {
        _listOfPlayerWeapons = new List<WeaponInventoryClass>();
        for (int i = 0; i < WeaponIDs.Count; i++)
        {
            _listOfPlayerWeapons.Add(new WeaponInventoryClass(WeaponIDs[i]));
        }
        for (int i = 0; i < ArmorIDs.Count; i++)
        {
            _listOfPlayerArmors.Add(new ArmorInventoryClass(ArmorIDs[i]));
        }
        OnAfterDeserialize();
        EquipmentManager.instance.UpdateEquipment();
    }
}
