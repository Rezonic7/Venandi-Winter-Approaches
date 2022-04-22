using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Singleton<Weapons>
{
    public int damageValue;
    public bool hasDamaged = false;

    private void Start()
    {
        hasDamaged = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TrainingDummy>() == null)
        {
            return;
        }
        if(hasDamaged)
        {
            return;
        }
        hasDamaged = true;
        TrainingDummy TD = other.gameObject.GetComponent<TrainingDummy>();
        TD.TakeDamage(damageValue);
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position);
        CanvasManager.instance.SpawnDamage(damageValue, spawnPos);
    }
}
