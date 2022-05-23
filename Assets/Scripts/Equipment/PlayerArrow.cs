using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public GameObject bloodParticle;

    [SerializeField] private float arrowSpeed;
    private Rigidbody rb;
    private int _damageValue;
    
    public int Damage { set { _damageValue = value; } }

    private void Start()
    {
         rb = GetComponent<Rigidbody>();
         rb.AddForce(transform.forward * Time.deltaTime * arrowSpeed, ForceMode.Impulse);
    }
    private void Update()
    {
        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag != "AnimalMesh")
        {
            return;
        }

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
