using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass _item;
    [SerializeField] private int _quantity;

    public SlotClass()
    {
        _item = null;
        _quantity = 0;
    }

    public SlotClass (ItemClass newItem, int newQuantity)
    {
        _item = newItem;
        _quantity = newQuantity;
    }

    public void Clear()
    {
        _item = null;
        _quantity = 0;
    }

    public ItemClass Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } set { _quantity = value; } }

}
