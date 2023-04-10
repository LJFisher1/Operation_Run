using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikeDamager : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float resetTime;
    [SerializeField] BoxCollider trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Damage(other.GetComponent<IDamage>()));
        }
    }

    IEnumerator Damage(IDamage idamage)
    {
        trigger.enabled = false;
        idamage.TakeDamage(damage);       
        yield return new WaitForSeconds(resetTime);
        trigger.enabled = true;
    }
}
