using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_AggroRange : MonoBehaviour
{
    private LargeAnimal parentScript;
    private void Start()
    {
        parentScript = GetComponentInParent<LargeAnimal>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if(!parentScript.IsPassive)
            {
                parentScript.IsPlayerInRange = true;
            }
        }
    }
}
