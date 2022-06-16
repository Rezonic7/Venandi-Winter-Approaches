using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission Data", menuName = "Create Data/Mission/Animal Spawn Data")]
public class MissionData : ScriptableObject
{
    [Header("Seconds")]
    [SerializeField] private float _missionTime = 60;
    [SerializeField] private int _playerLives = 1;
    [SerializeField] private List<AnimalSpawnerClass> _animalsToSpawn;
    [SerializeField] private ObjectiveTypes _objectiveType;
    [SerializeField] private AnimalData _huntObjective;
    [SerializeField] private ItemClass _gatherObjective;
    [SerializeField] private int _objectiveGoal;

    public AnimalData HuntObjective { get { return _huntObjective; } set { _huntObjective = value; } }
    public ItemClass GatherObjective { get { return _gatherObjective; } set { _gatherObjective = value; } }
    public int ObjectiveGoal { get { return _objectiveGoal; } set { _objectiveGoal = value; } }
    public ObjectiveTypes ObjectiveType { get { return _objectiveType; } set { _objectiveType = value; } }
    public List<AnimalSpawnerClass> AnimalsToSpawn { get { return _animalsToSpawn; } set { _animalsToSpawn = value; } }
    public int PlayerLives { get { return _playerLives; } set { _playerLives = value; } }
    public float MissionTime { get { return _missionTime; } set { _missionTime = value; } }

    public enum ObjectiveTypes
    {
        Gather,
        Hunt,
    }

}
