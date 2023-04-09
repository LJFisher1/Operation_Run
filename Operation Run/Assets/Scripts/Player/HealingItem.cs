using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [Range(0, 1)][SerializeField] float volumeAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore(5, "Pickup");
            GameManager.instance.playerController.PickupHealItem();
            GameManager.instance.playerController.PlayAud(audioClips, volumeAudio);
            Destroy(gameObject);
        }
    }
}
