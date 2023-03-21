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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Expl());
        }
    }
    IEnumerator Expl()
    {
        GameManager.instance.playerController.PlayAud(trapAud, trapAudVol);
        yield return new WaitForSeconds(timer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
