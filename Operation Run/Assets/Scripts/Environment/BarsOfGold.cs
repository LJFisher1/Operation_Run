using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsOfGold : MonoBehaviour
{
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audGoldPickup;
    [Range(0, 1)] [SerializeField] float audGoldPickupVol;
    [Header("--- score value ---")]
    [SerializeField] int scoreValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore(scoreValue, "Gold");
            ++GameManager.instance.goldCollected;
            GameManager.instance.playerController.PlayAud(audGoldPickup, audGoldPickupVol);
            Destroy(gameObject);
        }
    }
}
