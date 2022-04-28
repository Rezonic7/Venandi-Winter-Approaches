using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpots : MonoBehaviour
{
    [SerializeField] private GatheringTypeData gatheringData;

    private int _gatheringTimes;
    public int GatheringTimes { set { _gatheringTimes = value; } }

    private void Start()
    {
        _gatheringTimes = RandomTimes();
    }
    
    float total;
   
    public GatherableMaterialData GatherItem()
    {
        foreach(var item in gatheringData.ListOfGatherableMaterials)
        {
            total += item.InitialGatherPercentage;
        }
        float random = Random.Range(0, total);
        foreach (var item in gatheringData.ListOfGatherableMaterials)
        {
            if(random <= item.InitialGatherPercentage)
            {
                _gatheringTimes -= 1;
                total = 0;
                if(_gatheringTimes <= 0)
                {
                    Player_Controller.instance.canGather = false;
                    Player_Controller.instance.gatheringItem = null;
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
