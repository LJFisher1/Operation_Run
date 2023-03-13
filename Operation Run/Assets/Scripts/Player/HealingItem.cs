using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [Range(5, 15)] [SerializeField] int heal;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerController.HP < GameManager.instance.playerController.hpMax)
        {
            GameManager.instance.playerController.Heal(heal);
            Destroy(gameObject);
        }
    }
}
