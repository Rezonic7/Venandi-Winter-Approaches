using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GatheringSpotData", menuName = "Create Data/Create Gathering Data/Gathering Spots") ]
public class GatheringTypeData : ScriptableObject
{
    [SerializeField] private GatheringTypes _gatheringType;
    [SerializeField] private List<GatherableMaterialData> _listOfGatherableMaterials;
    [SerializeField] private int _minTimesToGather;
    [SerializeField] private int _maxTimesToGather;


    public GatheringTypes GatheringType { get { return _gatheringType; } set { _gatheringType = value; } }
    public List<GatherableMaterialData> ListOfGatherableMaterials { get { return _listOfGatherableMaterials; } set { _listOfGatherableMaterials = value; } }

    public int MinTimesToGather { get { return _minTimesToGather; } set { _minTimesToGather = value; } }
    public int MaxTimesToGather { get { return _maxTimesToGather; } set { _maxTimesToGather = value; } }


    public enum GatheringTypes
    {
        MiningSpot,
        ForagingSpot,
        HerbsandPlants
    }

}
