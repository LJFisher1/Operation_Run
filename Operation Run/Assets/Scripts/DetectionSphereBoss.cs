using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSphereBoss : MonoBehaviour
{
    public BossIA enemy;
    private void Update()
    {
        transform.position = enemy.transform.position;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.playerInRange = false;
            enemy.agent.stoppingDistance = 0;
        }
    }
}