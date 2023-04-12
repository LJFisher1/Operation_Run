using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsOfPower : MonoBehaviour
{
    [SerializeField] AudioClip[] gemPickup;
    [Range(0, 1)] [SerializeField] float audioVol;
    private void Start()
    {
        GameManager.instance.GameUpdateGoal(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.instance.tutorialManager.CheckCompleted("Gem"))
            {
                StartCoroutine(GameManager.instance.FlashTutorialPopup("Gem"));
                GameManager.instance.tutorialManager.SetTutorialCompletion("Gem", true);
            }
            GameManager.instance.UpdateScore(10, "Gem");
            GameManager.instance.GemPickup();
            ++GameManager.instance.gemsCollected;
            GameManager.instance.playerController.PlayAud(gemPickup, audioVol);
            Destroy(gameObject);
        }
    }
}
