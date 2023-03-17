using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public string hitTag = "Player";
    public int timer;
    [SerializeField] GameObject hitEffect;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        
        if (other.CompareTag(hitTag))
        {
            other.GetComponent<IDamage>().TakeDamage(damage);
        }
        if (hitEffect)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
