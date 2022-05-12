using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDropsClass
{
    [SerializeField] private ItemClass _item;
    [SerializeField] private int _quantity;
    public ItemClass Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } set { _quantity = value; } }
}
