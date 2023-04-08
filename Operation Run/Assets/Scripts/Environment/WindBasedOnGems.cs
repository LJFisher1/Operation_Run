using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBasedOnGems : MonoBehaviour
{

    [SerializeField] float windSpeed;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (GameManager.instance.GemsRemaining > 0)
            {
                StartCoroutine(GameManager.instance.FlashTutorialPopup("Gem Wind"));
                GameManager.instance.playerController.ApplyForce(transform.forward * windSpeed);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.GemsRemaining > 0 && GameManager.instance.tutorialManager.CheckCompleted("Gem Wind"))
        {
            
            StartCoroutine(GameManager.instance.FlashTutorialPopup("Gem Wind"));
        }
        else
        {
            GameManager.instance.tutorialManager.SetTutorialCompletion("Gem Wind", true);
        }
    }
}
