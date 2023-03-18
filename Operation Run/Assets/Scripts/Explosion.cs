using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] Collider col;
    [SerializeField] public AudioSource aud;
    
    [Header("----- Explosion Stats -----")]
    [SerializeField] int damage;
    [SerializeField] int explosionStrength;
    [SerializeField] bool pull;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audExplosion;
    [Range(0, 1)] [SerializeField] float audExplosionVol;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        model.enabled = false;
        col.enabled = false;
        aud.PlayOneShot(audExplosion[Random.Range(0, audExplosion.Length)], audExplosionVol);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerController.TakeDamage(damage);
            if (pull) // Pulls inwards
            {
                GameManager.instance.playerController.ApplyForce((transform.position - GameManager.instance.playerController.transform.position).normalized * explosionStrength);
            }
            else // Pushes outward
            {
                GameManager.instance.playerController.ApplyForce((transform.position + GameManager.instance.playerController.transform.position).normalized * explosionStrength);
            }
        }
    }
}
