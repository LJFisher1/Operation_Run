using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.GemsRemaining <= 0)
            {
                GameManager.instance.PlayerWin();
            }
            else
            {
                StartCoroutine(GameManager.instance.FlashTutorialPopup("Gem"));
            }
        }
    }
}
