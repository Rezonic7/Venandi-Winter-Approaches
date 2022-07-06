using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [Header("Weapons Tab")]
    [SerializeField] private WeaponEquipment currentWeapon;

    [SerializeField] private Text equippedWeaponName;
    [SerializeField] private Text equippedWeaponType;
    [SerializeField] private Text equippedWeaponDamage;

    [SerializeField] private GameObject weaponPreview;

    [SerializeField] private Text previewWeaponName;
    [SerializeField] private Text previewWeaponType;
    [SerializeField] private Text previewWeaponDamage;

    [SerializeField] private Text weaponLimit;

    [SerializeField] private GameObject weaponsHolder;
    [SerializeField] private WeaponInventoryClass[] listOfWeapons;
    
    private GameObject[] weapons;
    private WeaponDatabase weaponDatabase;
    public int currentDamageValue;

    [Header("Armor Tab")]
    [SerializeField] private ArmorEquipment currentArmor;

    [SerializeField] private Text equippedArmorName;
    [SerializeField] private Text equippedArmorDefence;

    [SerializeField] private GameObject armorPreview;

    [SerializeField] private Text previewArmorName;
    [SerializeField] private Text previewArmorDefence;

    [SerializeField] private Text armorLimit;

    [SerializeField] private GameObject armorsHolder;
    [SerializeField] private ArmorInventoryClass[] listOfArmors;

    private GameObject[] armors;
    private ArmorDatabase armorDatabase;
    public int currentArmorValue;
    private void Start()
    {
        //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS//
        #region WEAPONS
        weaponDatabase = PlayerProgress.instance.weaponDatabase;

        weapons = new GameObject[weaponsHolder.transform.childCount];
        listOfWeapons = new WeaponInventoryClass[weapons.Length];


        for (int i = 0; i < weapons.Length; i++)
        {
            listOfWeapons[i] = new WeaponInventoryClass();
        }
        for (int i = 0; i < weaponsHolder.transform.childCount; i++)
        {
            weapons[i] = weaponsHolder.transform.GetChild(i).gameObject;
        }

       
        #endregion WEAPONS
        //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS// //WEAPONS//

        //ARMORS// //ARMORS// //ARMORS// //ARMORS// //ARMORS// //ARMORS//
        #region ARMORS
        armorDatabase = PlayerProgress.instance.armorDatabase;

        armors = new GameObject[armorsHolder.transform.childCount];
        listOfArmors = new ArmorInventoryClass[armors.Length];

        for (int i = 0; i < armors.Length; i++)
        {
            listOfArmors[i] = new ArmorInventoryClass();
        }
        for (int i = 0; i < armorsHolder.transform.childCount; i++)
        {
            armors[i] = armorsHolder.transform.GetChild(i).gameObject;
        }
        //ARMORS// //ARMORS// //ARMORS// //ARMORS// //ARMORS// //ARMORS//
        UpdateEquipment();

        currentWeapon = weapons[0].GetComponent<WeaponEquipment>();
        currentDamageValue = WeaponTotal(currentWeapon.WeaponData);
        UpdateEquippedWeapon(currentWeapon, false);

        currentArmor = armors[0].GetComponent<ArmorEquipment>();
        currentArmorValue = 1 + currentArmor.ArmorData.DefenceValue;
        UpdateEquippedArmor(currentArmor, false);
        #endregion ARMORS
    }
    public void UpdateEquipment()
    {
        for (int i = 0; i < PlayerProgress.instance.ListOfPlayerWeapons.Count; i++)
        {
            listOfWeapons[i] = PlayerProgress.instance.ListOfPlayerWeapons[i];
        }
        for (int i = 0; i < PlayerProgress.instance.ListOfPlayerArmors.Count; i++)
        {
            listOfArmors[i] = PlayerProgress.instance.ListOfPlayerArmors[i];
        }
        RefreshUI();
    }
    #region Weapon
    public void AddWeapon (WeaponTypeData newWeapon)
    {
        for (int i = 0; i < listOfWeapons.Length; i++)
        {
            if (listOfWeapons[i].WeaponTypeData == null)
            {
                WeaponInventoryClass newWeaponClass = new WeaponInventoryClass(weaponDatabase.GetID[newWeapon], newWeapon);
                //CanvasManager.instance.ShowInfo("You have recieved " + newItem.ItemName + " x" + amount + "!");
                listOfWeapons[i] = newWeaponClass;
                PlayerProgress.instance.ListOfPlayerWeapons.Add(newWeaponClass);
                break;
            }
        }
        UpdateEquipment();
        PlayerProgress.instance.SavePlayerProgress();
    }
    public void UpdateEquippedWeapon(WeaponEquipment newWeaponData, bool showPreview)
    {
        equippedWeaponName.text = newWeaponData.WeaponData.name;
        equippedWeaponType.text = newWeaponData.WeaponData.WeaponType.ToString();
        equippedWeaponDamage.text = WeaponTotal(newWeaponData.WeaponData).ToString();

        currentWeapon = newWeaponData;
        currentDamageValue = WeaponTotal(currentWeapon.WeaponData);

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].gameObject != newWeaponData.gameObject)
            {
                weapons[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                weapons[i].transform.GetChild(1).GetComponent<Text>().text = "E";

            }
        }

        if(showPreview)
        {
            ShowWeaponPreview(newWeaponData);
        }
    }

    public void ShowWeaponPreview(WeaponEquipment newWeaponData)
    {
        weaponPreview.SetActive(true);

        previewWeaponName.text = newWeaponData.WeaponData.name;
        previewWeaponType.text = newWeaponData.WeaponData.WeaponType.ToString();
        previewWeaponDamage.text = WeaponTotal(newWeaponData.WeaponData).ToString();

        if (currentDamageValue > WeaponTotal(newWeaponData.WeaponData))
        {
            previewWeaponDamage.color = Color.red;
        }
        else if (currentDamageValue < WeaponTotal(newWeaponData.WeaponData))
        {
            previewWeaponDamage.color = Color.green;
        }
        else
        {
            previewWeaponDamage.color = Color.white;
        }
    }
    public void HideWeaponPreview()
    {
        weaponPreview.SetActive(false);
    }
    #endregion Weapon

    #region Armor
    public void UpdateEquippedArmor(ArmorEquipment newArmorData, bool showPreview)
    {
        equippedArmorName.text = newArmorData.ArmorData.name;
        equippedArmorDefence.text = (1 + newArmorData.ArmorData.DefenceValue).ToString();

        currentArmor = newArmorData;
        currentArmorValue = 1 + newArmorData.ArmorData.DefenceValue;

        for (int i = 0; i < armors.Length; i++)
        {
            if (armors[i].gameObject != newArmorData.gameObject)
            {
                armors[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                armors[i].transform.GetChild(1).GetComponent<Text>().text = "E";
            }
        }

        if(showPreview)
        {
            ShowArmorPreview(newArmorData);
        }
    }

    public void ShowArmorPreview(ArmorEquipment newArmorData)
    {
        armorPreview.SetActive(true);

        previewArmorName.text = newArmorData.ArmorData.name;
        previewArmorDefence.text = (1 + newArmorData.ArmorData.DefenceValue).ToString();

        if (currentArmorValue > 1 + newArmorData.ArmorData.DefenceValue)
        {
            previewArmorDefence.color = Color.red;
        }
        else if (currentArmorValue < 1 + newArmorData.ArmorData.DefenceValue)
        {
            previewArmorDefence.color = Color.green;
        }
        else
        {
            previewArmorDefence.color = Color.white;
        }
    }
    public void HideArmorPreview()
    {
        armorPreview.SetActive(false);
    }
    #endregion Armor

    public void RefreshUI()
    {
        int currentWeapons = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            if(listOfWeapons[i].WeaponTypeData != null)
            {
                weapons[i].GetComponent<WeaponEquipment>().WeaponData = listOfWeapons[i].WeaponTypeData;
                weapons[i].GetComponent<Button>().enabled = true;
                weapons[i].GetComponent<Image>().enabled = true;
                weapons[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                weapons[i].transform.GetChild(0).GetComponent<Image>().sprite = listOfWeapons[i].WeaponTypeData.ImageSprite;
                currentWeapons += 1;
            }
            else
            {
                weapons[i].GetComponent<WeaponEquipment>().WeaponData = null;
                weapons[i].GetComponent<Button>().enabled = false;
                weapons[i].GetComponent<Image>().enabled = false;
                weapons[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                weapons[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        weaponLimit.text = (currentWeapons + " / " + weapons.Length);


        int currentArmors = 0;
        for (int i = 0; i < armors.Length; i++)
        {
            if (listOfArmors[i].ArmorData != null)
            {
                armors[i].GetComponent<ArmorEquipment>().ArmorData = listOfArmors[i].ArmorData;
                armors[i].GetComponent<Button>().enabled = true;
                armors[i].GetComponent<Image>().enabled = true;
                armors[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                armors[i].transform.GetChild(0).GetComponent<Image>().sprite = listOfArmors[i].ArmorData.ImageSprite;
                currentArmors += 1;
            }
            else
            {
                armors[i].GetComponent<ArmorEquipment>().ArmorData = null;
                armors[i].GetComponent<Button>().enabled = false;
                armors[i].GetComponent<Image>().enabled = false;
                armors[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                armors[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        armorLimit.text = (currentArmors + " / " + armors.Length);
    }
    public void testrefresh()
    {
        RefreshUI();
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

}
