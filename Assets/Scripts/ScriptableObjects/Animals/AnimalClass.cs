using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalClass : ScriptableObject
{
    [SerializeField] private string _animalName;
    [SerializeField] private int _health;
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private int _baseDamage;
    [SerializeField] private float _attackRange;
    [SerializeField] private bool _isPassive;

    [SerializeField] private Elements _weakness;
    [SerializeField] private Elements _resistance;

    [SerializeField] private List<MiscClass> _itemDrops;

    public string AnimalName { get { return _animalName; } set { _animalName = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public float BaseMovementSpeed { get { return _baseMoveSpeed; } set { _baseMoveSpeed = value; } }
    public int Damage { get { return _baseDamage; } set { _baseDamage = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }

    public Elements Weakness { get { return _weakness; } set { _weakness = value; } }
    public Elements Resistance { get { return _resistance; } set { _resistance = value; } }

    public List<MiscClass> ItemDrops { get { return _itemDrops; } set { ItemDrops = value; } }

}
