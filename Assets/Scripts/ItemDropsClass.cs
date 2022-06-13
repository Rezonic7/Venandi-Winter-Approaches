using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDropsClass
{
    [SerializeField] private ItemClass _item;
    [SerializeField] private int _minQuantityAmount;
    [SerializeField] private int _maxQuantityAmount;
    [SerializeField] private float _gatherPercentage;

    public int MinQuantityAmount { get { return _minQuantityAmount; } }
    public int MaxQuantityAmount { get { return _maxQuantityAmount; } }
    public float GatherPercentage { get { return _gatherPercentage; } }
    public ItemClass Item { get { return _item; } }
}
