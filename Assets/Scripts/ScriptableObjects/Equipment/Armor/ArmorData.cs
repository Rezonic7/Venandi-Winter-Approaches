using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Data", menuName = "Create Data/Create Equipment Data/Armor Data")]
public class ArmorData : ScriptableObject
{
    [SerializeField] private string _armorName;
    [SerializeField] private int _defenceValue;
    [SerializeField] private GameObject _modelPrefab;

    public string ArmorName { get { return _armorName; } set { _armorName = value; } }
    public int DefenceValue { get { return _defenceValue; } set { _defenceValue = value; } }
    public GameObject ModelPrefab { get { return _modelPrefab; } set { _modelPrefab = value; } }
}
