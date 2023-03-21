using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamage
{
    public void TakeDamage(int dmg)
    {
        if (GameManager.instance.playerController.weapon.canBreakWalls == true)
        {
            Destroy(gameObject);
        }
    }
}
