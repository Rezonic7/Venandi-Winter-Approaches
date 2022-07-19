using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInventoryClass 
{
    public int ItemID;
    public ItemClass ItemData;
    public int Amount;

    public ItemInventoryClass()
    {
        ItemID = 0;
        ItemData = null;
        Amount = 0;
    }
    public ItemInventoryClass(int _id)
    {
        ItemID = _id;
    }
    public ItemInventoryClass (ItemClass _itemData, int _amount)
    {
        ItemData = _itemData;
        Amount = _amount;
    }

    public ItemInventoryClass(int _id, ItemClass _itemData, int _amount)
    {
        ItemID = _id;
        ItemData = _itemData;
        Amount = _amount;
    }

    public void Clear()
    {
        ItemID = 0;
        ItemData = null;
        Amount = 0;
    }
}
