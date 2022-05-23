using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal Data", menuName = "Create Data/Animal/Animal Data")]
public class AnimalData : ScriptableObject
{
    [Tooltip("Animal Name")]
    [SerializeField] private string _animalName;
    
    [Tooltip("Animal Variables")]
    [SerializeField] private float _attackRange;
    [SerializeField] private float _aggroRange;
    [SerializeField] private float _outofRange;

    [SerializeField] private float _baseLoiterTime;
    [SerializeField] private float _baseWanderTime;

    [Tooltip("Animal Stats")]
    [SerializeField] private int _health;
    [SerializeField] private int _baseDamage;
    [SerializeField] private float _baseMoveSpeed;

    [SerializeField] private bool _isPassive;
    
    [Tooltip("Animal Weakness and Reistances")]
    [SerializeField] private List<Elements> _weakness;
    [SerializeField] private List<Elements> _resistance;
   
    [Tooltip("Animal Drops")]
    [SerializeField] private List<ItemDropsClass> _itemDrops;

    public string AnimalName { get { return _animalName; } set { _animalName = value; } }
    public float BaseMovementSpeed { get { return _baseMoveSpeed; } set { _baseMoveSpeed = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float AggroRange { get { return _aggroRange; } set { _aggroRange = value; } }
    public float OutofRange { get { return _outofRange; } set { _outofRange = value; } }
    public float BaseLoiterTime { get { return _baseLoiterTime; } set { _baseLoiterTime = value; } }
    public float BaseWanderTime { get { return _baseWanderTime; } set { _baseWanderTime = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }

    public List<Elements> Weakness { get { return _weakness; } set { _weakness = value; } }
    public List<Elements> Resistance { get { return _resistance; } set { _resistance = value; } }
    public List<ItemDropsClass> ItemDrops { get { return _itemDrops; } set { ItemDrops = value; } }

}
