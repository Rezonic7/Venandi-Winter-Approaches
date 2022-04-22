using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Equipment : Singleton<Player_Equipment>
{
    private int baseDamage;
    private int baseArmor;

    public WeaponTypeData WeaponData;
    private int ArmorData;
    private int weaponTotal;
    public int totalDamage;
    public int totalArmor;

    public GameObject SheathedParent;
    public GameObject DrawnParent;

    private GameObject weaponPrefab;
    private Collider weaponCollider;


    private Vector3 weaponPostition;
    private Quaternion weaponRotation;
    private Vector3 weaponScale;

    // Start is called before the first frame update
    void Start()
    {
        baseDamage = 1;
        baseArmor = 1;


        UpdateSpawnWeapon(WeaponData);

        totalArmor = baseArmor + ArmorData;

    }
    public void WeaponDrawn(bool isDrawn)
    {
        if(isDrawn)
        {
            weaponPrefab.transform.SetParent(DrawnParent.transform);
            ResetWeaponTransform();
        }
        else
        {
            weaponPrefab.transform.SetParent(SheathedParent.transform);
            ResetWeaponTransform();
        }
    }
    public void UpdateSpawnWeapon(WeaponTypeData newWeaponData)
    {
        WeaponData = newWeaponData;
        if(weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponPrefab = Instantiate(WeaponData.ModelPrefab, SheathedParent.transform);

        weaponPostition = weaponPrefab.transform.localPosition;
        weaponRotation = weaponPrefab.transform.localRotation;
        weaponScale = weaponPrefab.transform.localScale;

        weaponCollider = weaponPrefab?.GetComponent<Collider>();

        weaponTotal = baseDamage + WeaponTotal(WeaponData);
        totalDamage = weaponTotal;

        Weapons.instance.damageValue = totalDamage;
    }
    void ResetWeaponTransform()
    {
        weaponPrefab.transform.localPosition = weaponPostition;
        weaponPrefab.transform.localRotation = weaponRotation;
        weaponPrefab.transform.localScale = weaponScale;
    }

    public int WeaponTotal(WeaponTypeData atkData)
    {
        int totalAtk = 0;
        switch (atkData.WeaponType)
        {
            case WeaponTypeData.WeaponTypes.LightClub:
                return totalAtk = MotionValue(atkData.WeaponData.AttackValue, -0.1f);
                break;
            case WeaponTypeData.WeaponTypes.HeavyClub:
                return totalAtk = MotionValue(atkData.WeaponData.AttackValue, 0.5f);
                break;
        }
        return totalAtk;
    }

    private int MotionValue(int WeaponDamage, float PercentageModifier)
    {
        int totalValue = (int)((float)WeaponDamage + (float)((float)WeaponDamage * PercentageModifier));
        return totalValue;
    }

    public void UpdateArmor(int newArmorData)
    {
        ArmorData = newArmorData;
        totalArmor = baseArmor + ArmorData;
    }

    public void EnableWeaponCollision()
    {
        Weapons.instance.hasDamaged = false;
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollision()
    {
        weaponCollider.enabled = false;
    }
    public void DefMV()
    {
        totalDamage = weaponTotal;
        Weapons.instance.damageValue = totalDamage;
    }

    public void LC_HMV()
    {
        totalDamage = MotionValue(weaponTotal, 0.3f);
        Weapons.instance.damageValue = totalDamage;
    }

    public void HC_HMW()
    {
        totalDamage = MotionValue(weaponTotal, 0.1f);
        Weapons.instance.damageValue = totalDamage;
    }
    public void HC_HMW2()
    {
        totalDamage = MotionValue(weaponTotal, 0.2f);
        Weapons.instance.damageValue = totalDamage;
    }

}
