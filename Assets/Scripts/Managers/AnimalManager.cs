using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : Singleton<AnimalManager>
{
    [SerializeField] private AnimalSpawnerData spawnData;
    private GameObject areaHolder;
    public List<int> areasNotSpawnable;
    private AreaClass[] areas;

    private void Start()
    {
        areaHolder = GameObject.FindGameObjectWithTag("AreaHolder");
        areas = new AreaClass[areaHolder.transform.childCount];
        for (int i = 0; i < areaHolder.transform.childCount; i++)
        {
            areas[i] = areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }

        for (int i = 0; i < spawnData.AnimalsToSpawn.Count; i++)
        {
            AreaClass currentArea = GetRandomAreaSpawn(spawnData.AnimalsToSpawn[i]);
            for (int q = 0; q < (spawnData.AnimalsToSpawn[i].Quantity - 1); q++)
            {
                if(currentArea != null)
                {
                    Vector3 spawnPos = currentArea.gameObject.transform.position;
                    spawnPos.y = 10f;
                    AnimalClass animal = Instantiate(spawnData.AnimalsToSpawn[i].Animal, spawnPos, Quaternion.identity, this.transform);
                    animal.CurrentArea = currentArea;
                }
            }
        }

    }
    private AreaClass GetRandomAreaSpawn(AnimalSpawnerClass animalData)
    {
        List<int> spawnIntData = new List<int>(animalData.AreasToSpawn);
        int randomSpawnInt = 0;
        int repeatCount = 0;
        for (int i = 0; i < animalData.AreasToSpawn.Count; i++)
        {
            for(int h = 0; h < spawnIntData.Count; h++)
            {
                Debug.Log(spawnIntData[h] + " Index:" + h);
            }
            //Debug.Log(spawnIntData.Count);
            randomSpawnInt = Random.Range(0, spawnIntData.Count);
            if(!areasNotSpawnable.Contains(spawnIntData[randomSpawnInt]))
            {
                areasNotSpawnable.Add(spawnIntData[randomSpawnInt]);
                break;
            }
            spawnIntData.Remove(spawnIntData[randomSpawnInt]);
            repeatCount++;
        }
        if(repeatCount >= animalData.AreasToSpawn.Count)
        {
            return null;
        }
        int GetAreaInt = animalData.AreasToSpawn[randomSpawnInt] - 1;
        AreaClass randomSpawnArea = areas[GetAreaInt];
        return randomSpawnArea;
    }
}
