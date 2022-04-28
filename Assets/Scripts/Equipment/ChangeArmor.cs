using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeArmor : MonoBehaviour
{
    [SerializeField] private ArmorData _armorData;
    public ArmorData ArmorData { get { return _armorData; } }
}
