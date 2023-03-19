using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.PickupHealItem();
            Destroy(gameObject);
            GameManager.instance.UpdateScore(5);
        }
    }
}
