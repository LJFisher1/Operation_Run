using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBars : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.instance.UpdateScore(5);
        }
    }
}
