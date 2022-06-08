using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Singleton<Weapons>
{
    public GameObject bloodParticle;

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
        if(other.gameObject.transform.tag == "AnimalMesh" || other.gameObject.transform.tag == "TrainingDummy")
        {
            if (HasDamaged)
            {
                return;
            }
            HasDamaged = true;

            GameObject BPGO = Instantiate(bloodParticle, other.gameObject.transform.position, Quaternion.identity, other.gameObject.transform);
            Destroy(BPGO, 1f);

            AnimalClass animal = other.gameObject.transform.GetComponentInParent<AnimalClass>();
            animal.TakeDamage(_damageValue);

            Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position);
            CanvasManager.instance.SpawnDamage(_damageValue, spawnPos);

            if (animal.IsPassive)
            {
                if (!animal.IsAgitated)
                {
                    animal.HasBeenAgitated();
                }
            }
        }
       
    }
}
