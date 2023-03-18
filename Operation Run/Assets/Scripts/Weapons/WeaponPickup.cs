using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] bool destroyOnPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.ChangeWeapon(weapon);
            if(destroyOnPickup) Destroy(gameObject);
        }
    }
}
