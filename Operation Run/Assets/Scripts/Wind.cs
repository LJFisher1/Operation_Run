using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    [SerializeField] float windSpeed;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerController.windPushback(transform.forward * windSpeed);
        }
    }
}
