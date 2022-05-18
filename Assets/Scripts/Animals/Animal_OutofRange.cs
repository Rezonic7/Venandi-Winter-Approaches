using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_OutofRange : MonoBehaviour
{
    private AnimalClass parentScript;
    private void Start()
    {
        parentScript = GetComponentInParent<AnimalClass>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            parentScript.IsPlayerInRange = false;
        }
    }
}
