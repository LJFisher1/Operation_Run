using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamage
{
    public void TakeDamage(int dmg)
    {
        if (GameManager.instance.playerController.weapon.canBreakWalls == true)
        {
            GameManager.instance.UpdateScore(5, "Wall");
            ++GameManager.instance.wallsBusted;
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int dmg, GameObject attacker)
    {
        EnemyAI enemy = attacker.GetComponent<EnemyAI>();
        if (enemy != null && enemy.canBreakWalls == true)
        {
            GameManager.instance.UpdateScore(10, "Wall");
            ++GameManager.instance.wallsBusted;
            Destroy(gameObject);
        }
    }
}
