using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Data", menuName = "Create Data/Create Equipment Data/Armor Data")]
public class ArmorData : ScriptableObject
{
    [SerializeField] private string _armorName;
    [SerializeField] private int _defenceValue;
    [SerializeField] private GameObject _modelPrefab;
    [SerializeField] private Sprite _imageSprite;

    public string ArmorName { get { return _armorName; } set { _armorName = value; } }
    public int DefenceValue { get { return _defenceValue; } set { _defenceValue = value; } }
    public GameObject ModelPrefab { get { return _modelPrefab; } set { _modelPrefab = value; } }
    public Sprite ImageSprite { get { return _imageSprite; } set { _imageSprite = value; } }
}
