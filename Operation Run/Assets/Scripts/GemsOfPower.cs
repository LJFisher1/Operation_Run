using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsOfPower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.GemPickup();
            Destroy(gameObject);
            GameManager.instance.UpdateScore(10);
        }
    }
}
