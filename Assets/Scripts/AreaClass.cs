using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaClass : MonoBehaviour
{
    [SerializeField] private List<AreaClass> _neighboringArea;
    public List<AreaClass> NeighboringAreas { get { return _neighboringArea; } }
}
