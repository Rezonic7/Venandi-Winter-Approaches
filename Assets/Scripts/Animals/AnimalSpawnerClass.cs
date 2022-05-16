using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSpawnerClass
{
    [SerializeField] private AnimalClass _animal;
    [SerializeField] private int _quantity;
    [SerializeField] private List<int> _areasToSpawn;
    
    public AnimalClass Animal { get { return _animal; } set { _animal = value; } }
    public int Quantity { get { return _quantity; } set { _quantity = value; } }
    public List <int> AreasToSpawn { get { return _areasToSpawn; } set { _areasToSpawn = value; } }
}
