using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : Singleton<PlayerProgress>, ISerializationCallbackReceiver
{
    #region Equipment Variables
    [Header("Equipment Variables")]
    public WeaponDatabase weaponDatabase;
    public ArmorDatabase armorDatabase;

    [SerializeField] private List<WeaponInventoryClass> _listOfPlayerWeapons = new List<WeaponInventoryClass>();
    [SerializeField] private List<ArmorInventoryClass> _listOfPlayerArmors = new List<ArmorInventoryClass>();

    [Header("Equipment IDs")]

    public List<int> WeaponIDs;
    public List<int> ArmorIDs;
    public List<WeaponInventoryClass> ListOfPlayerWeapons { get { return _listOfPlayerWeapons; } set { _listOfPlayerWeapons = value; } }
    public List<ArmorInventoryClass> ListOfPlayerArmors { get { return _listOfPlayerArmors; } set { _listOfPlayerArmors = value; } }

    #endregion Equipment Variables

    #region Item Variables
    [Header("Item Variables")]

    public ItemDatabase itemDatabase;

    [SerializeField] private List<ItemInventoryClass> _listOfPlayerInventoryItems = new List<ItemInventoryClass>();
    [SerializeField] private List<ItemInventoryClass> _listOfPlayerChestItems = new List<ItemInventoryClass>();
    
    [Header("Item IDs")]

    public List<int> InventoryIDs;
    public List<int> ChestIDs;

    [Header("Item Amounts")]
    public List<int> InventoryAmount;
    public List<int> ChestAmount;

    public List<ItemInventoryClass> ListOfPlayerInventoryItems { get { return _listOfPlayerInventoryItems; } set { _listOfPlayerInventoryItems = value; } }
    public List<ItemInventoryClass> ListOfPlayerChestItems { get { return _listOfPlayerChestItems; } set { _listOfPlayerChestItems = value; } }

    #endregion Item Variables

    private Text _playerCurrentMEATtext;
    private int _playerCurrentMEAT = 10;
    public int PlayerCurrentMEAT { get { return _playerCurrentMEAT; } }


    [Header("Shop Variables")]
    private int _playerShopUpgrade = 0;

    public List<ItemClass> _listOfCurrentShopItems;

    public List<ItemClass> _ShopItems0;
    public List<ItemClass> _ShopItems1;
    public List<ItemClass> _ShopItems2;
    public List<ItemClass> _ShopItems3;

    public void OnAfterDeserialize()
    {
        #region Equipment
        for (int i = 0; i < _listOfPlayerWeapons.Count; i++)
        {
            _listOfPlayerWeapons[i].WeaponTypeData = weaponDatabase.GetWeaponData[_listOfPlayerWeapons[i].ID];
        }
        for (int i = 0; i < _listOfPlayerArmors.Count; i++)
        {
            _listOfPlayerArmors[i].ArmorData = armorDatabase.GetArmorData[_listOfPlayerArmors[i].ID];
        }
        #endregion Equipment

        #region Inventory
        for (int i = 0; i < _listOfPlayerInventoryItems.Count; i++)
        {
            _listOfPlayerInventoryItems[i].ItemData = itemDatabase.GetItemData[_listOfPlayerInventoryItems[i].ItemID];
        }
        
        for (int i = 0; i < _listOfPlayerChestItems.Count; i++)
        {
            _listOfPlayerChestItems[i].ItemData = itemDatabase.GetItemData[_listOfPlayerChestItems[i].ItemID];
        }
        
        #endregion Inventory

        SavePlayerProgress();
    }

    public void OnBeforeSerialize()
    {

    }

    private void Start()
    {
        if(GameObject.FindWithTag("PlayerCurrentMEAT")?.GetComponent<Text>())
        {
            _playerCurrentMEATtext = GameObject.FindWithTag("PlayerCurrentMEAT")?.GetComponent<Text>();
            _playerCurrentMEATtext.text =  ("x" + _playerCurrentMEAT);
        }
        UpdateShopItems();

        OnAfterDeserialize();
    }
    public void SavePlayerProgress()
    {
        WeaponIDs = new List<int>();
        for (int i = 0; i < _listOfPlayerWeapons.Count; i++)
        {
            WeaponIDs.Add(weaponDatabase.GetWeaponID[_listOfPlayerWeapons[i].WeaponTypeData]);
        }

        ArmorIDs = new List<int>();
        for(int i = 0; i < _listOfPlayerArmors.Count; i++)
        {
            ArmorIDs.Add(armorDatabase.GetArmorID[_listOfPlayerArmors[i].ArmorData]);

        }

        InventoryIDs = new List<int>();
        for (int i = 0; i < _listOfPlayerInventoryItems.Count; i++)
        {
            InventoryIDs.Add(itemDatabase.GetItemID[_listOfPlayerInventoryItems[i].ItemData]);
        }
        InventoryAmount = new List<int>();
        for (int i = 0; i < _listOfPlayerInventoryItems.Count; i++)
        {
            InventoryAmount.Add(_listOfPlayerInventoryItems[i].Amount);
        }

        ChestIDs = new List<int>();
        for (int i = 0; i < _listOfPlayerChestItems.Count; i++)
        {
            ChestIDs.Add(itemDatabase.GetItemID[_listOfPlayerChestItems[i].ItemData]);
        }

        ChestAmount = new List<int>();
        for (int i = 0; i < _listOfPlayerChestItems.Count; i++)
        {
            ChestAmount.Add(_listOfPlayerChestItems[i].Amount);
        }


    }
    public void LoadPlayerProgress()
    {
        _listOfPlayerWeapons = new List<WeaponInventoryClass>();
        _listOfPlayerArmors = new List<ArmorInventoryClass>();
        for (int i = 0; i < WeaponIDs.Count; i++)
        {
            _listOfPlayerWeapons.Add(new WeaponInventoryClass(WeaponIDs[i]));
        }
        for (int i = 0; i < ArmorIDs.Count; i++)
        {
            _listOfPlayerArmors.Add(new ArmorInventoryClass(ArmorIDs[i]));
        }


        _listOfPlayerInventoryItems = new List<ItemInventoryClass>();
        _listOfPlayerChestItems = new List<ItemInventoryClass>();
        for (int i = 0; i < InventoryIDs.Count; i++)
        {
            _listOfPlayerInventoryItems.Add(new ItemInventoryClass(InventoryIDs[i]));
            _listOfPlayerInventoryItems[i].Amount = InventoryAmount[i];
        }
        for (int i = 0; i < ChestIDs.Count; i++)
        {
            _listOfPlayerChestItems.Add(new ItemInventoryClass(ChestIDs[i]));
            _listOfPlayerChestItems[i].Amount = ChestAmount[i];
        }

        OnAfterDeserialize();

        EquipmentManager.instance.UpdateEquipment();
        InventoryManager.instance.PopulateItems();
    }

    public void UpdateShopItems()
    {
        switch (_playerShopUpgrade)
        {
            case 0:
                _listOfCurrentShopItems = _ShopItems0;
                break;
            case 1:
                _listOfCurrentShopItems = _ShopItems1;
                break;
            case 2:
                _listOfCurrentShopItems = _ShopItems2;
                break;
            case 3:
                _listOfCurrentShopItems = _ShopItems3;
                break;
        }
    }
    public void UpdateMEATValue(int newValue)
    {
        _playerCurrentMEAT += newValue;

        _playerCurrentMEATtext.text = ("x" + _playerCurrentMEAT);
    }
}
