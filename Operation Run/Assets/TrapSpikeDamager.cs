using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikeDamager : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float resetTime;
    [SerializeField] BoxCollider trigger;

    [SerializeField] AudioClip[] audioHit;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float volume;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Damage(other.GetComponent<IDamage>()));
        }
    }

    IEnumerator Damage(IDamage idamage)
    {
        audioSource.PlayOneShot(audioHit[0], volume);
        trigger.enabled = false;
        idamage.TakeDamage(damage);       
        yield return new WaitForSeconds(resetTime);
        trigger.enabled = true;
    }
}
