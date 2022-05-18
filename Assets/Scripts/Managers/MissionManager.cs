using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    [SerializeField] private MissionData missionData;
    private GameObject areaHolder;

    // make private after testing is done
    public List<int> areasNotSpawnable;
    private AreaClass[] areas;
    private float missionTime;

    private void Start()
    {
        missionTime = missionData.MissionTime;

        areasNotSpawnable = new List<int>();
        areaHolder = GameObject.FindGameObjectWithTag("AreaHolder");
        areas = new AreaClass[areaHolder.transform.childCount];
        for (int i = 0; i < areaHolder.transform.childCount; i++)
        {
            areas[i] = areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }

        for (int i = 0; i < missionData.AnimalsToSpawn.Count; i++)
        {
            AreaClass currentArea = GetRandomAreaSpawn(missionData.AnimalsToSpawn[i]); 
            for (int q = 0; q < (missionData.AnimalsToSpawn[i].Quantity); q++)
            {
                if(currentArea != null)
                {
                    Vector3 areaPos = currentArea.gameObject.transform.position;

                    float scaleX = currentArea.transform.localScale.x / 2;
                    float scaleZ = currentArea.transform.localScale.z / 2;

                    float randomX = Random.Range(-scaleX, scaleX);
                    float randomZ = Random.Range(-scaleZ, scaleZ);
                    Vector3 spawnPos = new Vector3(randomX, 0, randomZ) + currentArea.transform.position;
                    spawnPos.y = 10f;

                    AnimalClass animal = Instantiate(missionData.AnimalsToSpawn[i].AnimalClass, spawnPos, Quaternion.identity, this.transform);
                
                    animal.AnimalData = missionData.AnimalsToSpawn[i].Animal;
                    animal.CurrentArea = currentArea;
                }
            }
        }

    }
    private AreaClass GetRandomAreaSpawn(AnimalSpawnerClass animalData)
    {
        List<int> currentSpawnData = new List<int>(animalData.AreasToSpawn);
        for (int i = 0; i < animalData.AreasToSpawn.Count; i++)
        {
            int randomIndex = Random.Range(0, currentSpawnData.Count);
            if(!areasNotSpawnable.Contains(currentSpawnData[randomIndex]))
            {
                areasNotSpawnable.Add(currentSpawnData[randomIndex]);
                int GetAreaInt = currentSpawnData[randomIndex];
                AreaClass randomSpawnArea = areas[GetAreaInt - 1];
                return randomSpawnArea;
            }
            else
            {
                currentSpawnData.Remove(currentSpawnData[randomIndex]);
            }
        }
        return null;
    }
}
