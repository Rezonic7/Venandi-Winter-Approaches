using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_AggroRange : MonoBehaviour
{
    private AnimalClass parentScript;
    private void Start()
    {
        parentScript = GetComponentInParent<AnimalClass>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            parentScript.IsPlayerInRange = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            parentScript.IsPlayerInRange = true;
        }
    }
}
