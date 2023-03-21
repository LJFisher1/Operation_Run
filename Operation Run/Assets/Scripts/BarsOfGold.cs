using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsOfGold : MonoBehaviour
{
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audGoldPickup;
    [Range(0, 1)] [SerializeField] float audGoldPickupVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore(5);
            //aud.PlayOneShot(audGoldPickup[Random.Range(0, audGoldPickup.Length)], audGoldPickupVol);
            GameManager.instance.playerController.pickUpLootSound(audGoldPickup, audGoldPickupVol);
            Destroy(gameObject);
        }
    }
}
