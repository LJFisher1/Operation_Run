using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsOfPower : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.GameUpdateGoal(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GemPickup();
            Destroy(gameObject);
            GameManager.instance.UpdateScore(10);
        }
    }
}
