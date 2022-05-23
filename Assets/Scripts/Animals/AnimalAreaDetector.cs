using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAreaDetector : MonoBehaviour
{
    private AnimalClass parentAnimal;
    private float baseSpeed;
    private float puddleSpeed;
    private void Start()
    {
        parentAnimal = GetComponentInParent<AnimalClass>();
        baseSpeed = parentAnimal.Agent.speed;
        puddleSpeed = baseSpeed - (baseSpeed * 0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Puddle")
        {
            parentAnimal.Agent.speed = puddleSpeed;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Puddle")
        {
            parentAnimal.Agent.speed = baseSpeed;
        }
    }
   
}
