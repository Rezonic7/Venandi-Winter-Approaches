using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDummyArrow : MonoBehaviour
{
    public float arrowSpeed;
    public int damage;
    private void Update()
    {
        transform.Translate(-this.gameObject.transform.forward * Time.deltaTime * arrowSpeed);
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player_Variables>() != null)
        {
            Player_Variables.instance.TakeDamage(damage);
            Destroy(gameObject);

        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
