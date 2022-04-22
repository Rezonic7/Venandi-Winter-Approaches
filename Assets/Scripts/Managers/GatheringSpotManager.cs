using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpotManager : Singleton<GatheringSpotManager>
{
    public void Respawn(GatheringSpots gs, int newRandomTimes)
    {
        StartCoroutine(RespawnTimer(gs, newRandomTimes));
    }

    IEnumerator RespawnTimer(GatheringSpots gs, int newRandomTimes)
    {
        yield return new WaitForSeconds(5);
        gs.gatheringTimes = newRandomTimes;
        gs.gameObject.SetActive(true);
    }
}
