using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class InventoryManager : Singleton<InventoryManager>
{
    public bool _isInInventoryChest = false;

    public Image currentlyHoldingItemImage;

    [Header("Inventory Variables")]
    //debug put to private after finalization: _listOfInventoryItems
    public ItemInventoryClass[] _listOfInventoryItems;
    public GameObject _inventoryItemsHolder;

    private GameObject[] _inventoryItems;

    public Text inventoryLimitText;


    [Header("Chest Variables")]
    //debug put to private after finalization: _listOfChestItems
    public ItemInventoryClass[] _listOfChestItems;
    public GameObject _chestItemsHolder;

    private GameObject[] _chestItems;

    public Text chestLimitText;

    [Header("Hovered Item Variables")]
    public Text _hoveredItemName;
    public Text _itemDescription;
    public GameObject _itemIconGO;
    private Image _itemIconSprite;
    public Text _itemType;


    private ItemInventoryClass _movingInventoryItem;
    private ItemInventoryClass _originalInventoryItem;
    private ItemInventoryClass _tempSlotInventoryItem;
    private ItemInventoryClass _backupInventoryItem;
    private bool _isMovingItem;

    [Header("ShopVariables")]
    public GameObject _shopInventoryItemsHolder;
    public GameObject _shopChestItemsHolder;

    private GameObject[] _shopInventoryItems;
    private GameObject[] _shopChestItems;

    public Text shopInventoryLimitText;
    public Text shopChestLimitText;





    // Start is called before the first frame update
    void Start()
    {
        _itemIconSprite = _itemIconGO.transform.GetChild(1).GetComponent<Image>();

        #region InventoryItems
        _inventoryItems = new GameObject[_inventoryItemsHolder.transform.childCount];
        _shopInventoryItems = new GameObject[_shopInventoryItemsHolder.transform.childCount];
        _listOfInventoryItems = new ItemInventoryClass[_inventoryItems.Length];

        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            _listOfInventoryItems[i] = new ItemInventoryClass();
        }
        for (int i = 0; i < _inventoryItemsHolder.transform.childCount; i++)
        {
            _inventoryItems[i] = _inventoryItemsHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < _shopInventoryItemsHolder.transform.childCount; i++)
        {
            _shopInventoryItems[i] = _shopInventoryItemsHolder.transform.GetChild(i).gameObject;
        }

        #endregion InventoryItems
        #region ChestItems

        _chestItems = new GameObject[_chestItemsHolder.transform.childCount];
        _shopChestItems = new GameObject[_chestItemsHolder.transform.childCount];
        _listOfChestItems = new ItemInventoryClass[_chestItems.Length];

        for (int i = 0; i < _chestItems.Length; i++)
        {
            _listOfChestItems[i] = new ItemInventoryClass();
        }
        for (int i = 0; i < _chestItemsHolder.transform.childCount; i++)
        {
            _chestItems[i] = _chestItemsHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < _shopChestItemsHolder.transform.childCount; i++)
        {
            _shopChestItems[i] = _shopChestItemsHolder.transform.GetChild(i).gameObject;
        }
        #endregion ChestItems

        PopulateItems();
    }
    public void IsInInventoryChest(int amount)
    {
        if(amount >= 0)
        {
            _isInInventoryChest = true;
        }
        else
        {
            _isInInventoryChest = false;

        }
    }
    public void PopulateItems()
    {
        for (int i = 0; i < PlayerProgress.instance.ListOfPlayerInventoryItems.Count; i++)
        {
            _listOfInventoryItems[i] = PlayerProgress.instance.ListOfPlayerInventoryItems[i];
        }
        for (int i = 0; i < PlayerProgress.instance.ListOfPlayerChestItems.Count; i++)
        {
            _listOfChestItems[i] = PlayerProgress.instance.ListOfPlayerChestItems[i];
        }
        RefreshUI();
    }
    public void SaveItems()
    {
        PlayerProgress.instance.ListOfPlayerInventoryItems = new List<ItemInventoryClass>();
        for (int i = 0; i < _listOfInventoryItems.Length; i++)
        {
            if(_listOfInventoryItems[i].ItemData != null)
            {
                PlayerProgress.instance.ListOfPlayerInventoryItems.Add(_listOfInventoryItems[i]);
            }
        }

        PlayerProgress.instance.ListOfPlayerChestItems = new List<ItemInventoryClass>();
        for (int i = 0; i < _listOfChestItems.Length; i++)
        {
            if(_listOfChestItems[i]. ItemData != null)
            {
                PlayerProgress.instance.ListOfPlayerChestItems.Add(_listOfChestItems[i]);
            }
        }
        PlayerProgress.instance.SavePlayerProgress();
    }
    public void ShowItemPreview(ItemInventory newItemData)
    {
        _hoveredItemName.text = newItemData.ItemData.ItemName;
        _itemDescription.text = newItemData.ItemData.ItemDescription;
        _itemIconGO.SetActive(true);
        _itemIconSprite.sprite = newItemData.ItemData.ImageSprite;

        string textType = "";

        switch (newItemData.ItemData.GetType().ToString())
        {
            case "MiscClass":
                textType = "Misc Item";
                break;
            case "ConsumableClass":
                textType = "Consumable Item";
                break;
        }

        _itemType.text = textType;
    }
    public void HideItemPreview()
    {
        _hoveredItemName.text = "Item";
        _itemDescription.text = "";
        _itemIconSprite.sprite = null;
        _itemIconGO.SetActive(false);
        _itemType.text = "";
    }
    public ItemInventoryClass Contains(ItemClass item, ItemInventoryClass[] listOfItems)
    {
        foreach (ItemInventoryClass inventoryItem in listOfItems)
        {
            if (inventoryItem.ItemData == item)
            {
                return inventoryItem;
            }
        }
        return null;
    }
    private void Update()
    {
        if(_isMovingItem)
        {
            currentlyHoldingItemImage.transform.position = Mouse.current.position.ReadValue();
        }
    }
    public void AddItemToInventory(ItemClass newItem, int amount)
    {
        ItemInventoryClass item = Contains(newItem, _listOfInventoryItems);
        if (item != null)
        {
            if (item.Amount + amount <= newItem.MaxStack)
            {
                item.Amount += amount;
            }
            else
            {
                CanvasManager.instance.ShowInfo("Inventory cannot carry anymore " + item.ItemData.ItemName);
                int leftOverAmount = (item.Amount + amount) - item.ItemData.MaxStack;
                for (int i = 0; i < _listOfChestItems.Length; i++)
                {
                    if (_listOfChestItems[i].ItemData == null)
                    {
                        _listOfChestItems[i] = new ItemInventoryClass(newItem, leftOverAmount);
                        break;
                    }
                }
                item.Amount = newItem.MaxStack;

            }
        }
        else
        {
            for (int i = 0; i < _listOfInventoryItems.Length; i++)
            {
                if (_listOfInventoryItems[i].ItemData == null)
                {
                    _listOfInventoryItems[i] = new ItemInventoryClass(newItem, amount);
                    break;
                }
            }
        }
        SaveItems();
        RefreshUI();
    }
    public void AddItemToChest(ItemClass newItem, int amount)
    {
        ItemInventoryClass item = Contains(newItem, _listOfChestItems);
        if (item != null)
        {
            if (item.Amount + amount <= newItem.MaxStack)
            {
                item.Amount += amount;
            }
            else
            {

                int leftOverAmount = (item.Amount + amount) - item.ItemData.MaxStack;

                for (int i = 0; i < _listOfChestItems.Length; i++)
                {
                    if (_listOfChestItems[i].ItemData == null)
                    {
                        _listOfChestItems[i] = new ItemInventoryClass(newItem, leftOverAmount);
                        break;
                    }
                }
                item.Amount = newItem.MaxStack;
            }
        }
        else
        {
            for (int i = 0; i < _listOfChestItems.Length; i++)
            {
                if (_listOfChestItems[i].ItemData == null)
                {
                    _listOfChestItems[i] = new ItemInventoryClass(newItem, amount);
                    break;
                }
            }
        }
        SaveItems();
        RefreshUI();
    }
    private void RefreshUI()
    {
        #region Inventory
        int currentItemsInInventory = 0;
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            if(_listOfInventoryItems[i].ItemData != null)
            {
                _inventoryItems[i].GetComponent<ItemInventory>().ItemData = _listOfInventoryItems[i].ItemData;
                _inventoryItems[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                _inventoryItems[i].transform.GetChild(1).GetComponent<Image>().sprite = _listOfInventoryItems[i].ItemData.ImageSprite;
                _inventoryItems[i].transform.GetChild(2).GetComponent<Text>().text = _listOfInventoryItems[i].Amount.ToString();

                _shopInventoryItems[i].GetComponent<ItemInventory>().ItemData = _listOfInventoryItems[i].ItemData;
                _shopInventoryItems[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                _shopInventoryItems[i].transform.GetChild(1).GetComponent<Image>().sprite = _listOfInventoryItems[i].ItemData.ImageSprite;
                _shopInventoryItems[i].transform.GetChild(2).GetComponent<Text>().text = _listOfInventoryItems[i].Amount.ToString();


                currentItemsInInventory += 1;
            }
            else
            {
                _inventoryItems[i].GetComponent<ItemInventory>().ItemData = null;
                _inventoryItems[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                _inventoryItems[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                _inventoryItems[i].transform.GetChild(2).GetComponent<Text>().text = "";

                _shopInventoryItems[i].GetComponent<ItemInventory>().ItemData = null;
                _shopInventoryItems[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                _shopInventoryItems[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                _shopInventoryItems[i].transform.GetChild(2).GetComponent<Text>().text = "";
            }
            inventoryLimitText.text = (currentItemsInInventory + " / " + _inventoryItems.Length);
            shopInventoryLimitText.text = inventoryLimitText.text;
        }
        #endregion Inventory

        #region Chests
        int currentItemsInChest = 0;
        for (int i = 0; i < _chestItems.Length; i++)
        {
            if (_listOfChestItems[i].ItemData != null)
            {
                _chestItems[i].GetComponent<ItemInventory>().ItemData = _listOfChestItems[i].ItemData;
                _chestItems[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                _chestItems[i].transform.GetChild(1).GetComponent<Image>().sprite = _listOfChestItems[i].ItemData.ImageSprite;
                _chestItems[i].transform.GetChild(2).GetComponent<Text>().text = _listOfChestItems[i].Amount.ToString();

                _shopChestItems[i].GetComponent<ItemInventory>().ItemData = _listOfChestItems[i].ItemData;
                _shopChestItems[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                _shopChestItems[i].transform.GetChild(1).GetComponent<Image>().sprite = _listOfChestItems[i].ItemData.ImageSprite;
                _shopChestItems[i].transform.GetChild(2).GetComponent<Text>().text = _listOfChestItems[i].Amount.ToString();

                currentItemsInChest += 1;
            }
            else
            {
                _chestItems[i].GetComponent<ItemInventory>().ItemData = null;
                _chestItems[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                _chestItems[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                _chestItems[i].transform.GetChild(2).GetComponent<Text>().text = "";

                _shopChestItems[i].GetComponent<ItemInventory>().ItemData = null;
                _shopChestItems[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                _shopChestItems[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                _shopChestItems[i].transform.GetChild(2).GetComponent<Text>().text = "";
            }
            chestLimitText.text = (currentItemsInChest + " / " + _chestItems.Length);
            shopChestLimitText.text = chestLimitText.text;
        }
        #endregion Chests
    }
    public void OnLeftClick(InputAction.CallbackContext value)
    {
        if(!_isInInventoryChest)
        {
            return;
        }
        if (!value.started)
        {
            return;
        }
        if (_isMovingItem)
        {
            EndMoveItem();
            return;
        }
        StartMoveItem();
    }
    private void StartMoveItem()
    {
        _originalInventoryItem = GetInventoryClosestSlot();
        if(_originalInventoryItem == null)
        {
            _originalInventoryItem = GetChestClosestSlot();
        }

        _backupInventoryItem = _originalInventoryItem;
        if (_originalInventoryItem == null || _originalInventoryItem.ItemData == null)
        {
            return;
        }
        _movingInventoryItem = new ItemInventoryClass(_originalInventoryItem.ItemData, _originalInventoryItem.Amount);
        _originalInventoryItem.Clear();
        _isMovingItem = true;

        currentlyHoldingItemImage.gameObject.SetActive(true);
        currentlyHoldingItemImage.sprite = _movingInventoryItem.ItemData.ImageSprite;

        RefreshUI();
    }
    private void EndMoveItem()
    {
        _originalInventoryItem = GetInventoryClosestSlot();
        if (_originalInventoryItem == null)
        {
            _originalInventoryItem = GetChestClosestSlot();
            if (_originalInventoryItem == null)
            {
                ReturnToOriginalSlot();
                return;
            }
           
        }
        if (_originalInventoryItem.ItemData != null)
        {
            //if Same Items Stack
            if (_originalInventoryItem.ItemData == _movingInventoryItem.ItemData)
            {
                //if amount is more than MaxStack   
                if (_originalInventoryItem.Amount + _movingInventoryItem.Amount <= _originalInventoryItem.ItemData.MaxStack)
                {
                    _originalInventoryItem.Amount += _movingInventoryItem.Amount;

                    _isMovingItem = false;

                    currentlyHoldingItemImage.gameObject.SetActive(false);
                    currentlyHoldingItemImage.sprite = null;

                    SaveItems();
                    RefreshUI();

                    return;

                }
                else
                {
                    _tempSlotInventoryItem = new ItemInventoryClass(_movingInventoryItem.ItemData, _movingInventoryItem.Amount);
                    int newAmount = (_originalInventoryItem.Amount += _movingInventoryItem.Amount) - _originalInventoryItem.ItemData.MaxStack;

                    _movingInventoryItem = new ItemInventoryClass(_originalInventoryItem.ItemData, newAmount);

                    _originalInventoryItem.Amount = _originalInventoryItem.ItemData.MaxStack;

                }
            }
            else
            {
                _tempSlotInventoryItem = new ItemInventoryClass(_movingInventoryItem.ItemData, _movingInventoryItem.Amount);
                _movingInventoryItem = new ItemInventoryClass(_originalInventoryItem.ItemData, _originalInventoryItem.Amount);
                _originalInventoryItem.ItemData = _tempSlotInventoryItem.ItemData;
                _originalInventoryItem.Amount = _tempSlotInventoryItem.Amount;

                currentlyHoldingItemImage.sprite = _movingInventoryItem.ItemData.ImageSprite;
            }

            _tempSlotInventoryItem.Clear();
            _isMovingItem = true;

            SaveItems();
            RefreshUI();
        }
        else
        {
            _originalInventoryItem.ItemData = _movingInventoryItem.ItemData;
            _originalInventoryItem.Amount = _movingInventoryItem.Amount;
            _movingInventoryItem.Clear();
            _isMovingItem = false;

            currentlyHoldingItemImage.gameObject.SetActive(false);
            currentlyHoldingItemImage.sprite = null;

            SaveItems();
            RefreshUI();
        }

    }
    public void ReturnToOriginalSlot()
    {
        _backupInventoryItem.ItemData = _movingInventoryItem.ItemData;
        _backupInventoryItem.Amount = _movingInventoryItem.Amount;
        _movingInventoryItem.Clear();
        _isMovingItem = false;

        currentlyHoldingItemImage.gameObject.SetActive(false);
        currentlyHoldingItemImage.sprite = null;

        RefreshUI();
    }
    public void OnRightClick(InputAction.CallbackContext value)
    {
        if (!_isInInventoryChest)
        {
            return;
        }
        if (!value.started)
        {
            return;
        }
        if(_isMovingItem)
        {
            return;
        }
        QuickMoveInventory();
        
    }
    private void QuickMoveInventory()
    {
        ItemInventoryClass quickMoveItem;

        quickMoveItem = GetInventoryClosestSlot();
        if (quickMoveItem == null)
        {
            quickMoveItem = GetChestClosestSlot();

            if(quickMoveItem.ItemData == null)
            {
                return;
            }
            AddItemToInventory(quickMoveItem.ItemData, quickMoveItem.Amount);
            quickMoveItem.Clear();

            SaveItems();
            RefreshUI();

            return;
        }

        if (quickMoveItem.ItemData == null)
        {
            return;
        }

        AddItemToChest(quickMoveItem.ItemData, quickMoveItem.Amount);
        quickMoveItem.Clear();

        SaveItems();
        RefreshUI();
    }
    private ItemInventoryClass GetInventoryClosestSlot()
    {
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            if (Vector2.Distance(_inventoryItems[i].transform.position, Mouse.current.position.ReadValue()) <= 70)
            {
                return _listOfInventoryItems[i];
            }
        }
        return null;
    }
    private ItemInventoryClass GetChestClosestSlot()
    {
        for (int i = 0; i < _chestItems.Length; i++)
        {
            if (Vector2.Distance(_chestItems[i].transform.position, Mouse.current.position.ReadValue()) <= 70)
            {
                return _listOfChestItems[i];
            }
        }
        return null;
    }
} 
