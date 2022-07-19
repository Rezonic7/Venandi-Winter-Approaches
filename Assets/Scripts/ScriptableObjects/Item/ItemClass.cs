using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [SerializeField] private string _ItemName;
    [SerializeField] private Sprite _imageSprite;
    [SerializeField] private int _maxStack;
    [TextArea(5, 15)]
    [SerializeField] private string _itemDescription;
    [Header("Buy Properties")]
    [SerializeField] private int _itemBuyPrice;
    [SerializeField] private int _amountPerBuyIteration;

    public string ItemName { get { return _ItemName; } set { _ItemName = value; } }
    public Sprite ImageSprite { get { return _imageSprite; } set { _imageSprite = value; } }
    public int MaxStack { get { return _maxStack; } set { _maxStack = value; } }
    public string ItemDescription { get { return _itemDescription; } set { _itemDescription = value; } }
    public int ItemBuyPrice { get { return _itemBuyPrice; } set { _itemBuyPrice = value; } }
    public int AmountPerBuyIteration { get { return _amountPerBuyIteration; } set { _amountPerBuyIteration = value; } }

    public abstract ItemClass GetItem();
    public abstract MiscClass GetMisc();
    public abstract ConsumableClass GetConsumable();

    public abstract AmmoClass GetAmmo();
}
