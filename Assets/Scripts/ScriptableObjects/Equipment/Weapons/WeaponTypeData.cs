using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Type Data", menuName = "Create Data/Create Equipment Data/Weapon Type Data")]
[System.Serializable]
public class WeaponTypeData : ScriptableObject
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private WeaponTypes _weaponType;
    [SerializeField] private GameObject _modelPrefab;
    [SerializeField] private Sprite _imageSpirte;
    
    public WeaponData WeaponData { get { return _weaponData; } set { _weaponData = value; } }
    public WeaponTypes WeaponType { get { return _weaponType; } set { _weaponType = value; } }
    public GameObject ModelPrefab { get { return _modelPrefab; } set { _modelPrefab = value; } }
    public Sprite ImageSprite { get { return _imageSpirte; } set { _imageSpirte = value; } }

    public enum WeaponTypes
    {
        LightClub,
        HeavyClub,
        Bow,
        Spear
    }
}
