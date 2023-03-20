using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamage
{
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (GameManager.instance.playerController.isUsingWeapon == true && GameManager.instance.playerController.weapon.canBreakWalls == true)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    public void TakeDamage(int dmg)
    {
        if (GameManager.instance.playerController.weapon.canBreakWalls == true)
        {
            Destroy(gameObject);
        }
    }
}
