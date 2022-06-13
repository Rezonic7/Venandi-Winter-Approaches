using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpots : MonoBehaviour
{
    [SerializeField] private GatheringSpotType gatheringData;

    private int _gatheringTimes;

    private bool _isGatherable = true;
    public bool IsGatherable { get { return _isGatherable; } set { _isGatherable = value; } }
    public int GatheringTimes { set { _gatheringTimes = value; } }

    private void Start()
    {
        _isGatherable = true;
        _gatheringTimes = RandomTimes();
    }
    
    float total;
   
    public GatherableItems GatherItem()
    {
        foreach(var item in gatheringData.ListOfGatherableItems)
        {
            total += item.InitialGatherPercentage;
        }
        float random = Random.Range(0, total);
        foreach (var item in gatheringData.ListOfGatherableItems)
        {
            if(random <= item.InitialGatherPercentage)
            {
                _gatheringTimes -= 1;
                total = 0;
                if(_gatheringTimes <= 0)
                {
                    Player_Controller.instance.CanGather = false;
                    Player_Controller.instance.GatheringItem = null;
                    _isGatherable = false;
                    GatheringSpotManager.instance.Respawn(gameObject.GetComponent<GatheringSpots>(), RandomTimes());
                    gameObject.SetActive(false);
                }
                return item;
            }
            else
            {
                random -= item.InitialGatherPercentage;
            }
        }
        total = 0;
        return null;
    }

    private int RandomTimes()
    {
        int randomTimes = Random.Range(gatheringData.MinTimesToGather, gatheringData.MaxTimesToGather + 1);
        return randomTimes;
    }

   
}
