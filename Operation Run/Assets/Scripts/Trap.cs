using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour

{
    [SerializeField] int timer;
    [SerializeField] GameObject explosion;
    [SerializeField] int damage;

    [Header("---- Audio ----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] trapAud;
    [Range(0, 1)] [SerializeField] float trapAudVol;

    //IEnumerator playAud()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    aud.PlayOneShot(trapAud[Random.Range(0, trapAud.Length)], trapAudVol);
    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // StartCoroutine(playAud());
            StartCoroutine(Expl());
        }
    }
    IEnumerator Expl()
    {
        GameManager.instance.playerController.pickUpLootSound(trapAud, trapAudVol);
        yield return new WaitForSeconds(timer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        GameManager.instance.playerController.TakeDamage(damage);
        Destroy(gameObject);
    }
}
