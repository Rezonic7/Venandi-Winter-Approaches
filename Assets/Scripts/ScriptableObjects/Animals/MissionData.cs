using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission Data", menuName = "Create Data/Mission/Animal Spawn Data")]
public class MissionData : ScriptableObject
{
    [SerializeField] private float _missionTime;
    [SerializeField] private List<AnimalSpawnerClass> _animalsToSpawn;
    public List<AnimalSpawnerClass> AnimalsToSpawn { get { return _animalsToSpawn; } set { _animalsToSpawn = value; } }
    public float MissionTime { get { return _missionTime; } set { _missionTime = value; } }
}
