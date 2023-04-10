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
            GameManager.instance.UpdateScore(10, "Gold");
            ++GameManager.instance.goldCollected;
            GameManager.instance.playerController.PlayAud(audGoldPickup, audGoldPickupVol);
            Destroy(gameObject);
        }
    }
}
