using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Set of Animals to Spawn", menuName = "Create Data/Animal/Animal Spawn Data")]
public class AnimalSpawnerData : ScriptableObject
{
    [SerializeField] private List<AnimalSpawnerClass> _animalsToSpawn;

    public List<AnimalSpawnerClass> AnimalsToSpawn { get { return _animalsToSpawn; } set { _animalsToSpawn = value; } }
}
