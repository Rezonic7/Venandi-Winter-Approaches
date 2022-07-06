using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Create Data/Create Equipment Data/Weapon Data")]
[System.Serializable]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string _weaponName;
    [SerializeField] private int _attackValue;

    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int AttackValue { get { return _attackValue; } set { _attackValue = value; } }
}
