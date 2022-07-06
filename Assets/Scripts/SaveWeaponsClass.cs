using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveWeaponsClass
{
    [SerializeField] private WeaponData _weaponData;

    [SerializeField] private string _weaponName;
    [SerializeField] private int _attackValue;

    [SerializeField] private WeaponTypeData.WeaponTypes _weaponType;
    [SerializeField] private GameObject _modelPrefab;
    [SerializeField] private Sprite _imageSpirte;

    public WeaponData WeaponData { get { return _weaponData; } set { _weaponData = value; } }

    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int AttackValue { get { return _attackValue; } set { _attackValue = value; } }

    public WeaponTypeData.WeaponTypes WeaponType { get { return _weaponType; } set { _weaponType = value; } }
    public GameObject ModelPrefab { get { return _modelPrefab; } set { _modelPrefab = value; } }
    public Sprite ImageSprite { get { return _imageSpirte; } set { _imageSpirte = value; } }

    public SaveWeaponsClass(WeaponTypeData weapon)
    {
        _weaponName = weapon.WeaponData.WeaponName;
        _attackValue = weapon.WeaponData.AttackValue;

        _weaponType = weapon.WeaponType;
        _modelPrefab = weapon.ModelPrefab;
        _imageSpirte = weapon.ImageSprite;
    }
}
