using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_HurtBox : MonoBehaviour
{
    private AnimalClass parentAnimal;

    private void Start()
    {
        parentAnimal = transform.GetComponentInParent<AnimalClass>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //if (parentAnimal.HasDamaged)
            //{
            //    return;
            //}
            Player_Variables.instance.TakeDamage(parentAnimal.TotalDamage);
            parentAnimal.HasDamaged = true;
        }
    }
}
