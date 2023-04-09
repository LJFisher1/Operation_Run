using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaFill : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [Range(0, 1)] [SerializeField] float volumeAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.playerController.MANA < GameManager.instance.playerController.manaMax)
            {
                GameManager.instance.UpdateScore(5, "Pickup");
                GameManager.instance.playerController.AddMana();
                GameManager.instance.playerController.PlayAud(audioClips, volumeAudio);
                Destroy(gameObject);
            }
        }
    }
}
