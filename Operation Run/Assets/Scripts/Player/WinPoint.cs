using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.enemiesRemaining < 1)
        {
            GameManager.instance.PlayerWin();
        }
    }

}
