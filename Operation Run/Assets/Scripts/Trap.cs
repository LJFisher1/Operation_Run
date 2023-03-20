using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour

{
    [SerializeField] int timer;
    [SerializeField] GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IEnumerator Expl()
            {
                yield return new WaitForSeconds(timer);
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
