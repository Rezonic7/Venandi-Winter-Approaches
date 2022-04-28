using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponTypeData _weaponData;
    public WeaponTypeData WeaponData { get { return _weaponData; } }
}
