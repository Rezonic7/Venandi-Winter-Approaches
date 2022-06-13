using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvingSpot : MonoBehaviour
{
    [SerializeField] private int carvingTimes;

    private int _gatheringTimes;
    private bool _isGatherable;
    private Collider carvingCollider;
    private AnimalClass parent;

    public bool IsGatherable { get { return _isGatherable; } set { _isGatherable = value; } }

    private AnimalClass parentClass;

    void Start()
    {
        parent = GetComponentInParent<AnimalClass>();
        carvingCollider = GetComponent<Collider>();
        parentClass = GetComponentInParent<AnimalClass>();
        _gatheringTimes = carvingTimes;
        IsGatherable = true;
    }

    float total;

    public ItemDropsClass GatherItem()
    {
       
        foreach (var item in parentClass.AnimalData.ItemDrops)
        {
            total += item.GatherPercentage;
        }
        float random = Random.Range(0, total);
        foreach (var item in parentClass.AnimalData.ItemDrops)
        {
            if (random <= item.GatherPercentage)
            {
                _gatheringTimes -= 1;
                total = 0;
                if (_gatheringTimes <= 0)
                {
                    _isGatherable = false;
                    carvingCollider.enabled = false;
                    if(parent.gameObject)
                    {
                        Destroy(parent.gameObject, 10f);
                    }
                }
                return item;
            }
            else
            {
                random -= item.GatherPercentage;
            }
        }
        total = 0;
        return null;
    }
}
