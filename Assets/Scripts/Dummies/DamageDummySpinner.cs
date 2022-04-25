using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDummySpinner : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player_Variables>() != null)
        {
            Player_Variables.instance.TakeDamage(damage);
        }
    }
}
