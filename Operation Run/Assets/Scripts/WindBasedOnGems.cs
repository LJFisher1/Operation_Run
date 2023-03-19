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
                GameManager.instance.playerController.ApplyForce(transform.forward * windSpeed);
            }
        }
    }
}
