using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [SerializeField] private string _ItemName;
    [SerializeField] private Sprite _imageSprite;
    [SerializeField] private int _maxStack;

    public string ItemName { get { return _ItemName; } set { _ItemName = value; } }
    public Sprite ImageSprite { get { return _imageSprite; } set { _imageSprite = value; } }
    public int MaxStack { get { return _maxStack; } set { _maxStack = value; } }

    public abstract ItemClass GetItem();
    public abstract MiscClass GetMisc();
    public abstract ConsumableClass GetConsumable();
}
