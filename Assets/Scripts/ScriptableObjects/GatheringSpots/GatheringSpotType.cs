using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gathering Spot Type", menuName = "Create Data/Create Gathering Data/Gathering Spot Type Data") ]
public class GatheringSpotType : ScriptableObject
{
    [SerializeField] private GatheringTypes _gatheringType;
    [SerializeField] private List<GatherableItems> _listOfGatherableItems;
    [SerializeField] private int _minTimesToGather;
    [SerializeField] private int _maxTimesToGather;


    public GatheringTypes GatheringType { get { return _gatheringType; } set { _gatheringType = value; } }
    public List<GatherableItems> ListOfGatherableItems { get { return _listOfGatherableItems; } set { _listOfGatherableItems = value; } }

    public int MinTimesToGather { get { return _minTimesToGather; } set { _minTimesToGather = value; } }
    public int MaxTimesToGather { get { return _maxTimesToGather; } set { _maxTimesToGather = value; } }


    public enum GatheringTypes
    {
        MiningSpot,
        ForagingSpot,
        HerbsandPlants
    }

}
