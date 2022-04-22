using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GatheredMaterialData", menuName = "Create Data/Create Gathering Data/Gathered Materail Data")]
public class GatherableMaterialData : ScriptableObject
{
    [SerializeField] private ItemData _item;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;
    [SerializeField] private float _initialGatherPercentage;

    public ItemData Item { get { return _item; } set { _item = value; } }
    public int MinAmount { get { return _minAmount; } set { _minAmount = value; } }
    public int MaxAmount { get { return _maxAmount; } set { _maxAmount = value; } }
    public float InitialGatherPercentage { get { return _initialGatherPercentage; } set { _initialGatherPercentage = value; } }

}
