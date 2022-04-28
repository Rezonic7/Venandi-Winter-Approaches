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

    [SerializeField] private WeaponTypeData _weaponData;
    [SerializeField] private ArmorData _armorData;
    private int _totalDamage;
    private int _totalArmor;

    [SerializeField] private GameObject armorParent;

    [SerializeField] private GameObject SheathedParent;
    [SerializeField] private GameObject DrawnParent;
    
    [SerializeField] private GameObject BowParent;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowOrigin;

    public WeaponTypeData WeaponData { get { return _weaponData; } set { _weaponData = value; } }
    public ArmorData ArmorData { get { return _armorData; } set { _armorData = value; } }
    public int TotalDamage { get { return _totalDamage; } }
    public int TotalArmor { get { return _totalArmor; } }

    // Start is called before the first frame update
    void Start()
    {
        baseDamage = 1;
        baseArmor = 1;

        _totalDamage = baseDamage;
        _totalArmor = baseArmor;

        if (_weaponData != null)
        {
            UpdateSpawnWeapon(_weaponData);
        }
        if (_armorData != null)
        {
            UpdateSpawnArmor(_armorData);
        }
    }
    public void UpdateSpawnArmor(ArmorData newArmorData)
    {
        _armorData = newArmorData;
        if (armorPrefab != null)
        {
            Destroy(armorPrefab);
        }
        armorPrefab = Instantiate(_armorData.ModelPrefab, armorParent.transform);

        _totalArmor = baseArmor * UpdateArmorValue(_armorData.DefenceValue);
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
        _weaponData = newWeaponData;
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponPrefab = Instantiate(_weaponData.ModelPrefab, SheathedParent.transform);

        weaponPostition = weaponPrefab.transform.localPosition;
        weaponRotation = weaponPrefab.transform.localRotation;
        weaponScale = weaponPrefab.transform.localScale;

        weaponCollider = weaponPrefab?.GetComponent<Collider>();

        weaponTotal = baseDamage * WeaponTotal(_weaponData);
        _totalDamage = weaponTotal;

        Weapons.instance.DamageValue = _totalDamage;
    }
    void ResetWeaponTransform()
    {
        weaponPrefab.transform.localPosition = weaponPostition;
        weaponPrefab.transform.localRotation = weaponRotation;
        weaponPrefab.transform.localScale = weaponScale;
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
        pA.Damage = _totalDamage;
        playerArrow.transform.LookAt(hitPos);

    }
    private int WeaponTotal(WeaponTypeData atkData)
    {
        int totalAtk = 0;
        switch (atkData.WeaponType)
        {
            case WeaponTypeData.WeaponTypes.LightClub:
                totalAtk = MotionValue(atkData.WeaponData.AttackValue, -0.1f);
                break;
            case WeaponTypeData.WeaponTypes.HeavyClub:
                totalAtk = MotionValue(atkData.WeaponData.AttackValue, 0.5f);
                break;
            case WeaponTypeData.WeaponTypes.Bow:
                totalAtk = MotionValue(atkData.WeaponData.AttackValue, 0.15f);
                break;
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
        Weapons.instance.HasDamaged = false;
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollision()
    {
        weaponCollider.enabled = false;
    }
    public void Add_MotionValue(float percentage)
    {
        _totalDamage = MotionValue(weaponTotal, percentage);
        Weapons.instance.DamageValue = TotalDamage;
    }
}
