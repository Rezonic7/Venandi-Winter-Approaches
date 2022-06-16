using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpotManager : Singleton<GatheringSpotManager>
{
    [SerializeField] private float respawnTime = 10f;
    public void Respawn(GatheringSpots gs, int newRandomTimes)
    {
        StartCoroutine(RespawnTimer(gs, newRandomTimes));
    }

    IEnumerator RespawnTimer(GatheringSpots gs, int newRandomTimes)
    {
        yield return new WaitForSeconds(respawnTime);
        gs.GatheringTimes = newRandomTimes;
        gs.gameObject.SetActive(true);
    }
}
