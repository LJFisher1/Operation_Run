using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour

{
    [SerializeField] int timer;
    [SerializeField] GameObject explosion;
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Expl());
           
        }
    }
    IEnumerator Expl()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        GameManager.instance.playerController.TakeDamage(damage);
        Destroy(gameObject);
    }
}
