using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public EnemyAI enemy;
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
