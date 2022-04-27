using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float arrowSpeed;
    public int damage;
    private Rigidbody rb;
    private void Start()
    {
         rb = GetComponent<Rigidbody>();
         rb.AddForce(transform.forward * Time.deltaTime * arrowSpeed, ForceMode.Impulse);
    }
    private void Update()
    {
        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.GetComponent<TrainingDummy>() != null)
        {
            TrainingDummy TD = collision.transform.gameObject.GetComponent<TrainingDummy>();
            TD.TakeDamage(damage);
            Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position);
            CanvasManager.instance.SpawnDamage(damage, spawnPos);

            Destroy(gameObject);
        }

    }

}
