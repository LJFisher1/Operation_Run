using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsOfGold : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore(5);
            Destroy(gameObject);
        }
    }
}
