using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemClass _itemData;

    public ItemClass ItemData { get { return _itemData; } set { _itemData = value; } }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_itemData)
        {
            ShopManager.instance.ShowShopItemPreview(this.ItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_itemData)
        {
            ShopManager.instance.HideShopItemPreview();
        }
    }

    public void BuyItem()
    {
        //IF has enough MEAT
        if(PlayerProgress.instance.PlayerCurrentMEAT >= _itemData.ItemBuyPrice)
        {
            ShopManager.instance.ItemToBuy = _itemData;
            ShopManager.instance.ShowItemToBuyPanel();
        }
        else
        {
            CanvasManager.instance.ShowInfo("You don't have enough MEAT to buy this item");
        }
    }
}
