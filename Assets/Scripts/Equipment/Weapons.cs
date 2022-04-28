using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Singleton<Weapons>
{
    private int _damageValue;
    private bool _hasDamaged;
    public int DamageValue { get { return _damageValue; } set { _damageValue = value; } }
    public bool HasDamaged { get { return _hasDamaged; } set { _hasDamaged = value; } }

    private void Start()
    {
        HasDamaged = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TrainingDummy>() == null)
        {
            return;
        }
        if(HasDamaged)
        {
            return;
        }
        HasDamaged = true;
        TrainingDummy TD = other.gameObject.GetComponent<TrainingDummy>();
        TD.TakeDamage(_damageValue);
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position);
        CanvasManager.instance.SpawnDamage(_damageValue, spawnPos);
    }
}
