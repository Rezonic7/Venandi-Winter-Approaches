using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : Singleton<ShopManager>
{
    [Header("Shop Item Preview Variables")]
    public Text _itemName;
    public Image _itemImage;
    public Text _itemType;
    public Text _itemDescription;
    public Text _itemPrice;

    [Header("Shop Item")]
    public GameObject _shopItemHolder;
    private GameObject[] _shopItems;
    private ItemInventoryClass[] _listOfShopItems;

    [Header("Amount To Buy Variables")]
    public GameObject _itemToBuyPanel;

    public Text _itemToBuyName;
    public Image _itemToBuyImage;
    public Text _itemToBuyType;

    public Text _itemToBuyAmountText;
    public Text _itemToBuyTotalPrice;

    private int _itemToBuyAmountInt = 1;


    /// <Buying>
    private ItemClass _itemToBuy = null;
    public ItemClass ItemToBuy { get { return _itemToBuy; } set { _itemToBuy = value; } } 

    // Start is called before the first frame update
    void Start()
    {
        _shopItems = new GameObject[_shopItemHolder.transform.childCount];
        _listOfShopItems = new ItemInventoryClass[_shopItems.Length];

        for (int i = 0; i < _shopItems.Length; i++)
        {
            _listOfShopItems[i] = new ItemInventoryClass();
        }
        for (int i = 0; i < _shopItemHolder.transform.childCount; i++)
        {
            _shopItems[i] = _shopItemHolder.transform.GetChild(i).gameObject;
        }

        PopulateShopItems();
        RefreshUI();
    }
    public string GetItemType(ItemClass newItem)
    {
        string textType = "";

        switch (newItem.GetType().ToString())
        {
            case "MiscClass":
                textType = "Misc Item";
                break;
            case "ConsumableClass":
                textType = "Consumable Item";
                break;
            case "AmmoClass":
                textType = "Ammo";
                break;
        }

        return textType;
    }
    public void PopulateShopItems()
    {
        for(int i = 0; i < PlayerProgress.instance._listOfCurrentShopItems.Count; i++)
        {
            _listOfShopItems[i].ItemData = PlayerProgress.instance._listOfCurrentShopItems[i];
            _listOfShopItems[i].ItemID = PlayerProgress.instance.itemDatabase.GetItemID[_listOfShopItems[i].ItemData];
        }
    }
    public void RefreshUI()
    {
        for (int i = 0; i < _shopItems.Length; i++)
        {
            if (_listOfShopItems[i].ItemData != null)
            {
                _shopItems[i].SetActive(true);

                _shopItems[i].GetComponent<ShopItem>().ItemData = _listOfShopItems[i].ItemData;
                _shopItems[i].transform.GetChild(0).GetComponent<Text>().text = _listOfShopItems[i].ItemData.ItemName;
                _shopItems[i].transform.GetChild(3).GetComponent<Image>().enabled = true;
                _shopItems[i].transform.GetChild(3).GetComponent<Image>().sprite = _listOfShopItems[i].ItemData.ImageSprite;
            }
            else
            {
                _shopItems[i].SetActive(false);

                //_shopItems[i].GetComponent<ShopItem>().ItemData = null;
                _shopItems[i].transform.GetChild(0).GetComponent<Text>().text = "";
                _shopItems[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
                _shopItems[i].transform.GetChild(3).GetComponent<Image>().sprite = null;
            }
        }
    }
    public void ShowShopItemPreview(ItemClass newItem)
    {
        _itemName.text = newItem.ItemName;
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = newItem.ImageSprite;

        _itemType.text = GetItemType(newItem);
        _itemDescription.text = newItem.ItemDescription;
        _itemPrice.text = ("x" + newItem.ItemBuyPrice);
    }
    public void HideShopItemPreview()
    {
        _itemName.text = "Item Name";
        _itemImage.gameObject.SetActive(false);
        _itemType.text = "";
        _itemDescription.text = "";
        _itemPrice.text = "";
    }
    public void ShowItemToBuyPanel()
    {
        ItemInventoryClass tempItemToBuy = new ItemInventoryClass();
        
        _itemToBuyPanel.SetActive(true);

        _itemToBuyName.text = _itemToBuy.ItemName;
        _itemToBuyImage.sprite = _itemToBuy.ImageSprite;
       
        
        _itemToBuyType.text = GetItemType(_itemToBuy);

        _itemToBuyAmountText.text = (_itemToBuyAmountInt * _itemToBuy.AmountPerBuyIteration).ToString();
        _itemToBuyTotalPrice.text = (_itemToBuy.ItemBuyPrice * _itemToBuyAmountInt).ToString();

    }
    public void HideItemToBuyPanel()
    {
        _itemToBuyPanel.SetActive(false);
        _itemToBuyAmountInt = 1;
    }

    public void AddAmount()
    {
        if(_itemToBuyAmountInt + 1 <= _itemToBuy.MaxStack)
        {
            if ((_itemToBuy.ItemBuyPrice * (_itemToBuyAmountInt + 1)) <= PlayerProgress.instance.PlayerCurrentMEAT)
            {
                _itemToBuyAmountInt += 1;
                _itemToBuyAmountText.text = (_itemToBuyAmountInt * _itemToBuy.AmountPerBuyIteration).ToString();
                _itemToBuyTotalPrice.text = ("x" + _itemToBuy.ItemBuyPrice * _itemToBuyAmountInt);
            }
            else
            {
                CanvasManager.instance.ShowInfo("You cannot buy anymore " + _itemToBuy.ItemName);
            }
        }
        else
        {
            CanvasManager.instance.ShowInfo(_itemToBuy.ItemName + " is at Maximum Stacks.");
        }
    }
    public void SubdractAmount()
    {
        if(_itemToBuyAmountInt - 1 > 0)
        {
            _itemToBuyAmountInt -= 1;
            _itemToBuyAmountText.text = _itemToBuyAmountInt.ToString();
            _itemToBuyTotalPrice.text = ("x" + _itemToBuy.ItemBuyPrice * _itemToBuyAmountInt);

        }
    }
    public void BuyItem()
    {
        PlayerProgress.instance.UpdateMEATValue(-(_itemToBuy.ItemBuyPrice * _itemToBuyAmountInt));
        if (ShopInventoryToggle.instance._isInChest)
        {
            InventoryManager.instance.AddItemToChest(_itemToBuy, _itemToBuyAmountInt * _itemToBuy.AmountPerBuyIteration);
        }
        else
        {
            InventoryManager.instance.AddItemToInventory(_itemToBuy, _itemToBuyAmountInt * _itemToBuy.AmountPerBuyIteration);
        }
    }
}
