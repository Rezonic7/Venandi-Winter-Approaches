using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDummy : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject arrowSpawnPos;
    public float shootCDDuration;

    private Animator anim;
    private float shootCDTimer;
    private bool animPlayed;

    private void Start()
    {
        shootCDTimer = shootCDDuration;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(shootCDTimer - 0.5f <= 0)
        {
            if(!animPlayed)
            {
                anim.SetTrigger("Fire");
                animPlayed = true;
            }
            
        }
        if (shootCDTimer <= 0)
        {
            shootCDTimer = shootCDDuration;
            animPlayed = false;
            Instantiate(arrowPrefab, arrowSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            shootCDTimer -= Time.deltaTime;
        }
    }
}
