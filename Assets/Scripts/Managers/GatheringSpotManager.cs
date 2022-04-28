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
        yield return new WaitForSeconds(10);
        gs.GatheringTimes = newRandomTimes;
        gs.gameObject.SetActive(true);
    }
}
