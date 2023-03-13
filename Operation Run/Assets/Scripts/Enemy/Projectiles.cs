using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int damage;
    public string hitTag = "Player";
    public int timer;
    

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
        Destroy(gameObject);
    }
}
