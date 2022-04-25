using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpots : MonoBehaviour
{
    [SerializeField] private GatheringTypeData gatheringData;
    public int amount;

    public int gatheringTimes;

    private void Start()
    {
        gatheringTimes = RandomTimes();
    }
    
    float total;
    public string GatherItem()
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
                total = 0;
                gatheringTimes -= 1;
                if(gatheringTimes <= 0)
                {
                    Player_Controller.instance.canGather = false;
                    Player_Controller.instance.gatheringItem = null;
                    GatheringSpotManager.instance.Respawn(gameObject.GetComponent<GatheringSpots>(), RandomTimes());
                    gameObject.SetActive(false);
                }
                amount = Random.Range(item.MinAmount, item.MaxAmount + 1);
                return item.Item.Name;
            }
            else
            {
                random -= item.InitialGatherPercentage;
            }
        }
        total = 0;
        return "You got nothing";
    }

    private int RandomTimes()
    {
        int randomTimes = Random.Range(gatheringData.MinTimesToGather, gatheringData.MaxTimesToGather + 1);
        return randomTimes;
    }

   
}
