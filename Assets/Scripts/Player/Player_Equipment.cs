using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Equipment : Singleton<Player_Equipment>
{
    private int baseDamage;
    private int baseArmor;
    private int weaponTotal;

    private GameObject weaponPrefab;
    private Collider weaponCollider;

    private GameObject armorPrefab;

    private Vector3 weaponPostition;
    private Quaternion weaponRotation;
    private Vector3 weaponScale;

    public WeaponTypeData weaponData;
    public ArmorData armorData;
    public int totalDamage;
    public int totalArmor;

    public GameObject armorParent;
    public GameObject SheathedParent;
    public GameObject DrawnParent;
    public GameObject BowParent;

    public GameObject arrowPrefab;
    public GameObject arrowOrigin;
    public Transform arrowDirection;


    // Start is called before the first frame update
    void Start()
    {
        baseDamage = 1;
        baseArmor = 1;

        totalDamage = baseDamage;
        totalArmor = baseArmor;

        if (weaponData != null)
        {
            UpdateSpawnWeapon(weaponData);
        }
        if (armorData != null)
        {
            UpdateSpawnArmor(armorData);
        }
    }

    public void SpawnArrow()
    {
        float angle = 0f;
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(arrowOrigin.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            hitPos = hit.point;
            Debug.DrawLine(arrowOrigin.transform.position, hit.point, Color.blue, 5f);
            Vector3 direction = hit.point - arrowOrigin.transform.position;
            angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        GameObject playerArrow = Instantiate(arrowPrefab, arrowOrigin.transform.position, rotation);
        PlayerArrow pA = playerArrow.GetComponent<PlayerArrow>();
        pA.damage = totalDamage;
        playerArrow.transform.LookAt(hitPos);

    }

    public void UpdateSpawnArmor(ArmorData newArmorData)
    {
        armorData = newArmorData;
        if (armorPrefab != null)
        {
            Destroy(armorPrefab);
        }
        armorPrefab = Instantiate(armorData.ModelPrefab, armorParent.transform);

        totalArmor = baseArmor * UpdateArmorValue(armorData.DefenceValue);
    }

    private int UpdateArmorValue(int newArmorValue)
    {
        int newValue = baseArmor * newArmorValue;
        return newValue;
    }

    public void WeaponDrawn(bool isDrawn)
    {
        if (isDrawn)
        {
            weaponPrefab.transform.SetParent(DrawnParent.transform);
            ResetWeaponTransform();
        }
        else
        {
            StartCoroutine(SheathWeapon());
        }
    }
    public void BowDrawn(bool isDrawn)
    {
        if (isDrawn)
        {
            weaponPrefab.transform.SetParent(BowParent.transform);
            ResetWeaponTransform();
        }
        else
        {
            StartCoroutine(SheathWeapon());
        }
    }
    IEnumerator SheathWeapon()
    {
        yield return new WaitForSeconds(0.5f);
        weaponPrefab.transform.SetParent(SheathedParent.transform);
        ResetWeaponTransform();
    }
    public void UpdateSpawnWeapon(WeaponTypeData newWeaponData)
    {
        weaponData = newWeaponData;
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponPrefab = Instantiate(weaponData.ModelPrefab, SheathedParent.transform);

        weaponPostition = weaponPrefab.transform.localPosition;
        weaponRotation = weaponPrefab.transform.localRotation;
        weaponScale = weaponPrefab.transform.localScale;

        weaponCollider = weaponPrefab?.GetComponent<Collider>();

        weaponTotal = baseDamage * WeaponTotal(weaponData);
        totalDamage = weaponTotal;

        Weapons.instance.damageValue = totalDamage;
    }

    void ResetWeaponTransform()
    {
        weaponPrefab.transform.localPosition = weaponPostition;
        weaponPrefab.transform.localRotation = weaponRotation;
        weaponPrefab.transform.localScale = weaponScale;
    }

    private int WeaponTotal(WeaponTypeData atkData)
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
            case WeaponTypeData.WeaponTypes.Bow:
                return totalAtk = MotionValue(atkData.WeaponData.AttackValue, 0.15f);
        }
        return totalAtk;
    }

    private int MotionValue(int WeaponDamage, float PercentageModifier)
    {
        int totalValue = (int)((float)WeaponDamage + (float)((float)WeaponDamage * PercentageModifier));
        return totalValue;
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
    public void Add_MotionValue(float percentage)
    {
        totalDamage = MotionValue(weaponTotal, percentage);
        Weapons.instance.damageValue = totalDamage;
    }
}
