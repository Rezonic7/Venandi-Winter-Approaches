using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalClass : ScriptableObject
{
    private string _animalName;
    private int _health;
    private float _baseMoveSpeed;
    private int _damage;
    private bool _isPassive;

    private Elements _weakness;
    private Elements _resistance;

    private List<MiscClass> itemDrops;

}
