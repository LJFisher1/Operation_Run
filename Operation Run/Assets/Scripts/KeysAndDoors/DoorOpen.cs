using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession >= 1)
        {
            GameManager.instance.playerController.KeyUsed();
            GameManager.instance.usedKey1();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession <= 0)
        {
            StartCoroutine(GameManager.instance.noKeysFlash());
        }
    }
}