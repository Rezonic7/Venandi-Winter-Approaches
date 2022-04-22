using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Create Data/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _imageSprite;
    [SerializeField] private int _maxStack;

    public string Name { get { return _name; } set { _name = value; } }
    public Sprite ImageSprite { get { return _imageSprite; } set { _imageSprite = value; } }
    public int MaxStack { get { return _maxStack; } set { _maxStack = value; } }

}
