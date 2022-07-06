using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public WeaponDatabase weaponDatabase;

    private List<SaveWeaponsClass> _listOfPlayerWeapons;

    public string[] _weaponNameList;
    public int[] _attackValueList;

    public WeaponTypeData.WeaponTypes[] _weaponTypeList;
    public GameObject[] _modelPrefabList;
    public Sprite[] _imageSpirteList;


    public List<SaveWeaponsClass> ListofPlayerWeapons { get { return _listOfPlayerWeapons; } set { _listOfPlayerWeapons = value; } }

    public PlayerData(PlayerProgress playerProgress)
    {


        //_weaponNameList = new string[playerProgress.ListofPlayerWeapons.Length];
        //_attackValueList = new int[playerProgress.ListofPlayerWeapons.Length];

        //_weaponTypeList = new WeaponTypeData.WeaponTypes[playerProgress.ListofPlayerWeapons.Length];
        //_modelPrefabList = new GameObject[playerProgress.ListofPlayerWeapons.Length];
        //_imageSpirteList = new Sprite[playerProgress.ListofPlayerWeapons.Length];

        //for (int i = 0; i < playerProgress.ListofPlayerWeapons.Length; i++)
        //{
        //    _weaponNameList[i] = playerProgress.ListofPlayerWeapons[i].WeaponData.WeaponName;
        //    _attackValueList[i] = playerProgress.ListofPlayerWeapons[i].WeaponData.AttackValue;

        //    _weaponTypeList[i] = playerProgress.ListofPlayerWeapons[i].WeaponType;
        //    _modelPrefabList[i] = playerProgress.ListofPlayerWeapons[i].ModelPrefab;
        //    _imageSpirteList[i] = playerProgress.ListofPlayerWeapons[i].ImageSprite;
        //}
          

        //_listOfPlayerWeapons = playerWeapons;
    }
}
