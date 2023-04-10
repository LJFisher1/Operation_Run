using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    [SerializeField] AudioClip[] audioPickUp;
    [Range(0, 1)][SerializeField] float volumePickUp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore(5, "Key");
            ++GameManager.instance.keysCollected;
            GameManager.instance.playerController.KeyPickup();
            GameManager.instance.playerController.PlayAud(audioPickUp, volumePickUp);
            Destroy(gameObject);
        }
    }
}
