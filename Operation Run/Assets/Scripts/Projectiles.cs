using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int damage;
    [SerializeField] int timer;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamage>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
