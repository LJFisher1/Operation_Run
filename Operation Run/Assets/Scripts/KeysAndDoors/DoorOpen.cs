using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] AudioClip[] doorOpen;
    [Range(0, 1)] [SerializeField] float volumeAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession >= 1)
        {
            GameManager.instance.playerController.pickUpLootSound(doorOpen, volumeAudio);
            GameManager.instance.playerController.KeyUsed();
            GameManager.instance.usedKey1();
            Destroy(gameObject);
            GameManager.instance.UpdateScore(5);
        }
        else if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession <= 0)
        {
            StartCoroutine(GameManager.instance.noKeysFlash());
        }
    }
}